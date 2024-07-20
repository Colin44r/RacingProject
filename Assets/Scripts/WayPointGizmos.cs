using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WayPointGizmos : MonoBehaviour
{
    private const string WAYPOINT = "Waypoint";

    [SerializeField, Range(1, 10)] private int mFlattenHeight = 51;

    [SerializeField] private float mRadius = 1.0f;
    [SerializeField] private Color mColor = Color.yellow;
    private Transform[] mWaypoints;

   public int  FlattenHeight => mFlattenHeight;

    private void OnDrawGizmos()
    {
        mWaypoints = GetComponentsInChildren<Transform>();
        var lastWaypoint = mWaypoints[mWaypoints.Length - 1].position;

        for (int i = 1; i < mWaypoints.Length; i++)
        { 
            Gizmos.color = mColor;
            Gizmos.DrawSphere(mWaypoints[i].position, mRadius);
            Gizmos.DrawLine(lastWaypoint, mWaypoints[i].position);
            lastWaypoint = mWaypoints[i].position;

            mWaypoints[i].name = WAYPOINT + " " + i.ToString();
        
        }

    }



}
