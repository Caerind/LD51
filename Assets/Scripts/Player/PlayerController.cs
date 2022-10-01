using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : Soldier
{
    [SerializeField] private Light2D fovLight;
    [SerializeField] private float IntensiteTirShake = 3f;
    [SerializeField] private float TimerTirShake = 1f;
    [SerializeField] private float IntensiteDegatShake = 6f;
    [SerializeField] private float TimerDegatShake = 3f;
    
    private void Awake()
    {
        AwakeSoldier();
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnDeath += HealthSystem_OnDied;
        fovLight.pointLightInnerRadius = 0.0f;
        fovLight.pointLightOuterRadius = fireDistance;
    }

    private void Start()
    {
        StartSoldier(isPlayer: true);
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        if(IsMainSoldier())
        {
            PlayerCameraController.Instance.Shake(IntensiteDegatShake, TimerDegatShake);
        }
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        GetGeneral().RemoveRefToSoldier(this);

        // Spawn new entity
        Transform pfDeadBody = Resources.Load<Transform>("pfDeadBody");
        Instantiate(pfDeadBody, transform.position, Quaternion.identity);

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
    }

    public override void SetMainSoldier(bool mainSoldier)
    {
        base.SetMainSoldier(mainSoldier);

        if (mainSoldier)
        {
            PlayerCameraController.Instance.SetFollow(transform);
        }
    }

    private void UpdateMainPlayer()
    {
        Inputs inputs = Inputs.Instance;

        // Mvt
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
            Fire();
            PlayerCameraController.Instance.Shake(IntensiteTirShake, TimerTirShake);
        }
    }

    protected override bool CanFire()
    {
        return timerFire >= (fireCooldown + (IsMainSoldier() ? 0.0f : fireCooldownBonusReaction));
    }
}
