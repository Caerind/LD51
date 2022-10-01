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

    private bool isMainSoldier = false;
    protected bool isPlayerSoldier = false;

    protected HealthSystem healthSystem;
    protected LineRenderer lineRenderer;

    protected float timerFire = 900.0f;
    
    public virtual void SetMainSoldier(bool mainSoldier)
    {
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
        Vector2 dir = GetLookDir();
        if (reactionFire)
        {
            // TODO : Add random dev
        }

        Shoot(dir);
        timerFire = 0.0f;
    }

    public void RecevedDamage(int Damage)
    {
        healthSystem.Damage(Damage);
    }

    private void Shoot(Vector2 dir)
    {
        BulletProjectile.Create(this, dir, 2.0f);
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

    protected void StartSoldier(bool isPlayer)
    {
        isPlayerSoldier = isPlayer;
        BuildFov();
    }

    protected void UpdateSoldier()
    {
        timerFire += Time.deltaTime;
    }

    protected void UpdateReactions()
    {
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
                if (Vector2.Dot(GetLookDir(), diff) > cosHalfFov) // Is in fov
                {
                    /*
                    transform.eulerAngles = new Vector3(0.0f, 0.0f, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
                    if (isPlayerSoldier)
                    {
                        transform.eulerAngles = new Vector3(0.0f, 0.0f, GetLookAngle());
                    }
                    else
                    {
                        Vector3 eulerAngles = transform.eulerAngles;
                        transform.eulerAngles = new Vector3(GetLookAngle(), eulerAngles.y, eulerAngles.z);
                    }
                    */

                    if (CanFire())
                    {
                        Fire(reactionFire: true);
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
