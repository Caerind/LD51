using UnityEngine;

public class PrefabManager: Singleton<PrefabManager>
{
    [SerializeField] private GameObject deadBodyAIPrefab;
    [SerializeField] private GameObject deadBodyPlayerPrefab;
    [SerializeField] private GameObject bulletProjectilePrefab;

    public GameObject GetDeadBodyAIPrefab() => deadBodyAIPrefab;
    public GameObject GetDeadBodyPlayerPrefab() => deadBodyPlayerPrefab;
    public GameObject GetBulletProjectilePrefab() => bulletProjectilePrefab;
}
