using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSelection : MonoBehaviour
{
    //Recup�ration du g�n�ral
    General general = GameManager.Instance.GetPlayerGeneral();

    private void Update()
    {
        Soldier soldier=general.GetSelectedSoldier();
    }
}
