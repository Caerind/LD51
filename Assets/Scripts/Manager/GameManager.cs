using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private General playerGeneral = null;
    private General aiGeneral = null;
    private List<InterestPoint> interestPoints = new List<InterestPoint>();
    private List<ZonePoint> playerZonePoints = new List<ZonePoint>();
    private List<ZonePoint> enemyZonePoints = new List<ZonePoint>();

    private float timer = 0.0f;
    public const float timerMax = 10.0f;

    private bool isPlaying = false;

    private void Update()
    {
        if (isPlaying)
        {
            timer += Time.deltaTime;
            if (timer >= timerMax)
            {
                timer = 0.0f;
                playerGeneral.SelectNextPlayer();
                aiGeneral.SelectNextPlayer();
            }

            UpdateCheckGameFinished();
        }
        else if (playerGeneral != null && aiGeneral != null)
        {
            playerGeneral.SelectNextPlayer();
            aiGeneral.SelectNextPlayer();
            isPlaying = true;
            AudioManager.PlaySound("TICTAC");
            AudioManager.PlaySound("Battement");
        }
    }

    private void UpdateCheckGameFinished()
    {
        // Has player win ?
        if (playerGeneral != null && playerGeneral.GetSelectableSoldiersCount() == 0)
        {
            GameManager.Instance.Reset();
            GameApplication.Instance.SetPlayerWin(false);
            return;
        }

        // Has ai win ?
        if (aiGeneral != null && aiGeneral.GetSoldiers() != null && aiGeneral.GetSoldiers().Count == 0)
        {
            GameManager.Instance.Reset();
            GameApplication.Instance.SetPlayerWin(false);
            return;
        }
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }

    /*
    private void OnGUI()
    {
        GUI.Label(new Rect(5, 5, 100, 25), "ZonePlayer: " + ((AIGeneral)aiGeneral).GetPlayerZone().GetEnemyInZoneCounter().ToString());
        GUI.Label(new Rect(5, 35, 100, 25), "ZoneEnemy: " + ((AIGeneral)aiGeneral).GetEnemyZone().GetEnemyInZoneCounter().ToString());
        GUI.Label(new Rect(5, 60, 100, 25), "AINext: " + aiGeneral.GetNextSelectedIndex().ToString());
    }
    */

    public float GetTimer()
    {
        return timer;
    }

    public General GetPlayerGeneral()
    {
        return playerGeneral;
    }

    public General GetEnemyGeneral()
    {
        return aiGeneral;
    }

    public void RegisterGeneral(General general)
    {
        if (general.IsPlayerGeneral())
        {
            playerGeneral = general;
        }
        else
        {
            aiGeneral = general;
        }
    }

    public void RegisterInterestPoint(InterestPoint point)
    {
        interestPoints.Add(point);
    }

    public void RegisterZonePoint(ZonePoint point)
    {
        if (point.isPlayerZonePoint)
        {
            playerZonePoints.Add(point);
        }
        else
        {
            enemyZonePoints.Add(point);
        }
    }

    public List<InterestPoint> GetAllInterestPoints()
    {
        return interestPoints;
    }

    public List<ZonePoint> GetAllPlayerZonePoints()
    {
        return playerZonePoints;
    }

    public List<ZonePoint> GetAllEnemyZonePoints()
    {
        return enemyZonePoints;
    }

    public void Reset()
    {
        isPlaying = false;
        interestPoints.Clear();
        playerGeneral = null;
        aiGeneral = null;
        playerZonePoints.Clear();
        enemyZonePoints.Clear();
    }
}
