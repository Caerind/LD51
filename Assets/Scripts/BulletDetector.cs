using Unity.VisualScripting;
using UnityEngine;

public class BulletDetector : MonoBehaviour
{
    private Soldier soldier = null;
    [SerializeField] private float distanceShooterMax = 30.0f;

    private void Awake()
    {
        soldier = GetComponentInParent<Soldier>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BulletProjectile bullet = collision.GetComponent<BulletProjectile>();
        if (bullet != null && bullet.GetShooter() != null && bullet.GetShooter().IsPlayerSoldier() != soldier.IsPlayerSoldier())
        {
            Vector2 diff = (bullet.transform.position - soldier.transform.position).ToVector2();
            diff.Normalize();

            LayerMask mask = LayerMask.GetMask("Default");
            RaycastHit2D hit = Physics2D.Raycast(soldier.transform.position.ToVector2() + diff * 1.0f, diff, distanceShooterMax, mask);
            if (hit.collider.gameObject == soldier.gameObject)
            {
                soldier.SetLookDir(diff);
            }
        }
    }
}
