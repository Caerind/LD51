using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : Soldier
{
    [SerializeField] private Light2D fovLight;
    [SerializeField] private float IntensiteTirShake = 3f;
    [SerializeField] private float TimerTirShake = 1f;
    [SerializeField] private float IntensiteDegatShake = 6f;
    [SerializeField] private float TimerDegatShake = 3f;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private GameObject bloodDeath;
    [SerializeField] private GameObject bloodImpact;
    [SerializeField] private AudioSource Deplacement;

    private LineRenderer gamepadLine;

    private void Awake()
    {
        AwakeSoldier(isPlayer: true);
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnDeath += HealthSystem_OnDied;

        fovLight.pointLightInnerRadius = 0.0f;
        fovLight.pointLightOuterRadius = fireDistance;

        gamepadLine = cameraTarget.GetComponent<LineRenderer>();
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
                SetLookAt(senderSoldier.transform.position);
            }

            // Blood
            GameObject part = Instantiate(bloodImpact, transform.position, Quaternion.identity);
            Destroy(part, 2.0f);
        }
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        GetGeneral().RemoveRefToSoldier(this);

        // Spawn new entity
        GameObject pfDeadBody = PrefabManager.Instance.GetDeadBodyPlayerPrefab();
        Instantiate(pfDeadBody, transform.position, Quaternion.Euler(new Vector3(0.0f, 0.0f, GetLookAngle())));

        // Center cam on new entity
        if (IsMainSoldier())
        {
            PlayerCameraController.Instance.SetFollow(cameraTarget);
        }
        
        if (gamepadLine != null)
            gamepadLine.enabled = false;

        // Blood
        GameObject part = Instantiate(bloodDeath, transform.position, Quaternion.identity);
        Destroy(part, 2.0f);

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
            PlayerCameraController.Instance.SetFollow(cameraTarget);
            if (gamepadLine != null)
                gamepadLine.enabled = true;
        }
        else
        {
            Deplacement.Stop();
            animator?.SetFloat(animIDMvt, 0.0f);
            isMoving = false;
            if (gamepadLine != null)
                gamepadLine.enabled = false;
        }
    }

    private void UpdateMainPlayer()
    {
        Inputs inputs = Inputs.Instance;

        // Mvt
        float mvt = inputs.move.magnitude;
        animator?.SetFloat(animIDMvt, mvt);
        bool wasmooving = isMoving;
        isMoving = mvt > 0.025f;
        if(wasmooving != isMoving)
        {
            if (isMoving)
            {
                Deplacement.Play();
            }
            else
            {
                Deplacement.Stop();
            }
        }
        if (inputs.move != Vector2.zero)
        {
            float s = GetSpeed() * Time.deltaTime;
            transform.position += new Vector3(s * inputs.move.x, s * inputs.move.y, 0.0f);
        }
        // Look
        Vector2 look = Vector2.zero;
        if (inputs.IsUsingGamepad())
        {
            if (inputs.look != Vector2.zero) // Gamepad
            {
                look = inputs.look;
                cameraTarget.transform.position = transform.position + look.ToVector3() * fireDistance * 0.2f;
            }
            if (gamepadLine != null)
            {
                gamepadLine.enabled = true;
                Vector3[] lines = new Vector3[2];
                lines[0] = transform.position;
                lines[1] = transform.position + look.ToVector3() * fireDistance * 0.4f;
                gamepadLine.useWorldSpace = true;
                gamepadLine.positionCount = 2;
                gamepadLine.SetPositions(lines);
            }
        }
        else
        {
            if (inputs.point != Vector2.zero) // Mouse
            {
                Vector3 diff = (Camera.main.ScreenToWorldPoint(inputs.point) - transform.position);
                look = diff.ToVector2();
                cameraTarget.transform.position = transform.position + diff * 0.5f;
            }

            if (gamepadLine != null)
                gamepadLine.enabled = false;
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

    public override bool CanFire()
    {
        return timerAction >= (fireCooldown + (IsMainSoldier() ? 0.0f : fireCooldownBonusReaction));
    }

    public override bool CanCac()
    {
        return timerAction >= (cacCooldown + (IsMainSoldier() ? 0.0f : cacCooldownBonusReaction));
    }
}
