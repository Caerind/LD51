using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameApplication : Singleton<GameApplication>
{
    [SerializeField] private SceneReference menuScene;
    [SerializeField] private SceneReference campaignScene;
    [SerializeField] private List<SceneReference> gameScenes;
    [SerializeField] private SceneReference endGameScene;
    private bool hasPlayerWin;

    public void SetHasPlayerWin(bool hasPlayerWin)
    {
        this.hasPlayerWin = hasPlayerWin;
    }

    public bool GetHasPlayerWin()
    {
        return hasPlayerWin;
    }

    public void GoToMenuScene()
    {
        SceneManager.LoadScene(menuScene.ScenePath);
    }

    public void GoToCampaignGameScene()
    {
        SceneManager.LoadScene(endGameScene.ScenePath);
    }

    public int GetGameScenesCount()
    {
        return gameScenes.Count;
    }

    public void GoToGameScene(int index)
    {
        SceneManager.LoadScene(gameScenes[index].ScenePath);
    }

    public void GoToEndGameScene()
    {
        SceneManager.LoadScene(endGameScene.ScenePath);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void StartApplication()
    {
        GameObject gameObject = new GameObject("GameApplication");
        gameObject.AddComponent<GameApplication>();
        gameObject.AddComponent<GameManager>();
        DontDestroyOnLoad(gameObject);
    }
}
