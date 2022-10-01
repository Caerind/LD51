using UnityEngine;

public class GameApplication : Singleton<GameApplication>
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void StartApplication()
    {
        GameObject gameObject = new GameObject("GameApplication");
        gameObject.AddComponent<GameApplication>();
        gameObject.AddComponent<GameManager>();
        DontDestroyOnLoad(gameObject);
    }
}
