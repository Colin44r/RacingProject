using UnityEngine;


[RequireComponent(typeof(WheelCollider)), RequireComponent(typeof(AudioSource))]

public class SkidEnabler : MonoBehaviour
{
    private const string SKID_MARK = "SkidMark";
    [SerializeField] private CarData mCarConfig;
    [SerializeField] private WheelCollider mWheelCollider;
    [SerializeField] private AudioSource mAudioSource;
  
 
    private GameObject mSkidObject;
    private TrailRenderer mTrailRenderer;

    private void LateUpdate()
    {
        mWheelCollider.GetGroundHit(out WheelHit hit);

        //verify that the wheel is skidding 
        if (Mathf.Abs(hit.forwardSlip) > mCarConfig.SlipThreshold ||
            Mathf.Abs(hit.sidewaysSlip) > mCarConfig.SlipThreshold)
        {
            //check for existing object
            if (mSkidObject == null)
            {
                // retrieve object from the pool (only pooled objects)
                mSkidObject = PoolManager.Instance.GetObjectOfType(SKID_MARK, false);
                // move it into position
                mSkidObject.transform.position = hit.point + (mCarConfig.PlacementOffset * Vector3.up);
                mSkidObject.transform.SetParent(mWheelCollider.transform);
                // attach it to the collider
                mTrailRenderer = mSkidObject.GetComponent<TrailRenderer>();
                // retrieve a reference to the trail renderer
                mTrailRenderer.Clear();
                mAudioSource.Play();
            }
            // continually update the skid mark position to the point of contact 
            mSkidObject.transform.localPosition = transform.InverseTransformPoint(hit.point) +
                (mCarConfig.PlacementOffset * Vector3.up);
        }
        else 
        {
            // verify the object hasnt been return
            if (mSkidObject)
            { 
                // pool object
                PoolManager.Instance.PoolObject(mSkidObject, true);
                mSkidObject = null;
                mTrailRenderer=null;
                mAudioSource.Stop();
                
            }
        }
    }
}
