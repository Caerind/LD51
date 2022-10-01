using UnityEngine;

public class Soldier : MonoBehaviour
{
    [SerializeField] protected float speed = 5.0f;
    [SerializeField] protected float fireCooldown = 3.0f
    protected HealthSystem healthSystem;

    protected Vector2 lookVector;

    private bool isMainSoldier = false;
    
    public virtual void SetMainSoldier(bool mainSoldier)
    {
        isMainSoldier = mainSoldier;
    }

    public bool IsMainSoldier()
    {
        return isMainSoldier;
    }

    public void Fire()
    {
        Debug.Log(gameObject.name + ": Pan");
        healthSystem.Damage(10);


    }

}
