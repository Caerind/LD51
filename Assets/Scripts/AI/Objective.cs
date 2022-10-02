using UnityEngine;

public enum ObjectiveType
{
    ReachInterestPoint,
    AttackPlayerZone,
    DefendEnemyZone,
    AttackPlayer
}

public class Objective
{
    public ObjectiveType type { get; set; }
    public GameObject targetGameObject { get; set; }
    public GameObject aiGameObject { get; set; }
    public float score { get; set; }

    public Objective(ObjectiveType type, GameObject targetGameObject, GameObject aiGameObject, float score)
    {
        this.type = type;
        this.targetGameObject = targetGameObject;
        this.aiGameObject = aiGameObject;
        this.score = score;
    }
}
