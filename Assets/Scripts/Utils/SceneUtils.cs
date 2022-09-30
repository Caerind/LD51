using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneUtils
{
    public static void MoveGameObjectToMainScene(GameObject gameObject)
    {
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }
}
