using UnityEngine;

public class SoldierIcon : MonoBehaviour
{
    [SerializeField] private int PlayerIndex;
    [SerializeField] private GameObject WhiteHalo;
    [SerializeField] private float selectedTransformScale = 1.1f;

    private General general;
    private Soldier soldier;
    private HealthBarMiniMap HealthBar;

    private void Start()
    {
        general = GameManager.Instance.GetPlayerGeneral();

        soldier = general.GetSoldiers()[PlayerIndex];

        HealthBar = GetComponentInChildren<HealthBarMiniMap>();
        HealthBar.Init(soldier);
    }

    private void Update()
    {
        WhiteHalo.SetActive(soldier == general.GetSelectedSoldier());

        if (soldier == general.GetNextSelectedSoldier())
        {
            transform.localScale = new Vector3(selectedTransformScale, selectedTransformScale, selectedTransformScale);
        }
        else
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
}
