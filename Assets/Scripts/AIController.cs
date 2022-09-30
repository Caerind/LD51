using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform target;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = PlayerManager.Instance?.player?.transform;
    }

    private void Update()
    {
        if (target != null && NavMeshGenerator.Instance.IsNavMeshReady())
        {
            agent.destination = target.position;
        }
    }
}
