using UnityEngine;

[CreateAssetMenu(fileName = "CameraConfig", menuName = "ScriptableObjects/CameraConfig", order = 1)]
public class CameraData : ScriptableObject
{
    [SerializeField] private float mDistance = 0;
    [SerializeField] private float mHeight = 0;
    [SerializeField] private float mRotationDamping = 0;
    [SerializeField] private float mHeightDamping = 0;


    public float Distance => mDistance;
    public float Height => mHeight;
    public float RotationDamping => mRotationDamping;
    public float HeightDamping => mHeightDamping;   



}
