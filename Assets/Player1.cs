using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    [SerializeField] int PlayerID;
    private General general;
    private Soldier thissoldier;
    [SerializeField] private GameObject WhiteHalo;
    [SerializeField] private HealthBarMiniMap HealthBar;
    protected HealthSystem healthSystem;

    //Recupération du général
    private void Start()
    {
        general = GameManager.Instance.GetPlayerGeneral();
        List<Soldier> soldiers = general.GetSoldiers();
        thissoldier= soldiers[PlayerID];
        WhiteHalo.SetActive(false);
        HealthBar=GetComponentInChildren<HealthBarMiniMap>();
        HealthBar.Init(thissoldier);
    }

    private void Update()
    {
        if (thissoldier==general.GetNextSelectedSoldier())
        {
            WhiteHalo.SetActive(true);
        }
        else
        {

            WhiteHalo.SetActive(false);
        }
    }
}
