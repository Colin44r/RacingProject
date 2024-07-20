using UnityEngine;

public class ChaseCamera : MonoBehaviour
{
    [SerializeField] private CameraData mCameraConfig;
    [SerializeField] private Transform mChaseTarget;
    private float mDesiredAngle = 0;
    private float mDesiredHeight = 0;

    private void LateUpdate()
    {
        // Get the current yaw and height of the camera 
        var currentAngle = transform.eulerAngles.y;
        var currentHeight = transform.position.y;

        // Figure out desired rotation and height for the camera 
        mDesiredAngle = mChaseTarget.eulerAngles.y;
        mDesiredHeight = mChaseTarget.position.y + mCameraConfig.Height;

        // Figure out adjustments to move from the current towards desired values over time 
        currentAngle = Mathf.LerpAngle(currentAngle, mDesiredAngle, mCameraConfig.RotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, mDesiredHeight, mCameraConfig.HeightDamping * Time.deltaTime);

        //Create a quaternion from new yaw angle
        var currentRotation = Quaternion.Euler(0, currentAngle, 0);

        //Rotating a forward vector behind the target and scaling to chase distance
        var finalPosition = mChaseTarget.position - (currentRotation * Vector3.forward * mCameraConfig.Distance);

        //Set the final height to the new height
        finalPosition.y = currentHeight;

        //Move the camera
        transform.position = finalPosition;

        // Make sure camera is looking at the chase target
        transform.LookAt(mChaseTarget);

    }

}
