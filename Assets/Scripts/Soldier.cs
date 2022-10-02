using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Soldier : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    
    [SerializeField] protected float fireCooldown = 3.0f;
    [SerializeField] protected float fireCooldownBonusReaction = 0.5f;
    [SerializeField] protected float cacCooldown = 1.0f;
    [SerializeField] protected float cacCooldownBonusReaction = 0.5f;
    [SerializeField] protected float fov = 60.0f;
    [SerializeField] protected float fireDistance = 40.0f;
    [SerializeField] protected float reactionDevAngle = 5.0f;
    [SerializeField] protected float fireDevAngle = 2.5f;
    [SerializeField] protected Transform bulletSpawn;
    [SerializeField] protected Transform cacHitCast;
    [SerializeField] protected float cacHitRadius = 0.25f;
    [SerializeField] protected int cacDamage = 70;

    private bool isMainSoldier = false;
    protected bool isPlayerSoldier = false;
    private bool justFired = false;
    private bool justCaced = false;

    protected Animator animator;
    protected HealthSystem healthSystem;
    protected LineRenderer lineRenderer;

    protected float timerAction = 900.0f;

    private float speedMultiplier = 1.0f;

    
    public virtual void SetMainSoldier(bool mainSoldier)
    {
        isMainSoldier = mainSoldier;

        if (lineRenderer != null)
        {
            lineRenderer.enabled = !mainSoldier;
        }
    }

    public bool IsMainSoldier()
    {
        return isMainSoldier;
    }

    public bool IsPlayerSoldier()
    {
        return isPlayerSoldier;
    }

    public float GetSpeed()
    {
        return speed * speedMultiplier;
    }

    public General GetGeneral()
    {
        return IsPlayerSoldier() ? GameManager.Instance.GetPlayerGeneral() : GameManager.Instance.GetEnemyGeneral();
    }

    public General GetOppositeGeneral()
    {
        return IsPlayerSoldier() ? GameManager.Instance.GetEnemyGeneral() : GameManager.Instance.GetPlayerGeneral();
    }

    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }

    public Vector2 GetBulletPos()
    {
        return bulletSpawn.transform.position.ToVector2();
    }

    public void Fire(bool reactionFire = false)
    {
        float lookAngle = GetLookAngle();
        if (reactionFire)
        {
            lookAngle += Random.Range(-reactionDevAngle, reactionDevAngle);
        }
        else
        {
            lookAngle += Random.Range(-fireDevAngle, fireDevAngle);
        }
        lookAngle *= Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(lookAngle), Mathf.Sin(lookAngle));

        animator?.SetTrigger("fire");
        justFired = true;

        BulletProjectile.Create(this, dir);

        timerAction = 0.0f;
    }

    public void Cac()
    {
        animator?.SetTrigger("cac");
        justCaced = true;

        timerAction = 0.0f;
    }

    public void CacHit()
    {
        Soldier soldier = null;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(cacHitCast.transform.position.ToVector2(), cacHitRadius);
        foreach (var collider in colliders)
        {
            soldier = collider.GetComponent<Soldier>();
            if (soldier == null)
            {
                soldier = collider.GetComponentInParent<Soldier>();
                if (soldier == null)
                {
                    soldier = collider.GetComponentInParent<FakeAgent>()?.GetSoldier();
                }
            }
            if (soldier != null)
                break;
        }
        if (soldier != null)
        {
            soldier.RecevedDamage(cacDamage, this);

            // TODO : Sound

            if (IsPlayerSoldier() && IsMainSoldier())
            {
                PlayerCameraController.Instance.Shake(5.0f, 1.0f);
            }
        }
    }

    public void RecevedDamage(int Damage, Soldier shooter)
    {
        healthSystem.Damage(Damage, shooter);
    }

    public float GetFireDistanceMax()
    {
        return fireDistance;
    }

    protected virtual bool CanFire()
    {
        return false;
    }

    protected virtual bool CanCac()
    {
        return false;
    }

    protected void AwakeSoldier(bool isPlayer)
    {
        animator = GetComponentInChildren<Animator>();
        healthSystem = GetComponent<HealthSystem>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
        isPlayerSoldier = isPlayer;
        BuildFov();
    }

    protected void UpdateSoldier()
    {
        timerAction += Time.deltaTime;

        if (justFired)
        {
            animator?.ResetTrigger("fire");
            justFired = false;
        }
        if (justCaced)
        {
            animator?.ResetTrigger("cac");
            justCaced = false;
        }
    }

    protected void UpdateReactions()
    {
        General enemyGeneral = GetOppositeGeneral();
        float cosHalfFov = Mathf.Cos(fov * 0.5f * Mathf.Deg2Rad);
        foreach (Soldier soldier in enemyGeneral.GetSoldiers())
        {
            Vector2 currentPos = bulletSpawn.position.ToVector2();
            Vector2 soldierPos = soldier.transform.position;
            Vector2 diff = (soldierPos - currentPos);
            if (diff.sqrMagnitude <= fireDistance * fireDistance) // Is in fire range
            {
                float distance = diff.magnitude;
                diff.Normalize();
                if (Vector2.Dot(GetLookDir(), diff) > cosHalfFov) // Is in fov
                {
                    // Do a raycast to check is there is obstacle between us
                    LayerMask mask = LayerMask.GetMask("Default");
                    RaycastHit2D hit = Physics2D.Raycast(currentPos, diff, distance, mask);
                    if (hit.collider.gameObject == soldier.gameObject)
                    {
                        // Good so look at it
                        SetLookDir(diff);

                        // Fire at it if we can
                        if (CanFire())
                        {
                            Fire(reactionFire: true);
                        }
                    }
                }
            }
        }
    }

    public void SetLookAngle(float angle)
    {
        transform.eulerAngles = new Vector3(0.0f, 0.0f, angle);
    }

    public void SetLookDir(Vector2 lookDir)
    {
        SetLookAngle(Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg);
    }

    public float GetLookAngle()
    {
        return transform.eulerAngles.z;
    }

    public Vector2 GetLookDir()
    {
        float angle = GetLookAngle();
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    private void BuildFov()
    {
        if (lineRenderer != null)
        {
            float minAngle = -fov * 0.5f;

            int arcPointCount = 20;
            Vector3[] lines = new Vector3[1 + arcPointCount];
            float angle = minAngle;
            for (int i = 0; i < arcPointCount; ++i)
            {
                lines[1 + i] = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * fireDistance, Mathf.Sin(angle * Mathf.Deg2Rad) * fireDistance, 0.0f);
                angle += fov / arcPointCount;
            }
            lines[0] = Vector3.zero;

            lineRenderer.loop = true;
            lineRenderer.positionCount = 1 + arcPointCount;
            lineRenderer.SetPositions(lines);
        }
    }
}
