using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDamaged;
    public event EventHandler OnDeath;

    [SerializeField] private int healthAmountMax = 100;
    private int healthAmount;

    private void Awake()
    {
        healthAmount = healthAmountMax;
    }

    public void Damage(int Amount, Soldier shooter) 
    {
        healthAmount -= Amount;
        healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);

        OnDamaged?.Invoke(shooter, EventArgs.Empty);

        if (IsDead())
        {
            OnDeath?.Invoke(shooter, EventArgs.Empty);
        }
    }

    public bool IsDead()
    {
        return healthAmount <= 0;
    }

    public int GetHealthAmount()
    {
        return healthAmount;
    }

    public float GetHealthAmountNormalized()
    {
        return (float)healthAmount / healthAmountMax;
    }
}
