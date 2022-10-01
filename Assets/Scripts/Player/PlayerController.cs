using UnityEngine;
using UnityEngine.AI;

public class PlayerController : Soldier
{
    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDeath += HealthSystem_OnDied;
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    private void Start()
    {
        isPlayerSoldier = true;
        lookDir = new Vector2(0.0f, 1.0f);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
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
            float s = speed * Time.deltaTime;
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
            lookDir = new Vector2(look.x, look.y);
            lookDir.Normalize();
            transform.eulerAngles = new Vector3(0.0f, 0.0f, GetLookAngle());
        }

        // Fire
        if (inputs.fire && CanFire())
        {
            Fire();
        }
    }

    protected override bool CanFire()
    {
        return timerFire >= (fireCooldown + (IsMainSoldier() ? 0.0f : fireCooldownBonusReaction));
    }
}
