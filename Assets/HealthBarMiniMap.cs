using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarMiniMap : MonoBehaviour
{

    private HealthSystem healthSystem;
    private Transform target;
    private Transform fullBarTransform;
    private Transform barTransform;
    // Start is called before the first frame update

    private void Awake()
    {
        fullBarTransform = transform.Find("FullBar");
        barTransform = fullBarTransform.Find("bar");
        fullBarTransform.gameObject.SetActive(true);
    }
    public void Init(Soldier soldier)
    {
        healthSystem = soldier.GetComponent<HealthSystem>();
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnDeath += HealthSystem_OnDeath;
    }
    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateBar();
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    private void UpdateBar()
    {
        barTransform.localScale = new Vector3(healthSystem.GetHealthAmountNormalized(), 1, 1);
    }
}
