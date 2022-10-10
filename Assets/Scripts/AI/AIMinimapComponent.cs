using UnityEngine;

public class AIMinimapComponent : MonoBehaviour
{
    [SerializeField] private GameObject redCircle;

    private Soldier soldier = null;

    private void Start()
    {
        soldier = GetComponentInParent<Soldier>();
    }

    private void Update()
    {
        if (soldier.IsInOppositeZone())
        {
            redCircle.SetActive(true);
        }
        else
        {
            redCircle.SetActive(false);
        }
    }
}
