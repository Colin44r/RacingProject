using UnityEngine;

[CreateAssetMenu(fileName = "CarConfig_", menuName = "ScriptableObjects/CarConfig", order = 0)]
public class CarData : ScriptableObject
{
    [SerializeField] private float mMaxTorque = 0;
    [SerializeField] private float mMaxSteerAngle = 0;
    [SerializeField] private float mSpoilerRatio = 0;
    [SerializeField] private float mMaxAntiRollForce = 0;
    [SerializeField] private float mTireRPM = 0;
    [SerializeField] private float mBrakingTorque = 0;
    [SerializeField] private float mMaxSpeed = 0;
    [SerializeField] private float mMaxReverseSpeed = 0;
    [SerializeField] private float mHandBreakFowardStiffness =0 ;
    [SerializeField] private float mHandBreakSidewayStiffness =0;
    [SerializeField] private float mSlipThreshold = 0;
    [SerializeField] private float mPlacementOffset = 0;
    [SerializeField] private float mWaypointDistance = 0;
    [SerializeField] private float mBrakingOffSet = 0;
    [SerializeField] private float mBrakingDistance = 0;
    [SerializeField] private float mSpacingDistance = 0;
    [SerializeField] private float mSpacingOffSet = 0;
    [SerializeField] private Vector3 mCenterOfMassOffset = Vector3.zero;


    public float MaxTorque =>  mMaxTorque ;
    public float MaxSteerAngle => mMaxSteerAngle ;
    public float SpoilderRatio => mSpoilerRatio;
    public float SpoilerRatio => mSpoilerRatio;
    public float MaxAntiRollForce => mMaxAntiRollForce;
    public float TireRPM => mTireRPM;
    public float BrakingTorque => mBrakingTorque;
    public float MaxSpeed => mMaxSpeed;
    public float MaxReverseSpeed => mMaxReverseSpeed;
    public float HandBreakFowardStiffness => mHandBreakFowardStiffness ;
    public float HandBreakSidewayStiffness => mHandBreakSidewayStiffness ;
    public float SlipThreshold => mSlipThreshold;
    public float PlacementOffset => mPlacementOffset;
    public float WaypointDistance => mWaypointDistance;
    public float BrakingDistance => mBrakingOffSet; 
    public float  BrakingOffSet => mBrakingDistance ;
   public float   SpacingDistance=>mSpacingDistance ;
    public float  SpacingOffSet=>mSpacingOffSet ;
    public Vector3 CenterOfMassOffset => mCenterOfMassOffset;
 
}
