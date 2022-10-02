using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSelection : MonoBehaviour
{
    //Recupération du général
    General general = GameManager.Instance.GetPlayerGeneral();

    private void Update()
    {
        Soldier soldier=general.GetSelectedSoldier();
    }
}
