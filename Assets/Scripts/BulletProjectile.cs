using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 50.0f;
    [SerializeField] private int damage = 40;
    [SerializeField] private float distanceMinForCover = 3.0f;

    private Soldier shooter;
    private Vector2 dir;
    private float maxDistance;
    private float distanceTraveled;

    public static void Create(Soldier soldier, Vector2 dir)
    {
        GameObject bullet = Instantiate(PrefabManager.Instance.GetBulletProjectilePrefab(), soldier.GetBulletPos(), Quaternion.identity);

        BulletProjectile bulletProjectile = bullet.GetComponent<BulletProjectile>();
        bulletProjectile.shooter = soldier;
        bulletProjectile.dir = dir;
        bulletProjectile.maxDistance = soldier.GetFireDistanceMax();
        bulletProjectile.distanceTraveled = 0.0f;
    }

    private void Update()
    {
        transform.position += dir.ToVector3() * speed * Time.deltaTime;
        float mvt = Time.deltaTime * speed;
        maxDistance -= mvt;
        distanceTraveled += mvt;
        if (maxDistance <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Soldier soldier = collision.GetComponent<Soldier>();
        if (soldier == null)
        {
            soldier = collision.GetComponentInParent<FakeAgent>()?.GetSoldier();
        }
        if (soldier != null)
        {
            Destroy(gameObject);
            soldier.RecevedDamage(damage, shooter, fire:true);
        }
        else if (collision.GetComponent<BulletDetector>() == null)
        {
            Cover cover = collision.GetComponent<Cover>();
            if (cover != null)
            {
                if (distanceTraveled > distanceMinForCover)
                {
                    float random = Random.Range(0f, 1f);
                    if (random > cover.GetProtectionChance())
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    // Don't destroy if close from cover
                }                
            }
            else
            {
                Destroy(gameObject);
            }
        }
        /*
        else // If we have a BulletDetector, don't Destroy
        {
        }
        */
    }

    public Soldier GetShooter()
    {
        return shooter;
    }
}
