using UnityEngine;

[CreateAssetMenu(fileName = "RaceConfig_", menuName = "ScriptableObjects/RaceConfig", order = 2)]
public class RaceData : ScriptableObject
{
    [SerializeField] private float mRespawnDelay = 0;
    [SerializeField] private float mDistanceToCover = 0;
    [SerializeField] private int mRequiredLaps = 0;
    [SerializeField] private int mStartTime = 0;

    public float ReSpawnDelay => mRespawnDelay;

    public float DistanceToCover => mDistanceToCover;

    public int RequiredLaps => mRequiredLaps;
    public int StartTime => mStartTime;

}
