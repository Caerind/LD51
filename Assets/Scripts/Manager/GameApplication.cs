using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameApplication : Singleton<GameApplication>
{
    [Serializable]
    public struct SceneInfo
    {
        public string name;
        public string text;
        public Sprite icon;
        public Sprite map;
        public SceneReference scene;
    }

    [SerializeField] private SceneReference endGameScene;
    [SerializeField] private List<SceneInfo> gameScenes;

    private List<int> unlockedGameScenes;
    private int currentGameScene;

    private bool hasPlayerWin;

    private Texture2D cursorTexture;
    private bool showCursor = false;

    private void Start()
    {
        unlockedGameScenes = new List<int>();
        unlockedGameScenes.Add(0);

        GameObject mouseCursor = Resources.Load<GameObject>("MouseCursor");
        cursorTexture = mouseCursor.GetComponent<SpriteRenderer>().sprite.texture;
        ShowCursor(true);
    }

    private void OnMouseEnter()
    {
        ShowCursor(true);
    }

    private void OnMouseExit()
    {
        ShowCursor(false);
    }

    public void ShowCursor(bool visible)
    {
        if (showCursor != visible)
        {
            if (visible)
            {
                Cursor.SetCursor(cursorTexture, new Vector2(32.0f, 32.0f), CursorMode.ForceSoftware);
            }
            else
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
            showCursor = visible;
        }
    }

    public void SetPlayerWin(bool hasPlayerWin)
    {
        this.hasPlayerWin = hasPlayerWin;
        if (hasPlayerWin)
        {
            unlockedGameScenes.AddUnique(currentGameScene + 1);
        }
        SceneManager.LoadScene(endGameScene.ScenePath);
    }

    public bool GetHasPlayerWin()
    {
        return hasPlayerWin;
    }

    public bool HasUnlockedGameScene(int index)
    {
        return unlockedGameScenes.Contains(index);
    }

    public void UnlockedGameScene(int index)
    {
        unlockedGameScenes.AddUnique(index);
    }

    public void SetCurrentScene(int index)
    {
        currentGameScene = index;
    }

    public int GetGameSceneCount()
    {
        return gameScenes.Count;
    }

    public SceneInfo GetGameScene(int index)
    {
        return gameScenes[index];
    }

    public void GoToCurrentScene()
    {
        GameManager.Instance.Reset();
        SceneManager.LoadScene(gameScenes[currentGameScene].scene.ScenePath);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void StartApplication()
    {
        var app = GameObject.Instantiate(Resources.Load("GameApplication"));
        if (app == null)
            throw new ApplicationException();
        DontDestroyOnLoad(app);
    }
}
