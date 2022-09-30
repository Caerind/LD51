using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;

    private Inputs inputs;

    private void Start()
    {
        inputs = FindObjectOfType<Inputs>();
        PlayerManager.Instance.player = this;
    }

    private void Update()
    {
        if (inputs.move != Vector2.zero)
        {
            float s = speed * Time.deltaTime;
            transform.Translate(new Vector3(s * inputs.move.x, s * inputs.move.y, 0.0f));
        }
    }
}
