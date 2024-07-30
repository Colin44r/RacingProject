using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Cars : CarController
{
    private const string VERTICAL = "Vertical";
    private const string HORIZONTAL = "Horizontal";
    private const string HANDBRAKE = "HandBrake";
    private const float WHEEL_STIFFNESS = 1.0f;

    [SerializeField] Texture2D mSpeedometer;
    [SerializeField] Texture2D mNeedle;
    [SerializeField] GameObject mRearCamera;
    [SerializeField] private AudioSource GoldSFX;
    [SerializeField] private Camera mMainCamera;


    private float mWidthOffset = 300;
    private float mWidth= 300;
    private float mHeightOffset = 150;
    private float mHeight= 150;
    private float mMinFOV = 60;
    private float mMaxFOV = 90;





    private void OnGUI() 
    {
        GUI.DrawTexture(new Rect(Screen.width - mWidthOffset, Screen.height - mHeightOffset,
            mWidth,mHeightOffset), mSpeedometer);

        var speedFactor = mCurrentSpeed / mCarConfig.MaxSpeed;
        var rotationAngle = Mathf.Lerp(0, 180, Mathf.Abs(speedFactor));

        GUIUtility.RotateAroundPivot(rotationAngle, new Vector2(Screen.width - mHeightOffset, Screen.height));
        GUI.DrawTexture(new Rect(Screen.width - mWidthOffset, Screen.height - mHeightOffset, mWidth, mWidth), mNeedle);

    }

    new private void Update()
    {
      
        var localVelocity = transform.InverseTransformDirection(mRigidbody.velocity);

        //convert from m/s to km/h
        mCurrentSpeed = localVelocity.z * KMH;

        // speed limit
        if (mCurrentSpeed < mCarConfig.MaxSpeed || mCurrentSpeed > mCarConfig.MaxReverseSpeed)
        {
            // acceleration 
            for (int i = 0; i < 2; i++)
            {
                mWheelColliders[i].motorTorque = mAccelerationInput * mCarConfig.MaxTorque;
            }
        }
        else {
            //too fast
            for (int i = 0; i < 2; i++)
            {
                mWheelColliders[i].motorTorque = 0;

            }
        }



        if (RaceManager.Instance.GameStart == true)
        {
            mAccelerationInput = Input.GetAxis(VERTICAL);
            mSteeringInput = Input.GetAxis(HORIZONTAL);
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
            mBraking = true;
            mAccelerationAudioSource.Play();

        }
        else if (mAccelerationInput == 0)
        {
            mAppliedBrakeTorque = mCarConfig.BrakingTorque;
            brakeTorqueChanged = true;
            mBraking = false;
            mIdleEngineAudioSource.Play();
        }
        else
        {
            mAppliedBrakeTorque = 0;
            brakeTorqueChanged = true;
            mBraking = false;
            mReversing = localVelocity.z <0;
        }


        if (Input.GetButton(HANDBRAKE))
        {
            mHandBraking = true;
            mAppliedBrakeTorque = mCarConfig.BrakingTorque + mCarConfig.MaxTorque;
            brakeTorqueChanged = true;


            if (mRigidbody.velocity.sqrMagnitude > 0)
            {
                SetStiffness(mCarConfig.HandBreakFowardStiffness, mCarConfig.HandBreakSidewayStiffness);
            }
            else
            {
                SetStiffness(WHEEL_STIFFNESS, WHEEL_STIFFNESS);
            }

        }
        else
        {
            mHandBraking = false;
            brakeTorqueChanged = true;
            mAppliedBrakeTorque = 0;
            SetStiffness(WHEEL_STIFFNESS, WHEEL_STIFFNESS);


        }

        if (brakeTorqueChanged)
        {

            for (int i = 0; i > mWheelColliders.Length; i++)
            {
                mWheelColliders[i].brakeTorque = mAppliedBrakeTorque;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            mRearCamera.SetActive(true);
            Debug.Log("test");
        }
      
        else if (Input.GetKeyUp(KeyCode.F))
        {
            mRearCamera.SetActive(false);

        }

        mMainCamera.fieldOfView = Mathf.Clamp(mCurrentSpeed, mMinFOV, mMaxFOV);

        base.Update();
    }


    private void OnTriggerEnter(Collider collision)
    {
       
        if (collision.gameObject.tag == "GoldCoins")
        {
            Debug.Log(collision.gameObject.name);
            GoldSFX.Play();
            collision.gameObject.SetActive(false);
        }

    }

}
