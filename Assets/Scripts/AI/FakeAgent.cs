using UnityEngine;
using UnityEngine.AI;

public class FakeAgent : MonoBehaviour
{
    [SerializeField] private float acceleration = 20.0f;

    private NavMeshAgent agent;
    private Soldier soldier;

    public void Init(Soldier soldier)
    {
        transform.position = soldier.transform.position;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.acceleration = acceleration;
        agent.destination = transform.position;
        agent.stoppingDistance = 0.1f;
        agent.ResetPath();

        this.soldier = soldier;

        AIController aiController = (AIController)soldier;
        if (aiController != null)
        {
            aiController.SetFakeAgent(this);
        }
    }

    public void SetStopped(bool stopped)
    {
        agent.isStopped = stopped;
    }
    
    public bool IsStopped()
    {
        return agent.isStopped;
    }

    public Vector2 GetDestination()
    {
        return agent.destination.ToVector2();
    }

    public void SetDestination(Vector2 destination)
    {
        agent.ResetPath();
        agent.destination = destination.ToVector3();
    }

    public void SetSpeed(float speed)
    {
        agent.speed = speed;
    }

    public Soldier GetSoldier()
    {
        return soldier;
    }
}
