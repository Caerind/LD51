using UnityEngine;

public class PlayerController : Soldier
{
    private float timerFire = 900.0f;

    private void Update()
    {
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
            float angle = -90.0f + Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0.0f, 0.0f, angle);
            lookVector = look;
        }

        // Fire
        if (inputs.fire && CanFire())
        {
            Fire();
            timerFire = 0.0f;
        }
    }

    private void UpdateReactions()
    {
        // TODO
    }

    private bool CanFire()
    {
        return timerFire >= fireCooldown;
    }
}
