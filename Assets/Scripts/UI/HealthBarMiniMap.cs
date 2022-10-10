using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarMiniMap : MonoBehaviour
{
    [SerializeField] private Image soldierImage;
    [SerializeField] private float soldierDiedAlpha = 0.2f;

    private HealthSystem healthSystem;
    private Transform fullBarTransform;
    private Transform barTransform;

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
        soldierImage.color = new Color(1.0f, 1.0f, 1.0f, soldierDiedAlpha);
        Destroy(gameObject);
    }

    private void UpdateBar()
    {
        barTransform.localScale = new Vector3(healthSystem.GetHealthAmountNormalized(), 1.0f, 1.0f);
    }
}
