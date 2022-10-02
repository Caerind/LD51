using UnityEngine;

public class CacHitReceiver : MonoBehaviour
{
    private Soldier soldier;

    private void Awake()
    {
        soldier = GetComponentInParent<Soldier>();
    }

    public void CacHit()
    {
        soldier?.CacHit();
    }
}
