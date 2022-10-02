using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StartZone : MonoBehaviour
{
    [SerializeField] private bool playerZone;
    [SerializeField] private float timerMax = 30.0f;

    private int compteur = 0;
    private float timer = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Soldier soldier = collision.GetComponent<Soldier>();
        if (soldier != null && ( (soldier.IsPlayerSoldier() && playerZone == false) ||  (soldier.IsPlayerSoldier() == false && playerZone == true)) )
        {
            compteur++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Soldier soldier = collision.GetComponent<Soldier>();
        if (soldier != null && ((soldier.IsPlayerSoldier() && playerZone == false) || (soldier.IsPlayerSoldier() == false && playerZone == true)))
        {
            compteur--;
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
        if (compteur > 0)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;  
        }
    }

    public int GetCompteur()
    {
        return compteur;
    }

#if UNITY_EDITOR
    /*
    [SerializeField] private int genZonePointCount = 30;
    [SerializeField] private float genZonePointInterval = -4.0f;
    [SerializeField] private float yDir = 2;
    [CustomEditor(typeof(StartZone))]
    internal class StartZoneEditor : Editor
    {
        private StartZone zone => target as StartZone;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("GeneratePoints"))
            {
                zone.GenPoints();
            }
        }
    }
    public void GenPoints()
    {
        float side = playerZone ? 1.0f : -1.0f;
        for (int i = 0; i < genZonePointCount; ++i)
        {
            GameObject go = new GameObject();
            go.transform.parent = transform;
            go.transform.localPosition = new Vector3(i * genZonePointInterval, side * yDir, 0.0f);
            go.AddComponent<ZonePoint>().isPlayerZonePoint = playerZone;
        }
        for (int i = 1; i < genZonePointCount; ++i)
        {
            GameObject go = new GameObject();
            go.transform.parent = transform;
            go.transform.localPosition = new Vector3(-i * genZonePointInterval, side * yDir, 0.0f);
            go.AddComponent<ZonePoint>().isPlayerZonePoint = playerZone;
        }
    }
    */
#endif // UNITY_EDITOR
}
