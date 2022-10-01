using System;
using UnityEngine;

public class HealthBar : MonoBehaviour 
{
    [SerializeField] private Vector2 offset;

    private HealthSystem healthSystem;
    private Transform target;
    private Transform fullBarTransform;
    private Transform barTransform;

    private void Awake()
    {
        fullBarTransform = transform.Find("FullBar");
        barTransform = fullBarTransform.Find("bar");
    }

    public void Init(Soldier soldier)
    {
        healthSystem = soldier.GetComponent<HealthSystem>();
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        target = soldier.transform;
        fullBarTransform.gameObject.SetActive(false);
        healthSystem.OnDeath +=HealthSystem_OnDeath;
    }

    private void LateUpdate()
    {
        transform.position = target.position + offset.ToVector3();
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateBar();
    }
    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }

    private void UpdateBar()
    {
        fullBarTransform.gameObject.SetActive(true);
        barTransform.localScale = new Vector3(healthSystem.GetHealthAmountNormalized(), 1, 1);
    }
}
