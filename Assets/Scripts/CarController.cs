using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]


public class CarController : MonoBehaviour
{
    public const float KMH = 3.6f;
    public const float MPH = 2.24f;

    [SerializeField] protected CarData mCarConfig;
    [SerializeField] protected Transform[] mWheelMeshes;
    [SerializeField] protected WheelCollider[] mWheelColliders;
    [SerializeField] protected Rigidbody mRigidbody;
    [SerializeField] protected float mGizmoRadius;
    [SerializeField] private GameObject mBrakeLightContainer;
    [SerializeField] protected AudioSource mIdleEngineAudioSource;
    [SerializeField] protected AudioSource mAccelerationAudioSource;


    protected float mAccelerationInput;
    protected float mSteeringInput;
    protected float mAppliedBrakeTorque;
    protected float mCurrentSpeed;
    protected bool mHandBraking = false;
    protected bool mBraking = false;
    protected bool mReversing = false;
    protected bool mRaceStarted = false;

    protected Renderer[] mBrakeLightRenderers;
    protected Texture2D mLightIdleTex;
    protected Texture2D mLightBrakeTex;
    protected Texture2D mLightReverseTex;


    protected void Awake()
    {
        mRigidbody.centerOfMass += mCarConfig.CenterOfMassOffset;
        GetBrakeLightRenderers();
        LoadBrakeTextures();

    }

    protected void OnEnable()
    {
        Actions.StartRace += StartRaceTwo;
    }

    protected void OnDisable()
    {
        Actions.StartRace -= StartRaceTwo;
    }
    protected void StartRaceTwo() { mRaceStarted = true; }

    private void GetBrakeLightRenderers()
    {
        if (mBrakeLightContainer == null) { return; }
        mBrakeLightRenderers = mBrakeLightContainer .GetComponentsInChildren<Renderer>();
    }

    private void LoadBrakeTextures() 
    {
        mLightIdleTex = Resources.Load<Texture2D>("Vehicle/LightsIdle");
        mLightBrakeTex = Resources.Load<Texture2D>("Vehicle/LightsBrake");
        mLightReverseTex = Resources.Load<Texture2D>("Vehicle/LightsReverse");

    }

    private void UpdateBrakeLightState() 
    {   if (mBrakeLightRenderers == null) { return; }

        if (mBraking || mHandBraking)
        {
            foreach (Renderer r in mBrakeLightRenderers)
            {
                r.material.mainTexture = mLightBrakeTex;

            }
        }
        else if (mReversing)
        {
            foreach (Renderer r in mBrakeLightRenderers)
            {
                r.material.mainTexture = mLightReverseTex;

            }

        }
        else 
        {
            foreach (Renderer r in mBrakeLightRenderers)
            {
                r.material.mainTexture = mLightIdleTex;

            }

        }
    }

    protected void Update()
    {
        for (int i = 0; i < mWheelMeshes.Length; i++) 
        {   
            var rotationThisFrame = mWheelColliders[i].rpm * Time.deltaTime;
            mWheelMeshes[i].Rotate(rotationThisFrame, 0, 0);
        }

        for (int i = 0; i < 2; i++) 
        {
            mWheelMeshes[i].localEulerAngles = new Vector3(mWheelMeshes[i].localEulerAngles.x,
                mWheelColliders[i].steerAngle - mWheelMeshes[i].localEulerAngles.z,
                mWheelMeshes[i].localEulerAngles.z);
        }
        UpdateWheelPositions();
        UpdateBrakeLightState();
    }

    private void UpdateWheelPositions()
    {
        var contact = new WheelHit();
        for (int i = 0; i < mWheelColliders.Length; i++)
        {
            if (mWheelColliders[i].GetGroundHit(out contact))
            { 
                var tempPos = mWheelColliders[i].transform.position;
                tempPos.y = (contact.point + (mWheelColliders[i].transform.up * mWheelColliders[i].radius)).y;
                mWheelMeshes[i].position = tempPos;
            }
        }
    }

    protected void SetStiffness(float foreSlip, float sideSlip) 
    {
        //rear wheels only, start at index 2
        for (int i = 2; i < mWheelColliders.Length; ++i)
        {
            var wheelCol = mWheelColliders[i];
            var frictionCurve = wheelCol.forwardFriction;
            frictionCurve.stiffness = foreSlip;
            wheelCol.forwardFriction = frictionCurve;

            frictionCurve = wheelCol.sidewaysFriction;
            frictionCurve.stiffness = sideSlip;
            wheelCol.sidewaysFriction = frictionCurve;


        }
    
    }

    //protected void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.yellow;
    //    var carPos = transform;
    //    var temp = carPos.InverseTransformPoint(mRigidbody.centerOfMass);

    //    Gizmos.DrawSphere(temp, mGizmoRadius);
    //}
}
