using UnityEngine;

public class Cover : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float protectionChance = 0.5f;

    public float GetProtectionChance()
    {
        return protectionChance;
    }
}
