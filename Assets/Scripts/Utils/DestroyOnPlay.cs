using UnityEngine;

public class DestroyOnPlay : MonoBehaviour
{
    [SerializeField] private float timer = 0.0f;

    private void Start()
    {
        Destroy(gameObject, timer);
    }
}
