using UnityEngine;

public class SoldierIcon : MonoBehaviour
{
    [SerializeField] private int playerIndex;
    [SerializeField] private GameObject whiteHalo;
    [SerializeField] private float transformScaleSelected = 1.2f;

    private General general;
    private Soldier soldier;
    private HealthBarMiniMap healthBar;

    private void Start()
    {
        general = GameManager.Instance.GetPlayerGeneral();

        soldier = general.GetSoldiers()[playerIndex];

        healthBar = GetComponentInChildren<HealthBarMiniMap>();
        healthBar.Init(soldier);
    }

    private void Update()
    {
        whiteHalo.SetActive(soldier == general.GetSelectedSoldier());

        if (soldier == general.GetNextSelectedSoldier())
        {
            transform.localScale = new Vector3(transformScaleSelected, transformScaleSelected, transformScaleSelected);
        }
        else
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
}
