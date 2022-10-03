using UnityEngine;

public class InterestPoint : MonoBehaviour
{
    [SerializeField] private float baseScore = 10.0f;

    [SerializeField] private bool cover = true;
    [SerializeField] private bool defendBase = false;

    private bool locked = false;

    private void Awake()
    {
        GameManager.Instance.RegisterInterestPoint(this);
    }

    public float GetBaseScore()
    {
        return baseScore;
    }

    public bool IsLocked()
    {
        return locked;
    }

    public void SetIsLocked(bool locked)
    {
        this.locked = locked;
    }

    public Vector2 GetLookDir()
    {
        return new Vector2(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
        Gizmos.DrawLine(transform.position, transform.position + GetLookDir().ToVector3());
    }
}
