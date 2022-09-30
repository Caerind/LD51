using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject m_prefab = null;
    [SerializeField] private bool m_spawnOnStart = true;

    private bool m_havePendingSpawnRequest;

    private void Start()
    {
        if (m_spawnOnStart)
        {
            RequestSpawn();
        }
    }

    public void RequestSpawn()
    {
        m_havePendingSpawnRequest = true;
    }

    protected virtual void Spawn()
    {
        GameObject.Instantiate(m_prefab, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        if (m_havePendingSpawnRequest)
        {
            if (NavMeshGenerator.Instance.IsNavMeshReady())
            {
                Spawn();
                m_havePendingSpawnRequest = false;
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }

    protected void OnDrawGizmosSelected()
    {
        if (!EditorApplication.isPlaying) // Always drawn when selected
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 0.5f);

            SpriteRenderer spriteRenderer = m_prefab?.GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null && spriteRenderer.sprite != null)
            {
                GizmosUtils.DrawSprite(spriteRenderer.sprite, transform.position);
            }
        }
    }
#endif // UNITY_EDITOR
}
