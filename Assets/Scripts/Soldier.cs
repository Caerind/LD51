using UnityEngine;
using UnityEngine.Tilemaps;

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
    [SerializeField] protected float fireDevAngleMoving = 2.5f;
    [SerializeField] protected Transform bulletSpawn;
    [SerializeField] protected Transform cacHitCast;
    [SerializeField] protected float cacHitRadius = 0.25f;
    [SerializeField] protected int cacDamage = 70;
    [SerializeField] private float updateReactionTimer = 0.1f;
    [SerializeField] private GameObject bloodDeath;
    [SerializeField] private GameObject bloodImpact;
    [SerializeField] private AudioSource TirRecharge;
    [SerializeField] private AudioSource Baïonette;
    [SerializeField] private AudioSource Blessure;
    [SerializeField] public AudioSource Mort;
    [SerializeField] public bool isSelectable = true;

    private float reactionTimer;

    private bool isInOppositeZone = false;
    private bool isMainSoldier = false;
    protected bool isPlayerSoldier = false;
    private int justFired = 0;
    private int justCaced = 0;
    private const int skipFramesResetTrigger = 4;

    protected bool isMoving = false;
    protected bool isOnHole = false;

    protected Animator animator;
    protected HealthSystem healthSystem;
    protected LineRenderer lineRenderer;

    protected float timerAction = 900.0f;

    protected int animIDMvt;
    protected int animIDFire;
    protected int animIDCac;

    public float GetFireDistance()
    {
        return fireDistance;
    }
    
    public virtual void SetMainSoldier(bool mainSoldier)
    {
        reactionTimer = Random.Range(0.0f, updateReactionTimer); // Just so that all soldiers don't update on same frames

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
        return speed * 0.5f + speed * 0.5f * healthSystem.GetHealthAmountNormalized();
    }

    public General GetGeneral()
    {
        return IsPlayerSoldier() ? GameManager.Instance.GetPlayerGeneral() : GameManager.Instance.GetAIGeneral();
    }

    public General GetOppositeGeneral()
    {
        return IsPlayerSoldier() ? GameManager.Instance.GetAIGeneral() : GameManager.Instance.GetPlayerGeneral();
    }

    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }

    public Vector2 GetBulletPos()
    {
        return bulletSpawn.transform.position.ToVector2();
    }

    public void SetOnHole(bool hole)
    {
        isOnHole = hole;
    }

    public void Deces()
    {
        Mort.Play();
    }

    protected void OnSoldierDamaged(object sender)
    {
        if (sender != null)
        {
            Soldier senderSoldier = (Soldier)sender;
            if (senderSoldier != null && senderSoldier.IsPlayerSoldier() != IsPlayerSoldier())
            {
                SetLookAt(senderSoldier.transform.position);
            }
        }

        // Blood
        GameObject part = Instantiate(bloodImpact, transform.position, Quaternion.identity);
        Destroy(part, 2.0f);
    }

    protected void OnSoldierDied(GameObject deadBodyPrefab)
    {
        GetGeneral().RemoveRefToSoldier(this);

        // Spawn new entity
        Instantiate(deadBodyPrefab, transform.position, Quaternion.Euler(new Vector3(0.0f, 0.0f, GetLookAngle())));

        // Blood
        GameObject part = Instantiate(bloodDeath, transform.position, Quaternion.identity);
        Destroy(part, 2.0f);

        Destroy(gameObject);
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
            float isMovingBonus = isMoving ? fireDevAngleMoving : 0.0f;
            lookAngle += Random.Range(-fireDevAngle - isMovingBonus, fireDevAngle + isMovingBonus);
        }
        lookAngle *= Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(lookAngle), Mathf.Sin(lookAngle));

        animator?.SetTrigger(animIDFire);
        justFired = skipFramesResetTrigger;

        LayerMask mask = LayerMask.GetMask("Default");
        Vector2 pos = GetBulletPos();
        RaycastHit2D hit = Physics2D.Raycast(pos, dir, 1.0f, mask);
        TilemapCollider2D tilemap = hit.collider as TilemapCollider2D;
        bool invalidTilemap = false;
        if (tilemap != null && tilemap.tag == "Blocs")
            invalidTilemap = true;
        CompositeCollider2D tilemapComposite = hit.collider as CompositeCollider2D;
        bool invalidTilemapComposite = false;
        if (tilemapComposite != null && tilemapComposite.tag == "Blocs")
            invalidTilemapComposite = true;
        if (!invalidTilemap && !invalidTilemapComposite)
        {
            BulletProjectile.Create(this, dir);
        }

        timerAction = 0.0f;
        TirRecharge.Play();
    }

    public void Cac()
    {
        animator?.SetTrigger(animIDCac);
        justCaced = skipFramesResetTrigger;
        Baïonette.Play();
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
            if (soldier == this)
            {
                soldier = null;
            }
            if (soldier != null)
                break;
        }
        if (soldier != null)
        {
            soldier.RecevedDamage(cacDamage, this, fire:false);


            if (IsPlayerSoldier() && IsMainSoldier())
            {
                PlayerCameraController.Instance.Shake(5.0f, 1.0f);
            }
        }
        else
        {
            // TODO : Sound failed
        }
    }

    public void RecevedDamage(int Damage, Soldier shooter, bool fire)
    {
        Blessure.Play();
        
        if (isOnHole && fire)
        {
            healthSystem.Damage(Damage / 2, shooter);
        }
        else
        {
            healthSystem.Damage(Damage, shooter);
        }
    }

    public float GetFireDistanceMax()
    {
        return fireDistance;
    }

    public virtual bool CanFire()
    {
        return false;
    }

    public virtual bool CanCac()
    {
        return false;
    }

    protected void AwakeSoldier(bool isPlayer)
    {
        animator = GetComponentInChildren<Animator>();
        healthSystem = GetComponent<HealthSystem>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
        isPlayerSoldier = isPlayer;
        animIDMvt = Animator.StringToHash("mvt");
        animIDFire = Animator.StringToHash("fire");
        animIDCac = Animator.StringToHash("cac");
        BuildFov();
    }

    protected void UpdateSoldier()
    {
        timerAction += Time.deltaTime;

        justFired--;
        if (justFired == 0)
        {
            animator?.ResetTrigger(animIDFire);
        }
        justCaced--;
        if (justCaced == 0)
        {
            animator?.ResetTrigger(animIDCac);
        }
    }

    protected void UpdateReactions()
    {
        reactionTimer += Time.deltaTime;
        if (reactionTimer >= updateReactionTimer)
        {
            reactionTimer -= updateReactionTimer;

            General enemyGeneral = GetOppositeGeneral();
            float cosHalfFov = Mathf.Cos(fov * 0.5f * Mathf.Deg2Rad);
            foreach (Soldier soldier in enemyGeneral.GetSoldiers())
            {
                if (soldier == null)
                    continue;
                Vector2 currentPos = bulletSpawn.position.ToVector2();
                Vector2 soldierPos = soldier.transform.position;
                Vector2 diff = (soldierPos - currentPos);
                float d = diff.sqrMagnitude;
                if (d < 2.0f * 2.0f)
                {
                    diff.Normalize();
                    SetLookDir(diff);

                    // Cac at it if we can
                    if (CanCac())
                    {
                        Cac();
                    }
                }
                else if (d <= fireDistance * fireDistance) // Is in fire range
                {
                    float distance = diff.magnitude;
                    diff.Normalize();
                    if (Vector2.Dot(GetLookDir(), diff) > cosHalfFov) // Is in fov
                    {
                        // Do a raycast to check is there is obstacle between us
                        LayerMask mask = LayerMask.GetMask("Default");
                        RaycastHit2D hit = Physics2D.Raycast(currentPos, diff, distance, mask);
                        if (hit.collider != null)
                        {
                            FakeAgent fakeAgent = hit.collider.gameObject.GetComponentInParent<FakeAgent>();
                            if (hit.collider.gameObject == soldier.gameObject || (fakeAgent != null && fakeAgent.GetSoldier() == soldier))
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

    public void SetLookAt(Vector2 pos)
    {
        SetLookDir((pos - transform.position.ToVector2()).normalized);
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

    public bool IsInOppositeZone()
    {
        return isInOppositeZone;
    }

    public void SetIsInOppositeZone(bool isInOppositeZone)
    {
        this.isInOppositeZone = isInOppositeZone;
    }
}
