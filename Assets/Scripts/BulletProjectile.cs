using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 50.0f;
    [SerializeField] private int degat = 40;
    [SerializeField] private float distanceMinForCover = 3.0f;

    private Soldier shooter;
    private Vector2 dir;
    private float maxDistance;
    private float distanceDone;

    public static BulletProjectile Create(Soldier soldier, Vector2 dir)
    {
        Transform pfBulletProjectile = Resources.Load<Transform>("pfBulletProjectile");
        Transform bulletTransform = Instantiate(pfBulletProjectile, soldier.GetBulletPos(), Quaternion.identity);

        BulletProjectile bulletProjectile = bulletTransform.GetComponent<BulletProjectile>();
        bulletProjectile.dir = dir;
        bulletProjectile.maxDistance = soldier.GetFireDistanceMax();
        bulletProjectile.distanceDone = 0.0f;
        bulletProjectile.shooter = soldier;
        return bulletProjectile;
    }

    private void Update()
    {
        transform.position += dir.ToVector3() * speed * Time.deltaTime;
        float mvt = Time.deltaTime * speed;
        maxDistance -= mvt;
        distanceDone += mvt;
        if (maxDistance <= 0)
        {
            Debug.Log("Decay");
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
            Debug.Log("Soldier " + collision.gameObject.name);
            Destroy(gameObject);
            soldier.RecevedDamage(degat, shooter, fire:true);
        }
        else if (collision.GetComponent<BulletDetector>() == null)
        {
            Cover cover = collision.GetComponent<Cover>();
            if (cover != null)
            {
                if (distanceDone > distanceMinForCover)
                {
                    float random = Random.Range(0f, 1f);
                    if (random <= cover.coverPercent)
                    {
                        Debug.Log("Cover " + collision.gameObject.name);
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
                Debug.Log("Any " + collision.gameObject.name);
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
