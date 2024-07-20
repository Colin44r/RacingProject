using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class AICar : CarController
{
   // private static AICar instance;
  //  public static AICar Instance => instance;

    [SerializeField] private GameObject mWaypointContainer;
    private float mDistanceSqr;
    private Transform[] mWaypoints;
    private int mCurrentWaypoint = 0;
  //  private int mCountdownTimerDelay;
  //  private float mCountdownTimerStartTime;
    private bool mGameStart = false;
  //test


    public Transform CurrentWaypoint => mWaypoints[mCurrentWaypoint];

    public Transform LastWaypoint
    {
        get
        {
            return (mCurrentWaypoint - 1 < 0) ? mWaypoints[mWaypoints.Length - 1] : mWaypoints[mCurrentWaypoint - 1];
        }
    }

    private void GetWaypoints()
    {
        //string test = (mCurrentWaypoint < 0) ? "True" : "False";
        //Debug.Log(test);
        var potentialWaypoints = mWaypointContainer.GetComponentsInChildren<Transform>();
        mWaypoints = new Transform[potentialWaypoints.Length - 1];

        for (int i = 1; i < potentialWaypoints.Length; i++)
        {
            mWaypoints[i - 1] = potentialWaypoints[i];

        }

    }

    private void Start()
    {
        GetWaypoints();
        mDistanceSqr = mCarConfig.WaypointDistance * mCarConfig.WaypointDistance;

    }
    private void CheckWaypointPosition(Vector3 relativeWayPointPos)
    {
        if (relativeWayPointPos.sqrMagnitude < mDistanceSqr)
        {
            mCurrentWaypoint += 1;
            if (mCurrentWaypoint == mWaypoints.Length)
            {
                mCurrentWaypoint = 0;
                RaceManager.Instance.LapFinishByAI(this);
            }


        }

    }


    private new void Update()
    {
        if (RaceManager.Instance.GameStart == true)
        {
            var waypointPosition = mWaypoints[mCurrentWaypoint].position;
            var relativeWaypointPos = transform.InverseTransformPoint(new Vector3(waypointPosition.x,
                transform.position.y, waypointPosition.z));

            var localVelocity = transform.InverseTransformDirection(mRigidbody.velocity);

            var spacingAdjustment = SideCrashSteeringAdjustment();
            var brakingAdjustment = ForwardCrashBrakingAdjustment();

            mCurrentSpeed = localVelocity.z * KMH;

            //steering amount based on left / right of the waypoint
            mSteeringInput = relativeWaypointPos.x / relativeWaypointPos.magnitude + spacingAdjustment;

            //acceleration depends on distance to waypoint and steering 
            mAccelerationInput = (relativeWaypointPos.normalized.magnitude - Mathf.Abs(mSteeringInput)) * brakingAdjustment;

            //waypoint is behind vehicle
            if (relativeWaypointPos.z < 0)
            {
                mSteeringInput *= -1;
                mAccelerationInput = -1.0f;

            }

            // speed limit
            if (mCurrentSpeed < mCarConfig.MaxSpeed || mCurrentSpeed > mCarConfig.MaxReverseSpeed)
            {
                // acceleration 
                for (int i = 0; i < 2; i++)
                {
                    mWheelColliders[i].motorTorque = mAccelerationInput * mCarConfig.MaxTorque;
                }
            }
            else
            {
                //too fast
                for (int i = 0; i < 2; i++)
                {
                    mWheelColliders[i].motorTorque = 0;

                }
            }

            for (int i = 0; i < 2; i++)
            {
                //acceleration for the front wheels 
                //steering front wheels
                mWheelColliders[i].steerAngle = mSteeringInput * mCarConfig.MaxSteerAngle;

            }



            mRigidbody.AddForce(-transform.up * (localVelocity.z * mCarConfig.SpoilderRatio), ForceMode.Force);
            var brakeTorqueChanged = false;


            if ((mAccelerationInput < 0 && localVelocity.z > 0) || (mAccelerationInput > 0 && localVelocity.z < 0))
            {
                mAppliedBrakeTorque = mCarConfig.BrakingTorque + mCarConfig.MaxTorque;
                brakeTorqueChanged = true;

            }
            else if (mAccelerationInput == 0)
            {
                mAppliedBrakeTorque = mCarConfig.BrakingTorque;
                brakeTorqueChanged = true;
            }
            else
            {
                mAppliedBrakeTorque = 0;
                brakeTorqueChanged = true;
            }

            if (brakeTorqueChanged)
            {

                for (int i = 0; i > mWheelColliders.Length; i++)
                {
                    mWheelColliders[i].brakeTorque = mAppliedBrakeTorque;
                }
            }

            CheckWaypointPosition(relativeWaypointPos);
            base.Update();

        }
    }
    private float ForwardCrashBrakingAdjustment()
    {
        var origin = transform.GetComponentInChildren<Renderer>().bounds.center;
        var rayStart = origin + (transform.forward * mCarConfig.BrakingOffSet);
        Debug.DrawRay(rayStart, transform.forward * mCarConfig.BrakingDistance);

        if (Physics.Raycast(rayStart, transform.forward, out RaycastHit hit, mCarConfig.BrakingDistance))
        {
            return ((rayStart - hit.point).magnitude / mCarConfig.BrakingDistance * 2f) - 1f;
        }
        else
        {
            return 1f;
        }
    }

    private float SideCrashSteeringAdjustment()
    {
        var steerAdjust = 0.0f;
        var origin = transform.GetComponentInChildren<Renderer>().bounds.center;
        var carRightSide = origin + (transform.right * mCarConfig.SpacingOffSet);
        var carLeftSide = origin + (-transform.right * mCarConfig.SpacingOffSet);
        Debug.DrawRay(carRightSide, transform.right * mCarConfig.SpacingDistance);
        Debug.DrawRay(carRightSide, -transform.right * mCarConfig.SpacingDistance);

        if (Physics.Raycast(carRightSide, transform.right, out RaycastHit hit, mCarConfig.SpacingDistance))
        {
            steerAdjust += -1 + (carRightSide - hit.point).magnitude / mCarConfig.SpacingDistance;

        }

        if (Physics.Raycast(carLeftSide, -transform.right, out hit, mCarConfig.SpacingDistance))
        {
            steerAdjust += -1 + (carLeftSide - hit.point).magnitude / mCarConfig.SpacingDistance;

        }
        return steerAdjust;

  
    }


}
