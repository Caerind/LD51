using UnityEngine;

public class GameApplication : Singleton<GameApplication>
{
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

    public SceneReference GetEndGameScene()
    {
        return endGameScene;
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
