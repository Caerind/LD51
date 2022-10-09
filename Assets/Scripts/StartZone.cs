using UnityEngine;

public class StartZone : MonoBehaviour
{
    [SerializeField] private bool isPlayerZone;
    [SerializeField] private float timeToRemainInZoneToWin = 30.0f;

    private int enemyInZoneCounter = 0;
    private float enemyInZoneTimer = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Soldier soldier = collision.GetComponentInParent<Soldier>();
        if (soldier != null && soldier.IsPlayerSoldier() != !isPlayerZone)
        {
            enemyInZoneCounter++;

            soldier.SetIsInOppositeZone(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Soldier soldier = collision.GetComponentInParent<Soldier>();
        if (soldier != null && soldier.IsPlayerSoldier() != isPlayerZone)
        {
            enemyInZoneCounter--;

            soldier.SetIsInOppositeZone(false);
        }
    }

    private void Update()
    {
        if (enemyInZoneCounter > 0)
        {
            // TODO : UI Timer Win

            enemyInZoneTimer += Time.deltaTime;

            if (enemyInZoneTimer >= timeToRemainInZoneToWin)
            {
                GameManager.Instance.Reset();
                GameApplication.Instance.SetPlayerWin(!isPlayerZone);
                return;
            }
        }
        else
        {
            enemyInZoneTimer = 0.0f;  
        }
    }

    public bool IsPlayerZone()
    {
        return isPlayerZone;
    }

    public int GetEnemyInZoneCounter()
    {
        return enemyInZoneCounter;
    }

    public float GetEnemyInZoneTimer()
    {
        return enemyInZoneTimer;
    }
}
