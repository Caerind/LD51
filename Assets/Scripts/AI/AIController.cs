using UnityEngine;
using UnityEngine.AI;

public class AIController : Soldier
{
    [SerializeField] private float fireCooldownBonusAI = 0.5f;

    public Vector3 targetPositionDebug;

    private NavMeshAgent agent;

    private void Awake()
    {
        AwakeSoldier();
        healthSystem.OnDeath += HealthSystem_OnDied;

        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartSoldier(isPlayer: false, Vector2.down);

        agent.speed = speed;
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
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

    protected override bool CanFire()
    {
        return timerFire >= (fireCooldown + fireCooldownBonusAI + (IsMainSoldier() ? 0.0f : fireCooldownBonusReaction));
    }

    private void UpdateMainAI()
    {
        agent.destination = targetPositionDebug;
    }
}
