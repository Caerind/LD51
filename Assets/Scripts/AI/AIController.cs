using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AIController : Soldier
{
    [SerializeField] private float fireCooldownBonusAI = 0.5f;

    [SerializeField] private float maxDistanceLookBrain = 40.0f;

    private InterestPoint point = null;
    private bool justFoundPoint = false;

    private FakeAgent agent;

    private void Awake()
    {
        AwakeSoldier(isPlayer: false);
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnDeath += HealthSystem_OnDied;
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        if (sender != null)
        {
            Soldier senderSoldier = (Soldier)sender;
            if (senderSoldier != null && senderSoldier.IsPlayerSoldier())
            {
                SetLookDir((senderSoldier.transform.position - transform.position).ToVector2().normalized);
            }
        }
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        GetGeneral().RemoveRefToSoldier(this);

        // Spawn new entity
        Transform pfDeadBody = Resources.Load<Transform>("pfDeadBody");
        Instantiate(pfDeadBody, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void Update()
    {
        UpdateSoldier();

        if (IsMainSoldier())
        {
            UpdateMainAI();
        }
        else
        {
            UpdateReactions();
        }
    }

    public override void SetMainSoldier(bool mainSoldier)
    {
        base.SetMainSoldier(mainSoldier);

        agent.SetStopped(!mainSoldier);
    }

    public void SetFakeAgent(FakeAgent agent)
    {
        this.agent = agent;
    }

    protected override bool CanFire()
    {
        return timerFire >= (fireCooldown + fireCooldownBonusAI + (IsMainSoldier() ? 0.0f : fireCooldownBonusReaction));
    }

    private void UpdateMainAI()
    {
        if (point == null)
        {
            List<InterestPoint> points = new List<InterestPoint>(FindObjectsOfType<InterestPoint>());
            if (points.Count > 0)
            {
                // Go to closest point
                float bestSqrDistance = float.PositiveInfinity;
                InterestPoint bestPoint = null;
                foreach (InterestPoint iPoint in points)
                {
                    float sqrDistance = (transform.position.ToVector2() - iPoint.transform.position.ToVector2()).sqrMagnitude;
                    if (sqrDistance < bestSqrDistance)
                    {
                        bestSqrDistance = sqrDistance;
                        bestPoint = iPoint;
                    }
                }
                point = bestPoint;
            }
            justFoundPoint = true;

        }
        else if (justFoundPoint)
        {
            agent.SetDestination(point.transform.position);
            justFoundPoint = false;
        }

        agent.SetSpeed(GetSpeed());
    }

    private void LateUpdate()
    {
        if (IsMainSoldier())
        {
            Vector2 prevPos = transform.position.ToVector2();
            transform.position = agent.transform.position;

            Vector2 mvt = transform.position.ToVector2() - prevPos;
            if (mvt != Vector2.zero)
            {
                SetLookDir(mvt.normalized);
            }
            else if (point != null && (transform.position - point.transform.position).sqrMagnitude < 1.0f * 1.0f)
            {
                SetLookDir(point.GetLookDir());
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + GetLookDir().ToVector3() * 3.0f);
    }

    public List<Objective> ScanObjectives()
    {
        List<Objective> results = new List<Objective>();

        // ReachInterestPoint
        {
            List<InterestPoint> interestPoints = GameManager.Instance.GetAllInterestPoints();
            foreach (var point in interestPoints)
            {
                if (!point.IsLocked() && (point.transform.position - transform.position).sqrMagnitude < maxDistanceLookBrain * maxDistanceLookBrain)
                {
                    float score = 0.0f; // point.GetBaseScore();
                    // TODO : Improve compute score
                    results.Add(new Objective(ObjectiveType.ReachInterestPoint, point.gameObject, gameObject, score));
                }
            }
        }

        // AttackPlayerZone
        {
            List<ZonePoint> playerZonePoints = GameManager.Instance.GetAllPlayerZonePoints();

            ZonePoint bestPoint = null;
            float bestScore = -1.0f;

            float distanceCheck = maxDistanceLookBrain;

            AIGeneral general = (AIGeneral)GetGeneral();
            if (general != null && general.GetPlayerZone().GetCompteur() > 0)
            {
                distanceCheck *= general.GetCommander().factorDistanceAttackPlayerZone;
            }

            foreach (var point in playerZonePoints)
            {
                float d = (point.transform.position - transform.position).sqrMagnitude;
                if (d < maxDistanceLookBrain * maxDistanceLookBrain)
                {
                    // Nearest point is best
                    float score = distanceCheck * distanceCheck - d;

                    if (score > bestScore)
                    {
                        bestPoint = point;
                        bestScore = score;
                    }
                }
            }

            if (bestPoint != null)
            {
                results.Add(new Objective(ObjectiveType.AttackPlayerZone, bestPoint.gameObject, gameObject, bestScore));
            }
        }

        // DefendEnemyZone
        {
            List<ZonePoint> enemyZonePoints = GameManager.Instance.GetAllEnemyZonePoints();

            ZonePoint bestPoint = null;
            float bestScore = -1.0f;

            float distanceCheck = maxDistanceLookBrain;

            AIGeneral general = (AIGeneral)GetGeneral();
            if (general != null && general.GetEnemyZone().GetCompteur() > 0)
            {
                distanceCheck *= general.GetCommander().factorDistanceDefendEnemyZone;
            }

            foreach (var point in enemyZonePoints)
            {
                float d = (point.transform.position - transform.position).sqrMagnitude;
                if (d < distanceCheck * distanceCheck)
                {
                    // Nearest point is best
                    float score = maxDistanceLookBrain * maxDistanceLookBrain - d;

                    if (score > bestScore)
                    {
                        bestPoint = point;
                        bestScore = score;
                    }
                }
            }

            if (bestPoint != null)
            {
                results.Add(new Objective(ObjectiveType.DefendEnemyZone, bestPoint.gameObject, gameObject, bestScore));
            }
        }

        // AttackPlayer
        {
            List<Soldier> playerSoldiers = GameManager.Instance.GetPlayerGeneral().GetSoldiers();

            Soldier bestPlayer = null;
            float bestScore = -1.0f;

            foreach (var soldier in playerSoldiers)
            {
                // *4 (2^2) here because, we can move+shoot
                if ((soldier.transform.position - transform.position).sqrMagnitude < 4.0f * maxDistanceLookBrain * maxDistanceLookBrain)
                {
                    float score = 100.0f - soldier.GetHealthSystem().GetHealthAmount();

                    if (score > bestScore)
                    {
                        bestPlayer = soldier;
                        bestScore = score;
                    }
                }
            }

            if (bestPlayer != null)
            {
                results.Add(new Objective(ObjectiveType.AttackPlayer, bestPlayer.gameObject, gameObject, bestScore));
            }
        }

        return results;
    }
}
