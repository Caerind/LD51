using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AIBrain : MonoBehaviour
{
    [SerializeField] private float timeRecomputePath = 0.2f;
    [SerializeField] private float timeSkipBrain = 0.05f;

    public enum Pattern
    {
        NormalMan,
        CacMan,
        RushZoneMan,
        DefendMan,
        StationaryMan
    }
    [SerializeField] private Pattern defaultPattern;
    private Pattern currentPattern;

    private AIController controller;
    private Vector2 targetPosition;

    private InterestPoint targetPoint = null;
    private Soldier targetSoldier = null;

    private float timerRecomputePath = 0.0f;
    private float timerSkipBrain = 0.0f;

    private void Awake()
    {
        controller = GetComponent<AIController>();
        targetPosition = transform.position.ToVector2();
        currentPattern = defaultPattern;
    }

    private void Update()
    {
        timerRecomputePath += Time.deltaTime;
        timerSkipBrain += Time.deltaTime;

        if (timerSkipBrain >= timeSkipBrain)
        {
            timerSkipBrain -= timeSkipBrain;
            if (controller.IsMainSoldier())
            {
                Pattern newPattern;
                if (GameManager.Instance.GetAIZone().GetEnemyInZoneCounter() > 0)
                {
                    if (defaultPattern == Pattern.CacMan)
                    {
                        newPattern = Pattern.CacMan;
                        targetSoldier = GetPlayerProbablyInOurZone();
                    }
                    else
                    {
                        newPattern = Pattern.DefendMan;
                    }
                }
                else
                {
                    newPattern = defaultPattern;
                }
                if (newPattern != currentPattern)
                {
                    UnlockPoint();
                    currentPattern = newPattern;
                }

                switch (currentPattern)
                {
                    case Pattern.NormalMan: UpdateNormalMan(); break;
                    case Pattern.CacMan: UpdateCacMan(); break;
                    case Pattern.RushZoneMan: UpdateRushZoneMan(); break;
                    case Pattern.DefendMan: UpdateDefendMan(); break;
                    case Pattern.StationaryMan: UpdateStationaryMan(); break;
                }
            }
        }
    }

    private void UpdateNormalMan()
    {
        Soldier target = GetNearestVisiblePlayer();
        bool targetVisible = true;
        if (target == null)
        {
            targetVisible = false;
            target = GetNearestPlayer(transform.position);
        }
        if (target != null)
        {
            float distanceSearchMaxSqr = (target.transform.position.ToVector2() - transform.position.ToVector2()).sqrMagnitude;
            InterestPoint point = GetBestPointToShootAtPlayer(target, distanceSearchMaxSqr);
            if (point != null && point != targetPoint)
            {
                UnlockPoint();
                targetPoint = point;
                targetPoint.SetIsLocked(true);
                SetTargetPosition(targetPoint.transform.position);
                timerRecomputePath = 0.0f;
            }
            targetSoldier = target;
        }

        if (HasReachTarget() || targetPoint == null)
        {
            if (targetVisible)
            {
                controller.SetLookAt(targetSoldier.transform.position);

                float d = (targetSoldier.transform.position.ToVector2() - transform.position.ToVector2()).sqrMagnitude;
                if (d < 1.5f * 1.5f)
                {
                    if (controller.CanCac())
                    {
                        controller.Cac();
                    }
                }
                else if (d < controller.GetFireDistance() * controller.GetFireDistance())
                {
                    if (controller.CanFire())
                    {
                        controller.Fire(reactionFire: false);
                    }
                }
            }
            else if (targetPoint != null)
            {
                controller.SetLookDir(targetPoint.GetLookDir());
            }
        }
    }

    private void UpdateCacMan()
    {
        // Update nearest player target
        Soldier target = GetNearestPlayer(transform.position);
        if (target != null && targetSoldier != target)
        {
            if (targetSoldier == null)
            {
                SetTargetPosition(target.transform.position);
                timerRecomputePath = 0.0f;
            }
            targetSoldier = target;
        }

        // Recompute path in case target has moved since last time
        if (targetSoldier != null && timerRecomputePath >= timeRecomputePath)
        {
            timerRecomputePath -= timeRecomputePath;
            SetTargetPosition(targetSoldier.transform.position);
        }

        // If close : cac
        if (targetSoldier != null)
        {
            float d = (targetSoldier.transform.position.ToVector2() - transform.position.ToVector2()).sqrMagnitude;
            if (d < 1.5f * 1.5f)
            {
                controller.StopMoving();
                if (controller.CanCac())
                {
                    controller.Cac();
                }
            }
        }
    }

    private void UpdateRushZoneMan()
    {
        if (timerRecomputePath >= timeRecomputePath)
        {
            List<ZonePoint> playerZonePoints = GameManager.Instance.GetAllPlayerZonePoints();
            ZonePoint bestPoint = null;
            float bestScore = 9999999.9f;
            foreach (var point in playerZonePoints)
            {
                float d = (point.transform.position - transform.position).sqrMagnitude;
                if (d < bestScore)
                {
                    bestPoint = point;
                    bestScore = d;
                }
            }
            if (bestPoint != null)
            {
                SetTargetPosition(bestPoint.transform.position);
            }
        }

        {
            Soldier soldier = GetNearestVisiblePlayer();
            if (soldier != null)
            {
                controller.SetLookAt(soldier.transform.position);

                float d = (soldier.transform.position.ToVector2() - transform.position.ToVector2()).sqrMagnitude;
                if (d < 1.5f * 1.5f)
                {
                    if (controller.CanCac())
                    {
                        controller.Cac();
                    }
                }
                else if (d < controller.GetFireDistance() * controller.GetFireDistance())
                {
                    if (controller.CanFire())
                    {
                        controller.Fire(reactionFire: false);
                    }
                }
            }
        }
    }

    private void UpdateDefendMan()
    {
        if (timerRecomputePath >= timeRecomputePath)
        {
            Vector2 searchPos = transform.position;

            Soldier soldier = GetPlayerProbablyInOurZone();
            if (soldier != null)
            {
                searchPos = (searchPos + soldier.transform.position.ToVector2()) * 0.5f;
            }

            List<ZonePoint> enemyZonePoints = GameManager.Instance.GetAllAIZonePoints();
            ZonePoint bestPoint = null;
            float bestScore = 9999999.9f;
            foreach (var point in enemyZonePoints)
            {
                float d = (point.transform.position.ToVector2() - searchPos).sqrMagnitude;
                if (d < bestScore)
                {
                    bestPoint = point;
                    bestScore = d;
                }
            }
            if (bestPoint != null)
            {
                SetTargetPosition(bestPoint.transform.position);
            }
        }

        {
            Soldier soldier = GetNearestVisiblePlayer();
            if (soldier != null)
            {
                controller.SetLookAt(soldier.transform.position);

                float d = (soldier.transform.position.ToVector2() - transform.position.ToVector2()).sqrMagnitude;
                if (d < 1.5f * 1.5f)
                {
                    if (controller.CanCac())
                    {
                        controller.Cac();
                    }
                }
                else if (d < controller.GetFireDistance() * controller.GetFireDistance())
                {
                    if (controller.CanFire())
                    {
                        controller.Fire(reactionFire: false);
                    }
                }
            }
        }
    }

    private void UpdateStationaryMan()
    {
        {
            Soldier soldier = GetNearestVisiblePlayer();
            if (soldier != null)
            {
                controller.SetLookAt(soldier.transform.position);

                float d = (soldier.transform.position.ToVector2() - transform.position.ToVector2()).sqrMagnitude;
                if (d < 1.5f * 1.5f)
                {
                    if (controller.CanCac())
                    {
                        controller.Cac();
                    }
                }
                else if (d < controller.GetFireDistance() * controller.GetFireDistance())
                {
                    if (controller.CanFire())
                    {
                        controller.Fire(reactionFire: false);
                    }
                }
            }
        }
    }

    private void SetTargetPosition(Vector2 targetPos)
    {
        targetPosition = targetPos;

        Vector2 diff = (targetPos - transform.position.ToVector2());
        if (diff.sqrMagnitude >= 1.0f * 1.0f)
        {
            controller.SetTargetPosition(targetPos);
        }
    }

    public bool HasReachTarget(float distance = 1.0f)
    {
        Vector2 diff = (targetPosition - transform.position.ToVector2());
        if (diff.sqrMagnitude < distance * distance)
        {
            return true;
        }
        return false;
    }

    public void UnlockPoint()
    {
        if (targetPoint != null)
        {
            targetPoint.SetIsLocked(false);
        }
    }

    public List<Soldier> GetAllPlayers()
    {
        return GameManager.Instance.GetPlayerGeneral().GetSoldiers();
    }
    public Soldier GetNearestVisiblePlayer()
    {
        var players = GetAllPlayers();
        Soldier bestPlayer = null;
        float bestSqrScore = 999999999.9f;
        foreach (var player in players)
        {
            Vector2 aiPos = transform.position.ToVector2();
            Vector2 playerPos = player.transform.position.ToVector2();
            Vector2 diff = (playerPos - aiPos);
            Vector2 diffNormalized = diff.normalized;

            float d = diff.sqrMagnitude;
            if (d > 2.9f * 2.9f)
            {
                bool visible = false;
                // Do a raycast to check is there is obstacle between us
                LayerMask mask = LayerMask.GetMask("Default");
                RaycastHit2D hit = Physics2D.Raycast(aiPos + diff * 0.8f, diff, 50.0f, mask);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject == player.gameObject /*|| (fakeAgent != null && fakeAgent.GetSoldier() == soldier)*/)
                    {
                        visible = true;
                    }
                }
                if (!visible)
                {
                    continue;
                }
            }

            if (d < bestSqrScore)
            {
                bestPlayer = player;
                bestSqrScore = d;
            }
        }
        return bestPlayer;
    }
    public Soldier GetNearestPlayer(Vector2 pos)
    {
        var players = GetAllPlayers();
        Soldier bestPlayer = null;
        float bestSqrScore = 999999999.9f;
        foreach (var player in players)
        {
            float d = (player.transform.position.ToVector2() - pos).sqrMagnitude;
            if (d < bestSqrScore)
            {
                bestPlayer = player;
                bestSqrScore = d;
            }
        }
        return bestPlayer;
    }
    public Soldier GetPlayerProbablyInOurZone()
    {
        var players = GetAllPlayers();
        Soldier bestPlayer = null;
        float bestY = -99999.0f;
        foreach (var player in players)
        {
            float y = player.transform.position.ToVector2().y;
            if (y > bestY)
            {
                bestPlayer = player;
                bestY = y;
            }
        }
        return bestPlayer;
    }

    public List<InterestPoint> GetAllPoints()
    {
        return GameManager.Instance.GetAllInterestPoints();
    }
    public InterestPoint GetNearestPoint(Vector2 pos)
    {
        var points = GetAllPoints();
        InterestPoint bestPoint = null;
        float bestSqrScore = 999999999.9f;
        foreach (var testPoint in points)
        {
            if (!testPoint.IsLocked())
            {
                float d = (testPoint.transform.position.ToVector2() - pos).sqrMagnitude;
                if (d < bestSqrScore)
                {
                    bestPoint = testPoint;
                    bestSqrScore = d;
                }
            }
        }
        return bestPoint;
    }

    // TODO : Ideally include cover
    public InterestPoint GetBestPointToShootAtPlayer(Soldier player, float maxDistanceSearchSqr)
    {
        var points = GetAllPoints();
        InterestPoint bestPoint = null;
        float bestSqrScore = 999999999.9f;
        foreach (var testPoint in points)
        {
            if (!testPoint.IsLocked() || (testPoint.IsLocked() && targetPoint == testPoint))
            {
                // Distance between ai pos and point pos
                float d = (testPoint.transform.position.ToVector2() - transform.position.ToVector2()).sqrMagnitude;
                if (d < maxDistanceSearchSqr && d < bestSqrScore)
                {
                    bool visible = false;
                    LayerMask mask = LayerMask.GetMask("Default");
                    Vector2 diff = (player.transform.position.ToVector2() - testPoint.transform.position.ToVector2()).normalized;
                    RaycastHit2D hit = Physics2D.Raycast(testPoint.transform.position.ToVector2(), diff, 25.0f, mask);
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject == player.gameObject /*|| (fakeAgent != null && fakeAgent.GetSoldier() == soldier)*/)
                        {
                            visible = true;
                        }
                    }
                    if (visible)
                    {
                        bestPoint = testPoint;
                        bestSqrScore = d;
                    }
                }
            }
        }
        return bestPoint;
    }


    public List<Objective> ScanObjectives()
    {
        List<Objective> results = new List<Objective>();

        /*
        // ReachInterestPoint
        {
            List<InterestPoint> interestPoints = GameManager.Instance.GetAllInterestPoints();

            InterestPoint bestPoint = null;
            float bestScore = -1.0f;

            foreach (var point in interestPoints)
            {
                if (!point.IsLocked() && (point.transform.position - transform.position).sqrMagnitude < maxDistanceLookBrain * maxDistanceLookBrain)
                {
                    float score = point.GetBaseScore();

                    if (score > bestScore)
                    {
                        bestPoint = point;
                        bestScore = score;
                    }
                }
            }

            if (bestPoint != null)
            {
                results.Add(new Objective(ObjectiveType.ReachInterestPoint, bestPoint.gameObject, gameObject, bestScore));
            }
        }

        // AttackPlayerZone
        {
            List<ZonePoint> playerZonePoints = GameManager.Instance.GetAllPlayerZonePoints();

            ZonePoint bestPoint = null;
            float bestScore = -1.0f;

            float distanceCheck = maxDistanceLookBrain;

            AIGeneral general = (AIGeneral)controller.GetGeneral();
            if (general != null && general.GetPlayerZone().GetEnemyInZoneCounter() > 0)
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

            AIGeneral general = (AIGeneral)controller.GetGeneral();
            if (general != null && general.GetEnemyZone().GetEnemyInZoneCounter() > 0)
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
            List<Soldier> playerSoldiers = 

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
        */

        return results;
    }
}
