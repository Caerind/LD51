using UnityEngine;
using UnityEngine.AI;

public class FakeAgent : MonoBehaviour
{
    private NavMeshAgent agent;
    private Soldier soldier;

    public void Init(Soldier soldier)
    {
        agent = GetComponent<NavMeshAgent>();
        this.soldier = soldier;

        transform.position = soldier.transform.position;
        
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
