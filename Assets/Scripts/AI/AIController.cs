using UnityEngine;
using UnityEngine.AI;

public class AIController : Soldier
{
    [SerializeField] private float fireCooldownBonusAI = 0.5f;

    public Vector3 targetPositionDebug;


    private NavMeshAgent agent;

    private void Awake()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        isPlayerSoldier = false;
        lookDir = new Vector2(0.0f, -1.0f);

        agent.speed = speed;
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
