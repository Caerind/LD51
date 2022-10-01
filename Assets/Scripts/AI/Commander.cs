using UnityEngine;

[CreateAssetMenu()]
public class Commander : ScriptableObject
{
    [Header("ChooseBestAI")]
    public float badHealthScore;
    public float badHeatlhThreshold;

    public float badPlacementScore;
    public float badPlacementThreshold;

    public float canReachZoneScore;
    public float canReachZoneThreshold;

    public float canKillScore;
    public float canKillThreshold;

    public float canReachGoodPositionScore;
    public float canReachGoodPositionThreshold;

    public float hasntPlayedScore;
}
