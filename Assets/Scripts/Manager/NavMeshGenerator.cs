using UnityEngine;
using UnityEngine.AI;

public class NavMeshGenerator : Singleton<NavMeshGenerator>
{
    private NavMeshSurface m_surface;

    private bool m_navmeshReady = false;

    private void Awake()
    {
        m_surface = GetComponent<NavMeshSurface>();
    }

    public bool IsNavMeshReady()
    {
        return m_navmeshReady;
    }

    public void RebuildNavMeshAsync()
    {
        m_navmeshReady = false;
        m_surface.hideEditorLogs = true;
        m_surface.BuildNavMeshAsync().completed += OnNavMeshGenCompleted;
    }

    public void RebuildNavMeshOnSceneChange(RegionLoaderComponent regionLoader)
    {
        RebuildNavMeshAsync();
    }

    private void OnNavMeshGenCompleted(AsyncOperation operation)
    {
        if (operation.isDone)
        {
            operation.completed -= OnNavMeshGenCompleted;
            m_navmeshReady = true;
        }
    }
}