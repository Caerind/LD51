using UnityEngine;

public class HoleDetector : MonoBehaviour
{
    [SerializeField] private Soldier soldier;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        soldier.SetOnHole(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        soldier.SetOnHole(false);
    }
}
