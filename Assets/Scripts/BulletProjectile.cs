using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 60f;
    [SerializeField] private int degat = 20;

    private Soldier shooter;
    private Vector2 dir;
    private float maxdistance;

    public static BulletProjectile Create(Soldier soldier, Vector2 dir)
    {
        Transform pfBulletProjectile = Resources.Load<Transform>("pfBulletProjectile");
        Transform bulletTransform = Instantiate(pfBulletProjectile, soldier.GetBulletPos(), Quaternion.identity);

        BulletProjectile bulletProjectile = bulletTransform.GetComponent<BulletProjectile>();
        bulletProjectile.dir = dir;
        bulletProjectile.maxdistance = soldier.GetFireDistanceMax();
        bulletProjectile.shooter = soldier;
        return bulletProjectile;
    }

    private void Update()
    {
        transform.position += dir.ToVector3() * speed * Time.deltaTime;
        maxdistance = maxdistance - (Time.deltaTime * speed);
        if(maxdistance <= 0) 
            Destroy(gameObject);
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
            soldier.RecevedDamage(degat, shooter);
        }

        Debug.Log(collision.gameObject);
        Destroy(gameObject);
    }
}
