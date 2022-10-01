using System.Collections.Generic;
using UnityEngine;

public class AIController : Soldier
{
    [SerializeField] private float fireCooldownBonusAI = 0.5f;

    private InterestPoint point = null;

    private FakeAgent agent;

    private void Awake()
    {
        AwakeSoldier();
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnDeath += HealthSystem_OnDied;
    }

    private void Start()
    {
        StartSoldier(isPlayer: false);
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
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
        }
        else
        {
            agent.SetSpeed(GetSpeed());
            agent.SetDestination(point.transform.position);
        }
    }

    private void LateUpdate()
    {
        Vector2 prevPos = transform.position.ToVector2();
        transform.position = agent.transform.position;

        Vector2 mvt = transform.position.ToVector2() - prevPos;
        if (mvt != Vector2.zero)
        {
            SetLookDir(mvt.normalized);
        }
        else if (point != null)
        {
            float sqrPointDistance = (transform.position - point.transform.position).sqrMagnitude;
            if (sqrPointDistance < 2.0f * 2.0f)
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
}
