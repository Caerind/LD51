using UnityEngine;

public class HealthBarSystem : MonoBehaviour
{
    [SerializeField] private GameObject healthBarPrefab;

    private void Start()
    {
        General playerGeneral = GameManager.Instance.GetPlayerGeneral();
        foreach (Soldier soldier in playerGeneral.GetSoldiers())
        {
            InstantiateForSoldier(soldier);
        }

        General aiGeneral = GameManager.Instance.GetAIGeneral();
        foreach (Soldier soldier in aiGeneral.GetSoldiers())
        {
            InstantiateForSoldier(soldier);
        }
    }

    private void InstantiateForSoldier(Soldier soldier)
    {
        GameObject newHealthBar = Instantiate(healthBarPrefab);
        newHealthBar.GetComponent<HealthBar>().Init(soldier);
        newHealthBar.transform.parent = transform;
    }
}
