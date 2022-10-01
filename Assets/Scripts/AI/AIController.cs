using UnityEngine;
using UnityEngine.AI;

public class AIController : Soldier
{
    [SerializeField] private float fireCooldownBonusAI = 0.5f;

    public Vector3 targetPositionDebug;


    private NavMeshAgent agent;

    private float timerFire = 900.0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    private void Update()
    {
        if (IsMainSoldier())
        {
            agent.destination = targetPositionDebug;
        }
    }

    private bool CanFire()
    {
        return timerFire >= (fireCooldown + fireCooldownBonusAI);
    }
}
