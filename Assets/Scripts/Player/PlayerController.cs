using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Windows;

public class PlayerController : Soldier
{
    [SerializeField] private Light2D fovLight;
    [SerializeField] private float IntensiteTirShake = 3f;
    [SerializeField] private float TimerTirShake = 1f;
    [SerializeField] private float IntensiteDegatShake = 6f;
    [SerializeField] private float TimerDegatShake = 3f;
    [SerializeField] private GameObject CircleBlue;
    [SerializeField] private GameObject CircleWhite;

    private void Awake()
    {
        AwakeSoldier(isPlayer: true);
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnDeath += HealthSystem_OnDied;
        fovLight.pointLightInnerRadius = 0.0f;
        fovLight.pointLightOuterRadius = fireDistance;
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        if (IsMainSoldier())
        {
            PlayerCameraController.Instance.Shake(IntensiteDegatShake, TimerDegatShake);
        }
        else if (sender != null)
        {
            Soldier senderSoldier = (Soldier)sender;
            if (senderSoldier != null && !senderSoldier.IsPlayerSoldier())
            {
                SetLookDir((senderSoldier.transform.position - transform.position).ToVector2().normalized);
            }
        }
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        GetGeneral().RemoveRefToSoldier(this);

        // Spawn new entity
        Transform pfDeadBody = Resources.Load<Transform>("pfDeadBodyPlayer");
        Instantiate(pfDeadBody, transform.position, Quaternion.identity).eulerAngles = new Vector3(0.0f, 0.0f, GetLookAngle());

        // Center cam on new entity
        if (IsMainSoldier())
        {
            PlayerCameraController.Instance.SetFollow(transform);
        }

        Destroy(gameObject);
    }

    private void Update()
    {
        UpdateSoldier();

        if (IsMainSoldier())
        {
            UpdateMainPlayer();
        }
        else
        {
            UpdateReactions();
        }

        UpdateMinimap();

    }

    public override void SetMainSoldier(bool mainSoldier)
    {
        base.SetMainSoldier(mainSoldier);

        if (mainSoldier)
        {
            PlayerCameraController.Instance.SetFollow(transform);
        }
        else
        {
            animator?.SetFloat(animIDMvt, 0.0f);
            isMoving = false;
        }
    }

    private void UpdateMainPlayer()
    {
        Inputs inputs = Inputs.Instance;

        // Mvt
        float mvt = inputs.move.magnitude;
        animator?.SetFloat(animIDMvt, mvt);
        isMoving = mvt > 0.05f;
        if (inputs.move != Vector2.zero)
        {
            float s = GetSpeed() * Time.deltaTime;
            transform.position += new Vector3(s * inputs.move.x, s * inputs.move.y, 0.0f);
        }

        // Look
        Vector2 look = Vector2.zero;
        if (inputs.look != Vector2.zero) // Gamepad
        {
            look = inputs.look;
        }
        else if (inputs.point != Vector2.zero) // Mouse
        {
            look = (Camera.main.ScreenToWorldPoint(inputs.point) - transform.position).ToVector2();
        }
        if (look != Vector2.zero)
        {
            SetLookDir(look.normalized);
        }

        // Fire
        if (inputs.fire && CanFire())
        {
            inputs.fire = false;
            Fire();
            PlayerCameraController.Instance.Shake(IntensiteTirShake, TimerTirShake);
        }

        // Cac
        if (inputs.cac && CanCac())
        {
            inputs.cac = false;
            Cac();
        }
    }

    private void UpdateMinimap()
    {
        General general = GetGeneral();
        if (this == general.GetNextSelectedSoldier())
        {
            CircleWhite.SetActive(true);
            CircleBlue.SetActive(true);
        }
        else if (this == general.GetSelectedSoldier())
        {
            CircleWhite.SetActive(true);
            CircleBlue.SetActive(false);
        }
        else
        {
            CircleWhite.SetActive(false);
            CircleBlue.SetActive(true);
        }
    }

    protected override bool CanFire()
    {
        return timerAction >= (fireCooldown + (IsMainSoldier() ? 0.0f : fireCooldownBonusReaction));
    }

    protected override bool CanCac()
    {
        return timerAction >= (cacCooldown + (IsMainSoldier() ? 0.0f : cacCooldownBonusReaction));
    }
}
