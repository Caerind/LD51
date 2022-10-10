using System;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        unlockedGameScenes = new List<int>();
        unlockedGameScenes.Add(0);
    }

    public void SetPlayerWin(bool hasPlayerWin)
    {
        this.hasPlayerWin = hasPlayerWin;
        if (hasPlayerWin)
        {
            unlockedGameScenes.AddUnique(currentGameScene + 1);
        }
        endGameScene.LoadScene();
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
        gameScenes[currentGameScene].scene.LoadScene();
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
