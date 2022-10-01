using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDamaged;
    public event EventHandler OnDeath;

    [SerializeField] private int healthAmountMax;
    private int healthAmount;

    private void Awake()
    {
        healthAmount = healthAmountMax;
    }

    public void Damage(int Amount) 
    {
        healthAmount -= Amount;
        healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (IsDead())
        {
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsDead()
    {
        return healthAmount == 0;
    }

    public int GetHealthAMount()
    {
        return healthAmount;
    }
    public float GetHealthAmountNormalized()
    {
        return (float)healthAmount / healthAmountMax;
    }
}
