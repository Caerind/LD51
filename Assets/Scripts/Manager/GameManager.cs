using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private General playerGeneral = null;
    private General aiGeneral = null;

    private float timer = 0.0f;
    public const float timerMax = 10.0f;

    private bool isPlaying = true;

    private void Start()
    {
    }

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
        }
    }

    public void SetPlaying(bool playing)
    {
        isPlaying = playing;
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(5, 5, 100, 25), timer.ToString());
        GUI.Label(new Rect(5, 35, 100, 25), "PlayerNext: " + playerGeneral.GetNextSelectedIndex().ToString());
        GUI.Label(new Rect(5, 60, 100, 25), "AINext: " + aiGeneral.GetNextSelectedIndex().ToString());
    }

    public float GetTimer()
    {
        return timer;
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
}
