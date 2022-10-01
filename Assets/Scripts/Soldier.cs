using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [SerializeField] protected float speed = 5.0f;
    
    [SerializeField] protected float fireCooldown = 3.0f;
    [SerializeField] protected float fireCooldownBonusReaction = 0.5f;
    [SerializeField] protected float fov = 60.0f;
    [SerializeField] protected float fireDistance = 40.0f;

    protected Vector2 lookDir;

    private bool isMainSoldier = false;
    protected bool isPlayerSoldier = false;

    protected HealthSystem healthSystem;
    protected LineRenderer lineRenderer;
    private bool fovUpdated = false;

    protected float timerFire = 900.0f;
    
    public virtual void SetMainSoldier(bool mainSoldier)
    {
        if (isMainSoldier && !mainSoldier)
        {
            fovUpdated = false;
        }

        isMainSoldier = mainSoldier;

        lineRenderer.enabled = !mainSoldier;
    }

    public bool IsMainSoldier()
    {
        return isMainSoldier;
    }

    public bool IsPlayerSoldier()
    {
        return isPlayerSoldier;
    }

    public General GetGeneral()
    {
        return IsPlayerSoldier() ? GameManager.Instance.GetPlayerGeneral() : GameManager.Instance.GetEnemyGeneral();
    }

    public General GetOppositeGeneral()
    {
        return IsPlayerSoldier() ? GameManager.Instance.GetEnemyGeneral() : GameManager.Instance.GetPlayerGeneral();
    }

    public void Fire(bool reactionFire = false)
    {
        Vector2 dir = lookDir;
        if (reactionFire)
        {
            Debug.Log(gameObject.name + ": ReactionFire");
        }
        else
        {
            Debug.Log(gameObject.name + ": Fire");
        }
        Shoot();
        timerFire = 0.0f;
        healthSystem.Damage(50);
    }

    public void RecevedDamage(int Damage)
    {
        healthSystem.Damage(Damage);
    }
    private void Shoot()
    {
        BulletProjectile.Create(this);
    }
    public float GetFireDistanceMax()
    {
        return fireDistance;
    }



    protected virtual bool CanFire()
    {
        return false;
    }

    protected void AwakeSoldier()
    {
        healthSystem = GetComponent<HealthSystem>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    protected void StartSoldier(bool isPlayer, Vector2 look)
    {
        isPlayerSoldier = isPlayer;
        lookDir = look;
    }

    protected void UpdateSoldier()
    {
        timerFire += Time.deltaTime;
    }

    protected void UpdateReactions()
    {
        if (!fovUpdated)
        {
            UpdateFOV();
        }

        General enemyGeneral = GetOppositeGeneral();
        float cosHalfFov = Mathf.Cos(fov * 0.5f * Mathf.Deg2Rad);
        foreach (Soldier soldier in enemyGeneral.GetSoldiers())
        {
            Vector2 currentPos = transform.position;
            Vector2 soldierPos = soldier.transform.position;
            Vector2 diff = (soldierPos - currentPos);
            if (diff.sqrMagnitude <= fireDistance * fireDistance) // Is in fire range
            {
                diff.Normalize();
                if (Vector2.Dot(lookDir, diff) > cosHalfFov) // Is in fov
                {
                    lookDir = diff;
                    if (CanFire())
                    {
                        Fire(reactionFire: true);
                    }
                }
            }
        }
    }

    public float GetLookAngle()
    {
        return Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
    }

    public Vector2 GetLookDir()
    {
        return lookDir;
    }

    private void UpdateFOV()
    {
        if (lineRenderer != null)
        {
            float lookAngle = GetLookAngle();
            float minAngle = lookAngle - (fov * 0.5f);

            int arcPointCount = 10;
            Vector3[] lines = new Vector3[1 + arcPointCount];
            float angle = minAngle;
            for (int i = 0; i < arcPointCount; ++i)
            {
                lines[1 + i] = transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * fireDistance, Mathf.Sin(angle * Mathf.Deg2Rad) * fireDistance, 0.0f);
                angle += fov / arcPointCount;
            }
            lines[0] = transform.position;

            lineRenderer.useWorldSpace = true;
            lineRenderer.loop = true;
            lineRenderer.positionCount = 1 + arcPointCount;
            lineRenderer.SetPositions(lines);
        }

        fovUpdated = true;
    }

}
