using UnityEngine;

public class StartZone : MonoBehaviour
{
    [SerializeField] private bool playerZone;
    [SerializeField] private float timerMax = 30.0f;

    private int compteur = 0;
    private float timer = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Soldier soldier = collision.GetComponentInParent<Soldier>();
        if (soldier != null && ((soldier.IsPlayerSoldier() && playerZone == false) || (soldier.IsPlayerSoldier() == false && playerZone == true)))
        {
            compteur++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Soldier soldier = collision.GetComponentInParent<Soldier>();
        if (soldier != null && ((soldier.IsPlayerSoldier() && playerZone == false) || (soldier.IsPlayerSoldier() == false && playerZone == true)))
        {
            compteur--;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.GetPlayerGeneral().GetSoldiers().Count == 0)
        {
            GameManager.Instance.Reset();
            GameApplication.Instance.SetPlayerWin(false);
        }
        if (GameManager.Instance.GetEnemyGeneral().GetSoldiers().Count == 0)
        {
            GameManager.Instance.Reset();
            GameApplication.Instance.SetPlayerWin(true);
        }

        // TODO : UI Timer Win

        // Increment Timer
        if (compteur > 0)
        {
            timer += Time.deltaTime;

            if (timer > timerMax)
            {
                GameManager.Instance.Reset();
                GameApplication.Instance.SetPlayerWin(!playerZone);
            }
        }
        else
        {
            timer = 0;  
        }
    }

    public int GetCompteur()
    {
        return compteur;
    }

#if UNITY_EDITOR
    /*
    [SerializeField] private int genZonePointCount = 30;
    [SerializeField] private float genZonePointInterval = -4.0f;
    [SerializeField] private float yDir = 2;
    [CustomEditor(typeof(StartZone))]
    internal class StartZoneEditor : Editor
    {
        private StartZone zone => target as StartZone;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("GeneratePoints"))
            {
                zone.GenPoints();
            }
        }
    }
    public void GenPoints()
    {
        float side = playerZone ? 1.0f : -1.0f;
        for (int i = 0; i < genZonePointCount; ++i)
        {
            GameObject go = new GameObject();
            go.transform.parent = transform;
            go.transform.localPosition = new Vector3(i * genZonePointInterval, side * yDir, 0.0f);
            go.AddComponent<ZonePoint>().isPlayerZonePoint = playerZone;
        }
        for (int i = 1; i < genZonePointCount; ++i)
        {
            GameObject go = new GameObject();
            go.transform.parent = transform;
            go.transform.localPosition = new Vector3(-i * genZonePointInterval, side * yDir, 0.0f);
            go.AddComponent<ZonePoint>().isPlayerZonePoint = playerZone;
        }
    }
    */
#endif // UNITY_EDITOR
}
