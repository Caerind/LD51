using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;

public class StartZone : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private bool PlayerZone;
    private int compteur = 0;
    private float timer=0;
    [SerializeField] private float timerMax=30;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Soldier soldier = collision.GetComponent<Soldier>();
        if (soldier != null && ( (soldier.IsPlayerSoldier() && PlayerZone == false) ||  (soldier.IsPlayerSoldier() == false && PlayerZone ==true)) )
        {
            compteur++;
            Debug.Log(soldier);
            Debug.Log("Nombre de soldat dans la zone :" + compteur);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Soldier soldier = collision.GetComponent<Soldier>();
        if (soldier != null && ((soldier.IsPlayerSoldier() && PlayerZone == false) || (soldier.IsPlayerSoldier() == false && PlayerZone == true)))
        {
            compteur--;
            Debug.Log(soldier);
            Debug.Log("Nombre de soldat dans la zone  :" + compteur);
        }
    }

    private void Update()
    {
        //test victoire
        if (timer > timerMax)
        { 
            Debug.Log("VICTORY");
        }
        //Increment Timer
        if (compteur>0)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;  
        }
    }

}
