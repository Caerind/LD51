using UnityEngine;

public class GameApplication : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("HelloWorld");
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void StartApplication()
    {
        GameObject gameObject = new GameObject("GameApplication");
        gameObject.AddComponent<GameApplication>();
        gameObject.AddComponent<PlayerManager>();
        DontDestroyOnLoad(gameObject);
    }
}
