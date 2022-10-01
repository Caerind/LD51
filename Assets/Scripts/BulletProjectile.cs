using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Vector2 dir;
    [SerializeField] private float speed= 30f;
    private float maxdistance;
    [SerializeField] private int degat = 20;

    public static BulletProjectile Create(Soldier soldier, Vector2 dir, float startDistance)
    {
        Transform pfBulletProjectile = Resources.Load<Transform>("pfBulletProjectile");
        Transform bulletTransform = Instantiate(pfBulletProjectile, soldier.transform.position + dir.ToVector3() * startDistance, Quaternion.identity);

        BulletProjectile bulletProjectile = bulletTransform.GetComponent<BulletProjectile>();
        bulletProjectile.dir = dir;
        bulletProjectile.maxdistance = soldier.GetFireDistanceMax();
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
            Destroy(gameObject);
            soldier.RecevedDamage(degat);
        }
    }
}
