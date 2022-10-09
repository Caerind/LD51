using UnityEngine;

public class FakeAgentManager : MonoBehaviour
{
    [SerializeField] private GameObject fakeAgentPrefab;

    private void Start()
    {
        General aiGeneral = GameManager.Instance.GetAIGeneral();
        foreach (Soldier soldier in aiGeneral.GetSoldiers())
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
