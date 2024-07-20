using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class AntiRollBar : MonoBehaviour
{
    [SerializeField] private CarData mCarConfig;
    [SerializeField] private WheelCollider mLeftWheel;
    [SerializeField] private WheelCollider mRightWheel;
    [SerializeField] private Rigidbody mRigidBody;

    private void FixedUpdate()
    {
        WheelHit hit;

        var leftTravel = 1.0f;
        var rightTravel = 1.0f;

        var isLeftGrounded = mLeftWheel.GetGroundHit(out hit);
        if (isLeftGrounded)
        {
            leftTravel = (-mLeftWheel.transform.InverseTransformPoint(hit.point).y - mLeftWheel.radius)
                /mLeftWheel.suspensionDistance;

        }

        var isRightGrounded = mRightWheel.GetGroundHit(out hit);
        if (!isRightGrounded)
        {
            rightTravel = (-mRightWheel.transform.InverseTransformPoint(hit.point).y - mRightWheel.radius)
                / mRightWheel.suspensionDistance;
        }

        var antiRollForce = (leftTravel - rightTravel) * mCarConfig.MaxAntiRollForce;

        if (isLeftGrounded)
        {
         mRigidBody.AddForceAtPosition(mLeftWheel.transform.up * -antiRollForce,  mLeftWheel.transform.position);
        }
          
          
        

        if (isRightGrounded)
        {
            mRigidBody.AddForceAtPosition(mLeftWheel.transform.up * antiRollForce, mLeftWheel.transform.position);
        }
    }
}
