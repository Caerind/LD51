using UnityEngine;

public class FakeAgentManager : MonoBehaviour
{
    [SerializeField] private GameObject fakeAgentPrefab;

    private void Start()
    {
        General enemyGeneral = GameManager.Instance.GetEnemyGeneral();
        foreach (Soldier soldier in enemyGeneral.GetSoldiers())
        {
            InstantiateForSoldier(soldier);
        }
    }

    private void InstantiateForSoldier(Soldier soldier)
    {
        GameObject newFakeAgent = Instantiate(fakeAgentPrefab);
        newFakeAgent.GetComponent<FakeAgent>().Init(soldier);
        newFakeAgent.transform.parent = transform;
    }
}
