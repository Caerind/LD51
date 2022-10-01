using UnityEngine;

public class GameApplication : Singleton<GameApplication>
{
    private void Start()
    {
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
