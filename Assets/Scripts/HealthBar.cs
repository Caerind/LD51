using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour 
{
    [SerializeField] private HealthSystem healthSystem;

    private Transform bartransform;

    private void Awake()
    {
        bartransform = transform.Find("bar");
    }
    private void Start()
    {
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateBar();
    }

    private void UpdateBar()
    {
        bartransform.localScale = new Vector3(healthSystem.GetHealthAmountNormalized(), 1, 1);
    }
}
