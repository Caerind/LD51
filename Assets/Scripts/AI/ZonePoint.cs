using UnityEngine;

public class ZonePoint : MonoBehaviour
{
    public bool isPlayerZonePoint = false;

    private void Awake()
    {
        GameManager.Instance.RegisterZonePoint(this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
