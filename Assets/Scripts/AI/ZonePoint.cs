using UnityEngine;

public class ZonePoint : MonoBehaviour
{
    public bool isPlayerZonePoint = false;

    private void Awake()
    {
        GameManager.Instance.RegisterZonePoint(this);
    }

    public Vector2 GetLookDir()
    {
        return new Vector2(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + 2.0f * GetLookDir().ToVector3());
    }
}
