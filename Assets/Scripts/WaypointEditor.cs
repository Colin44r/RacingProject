using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(WayPointGizmos))]
public class WaypointEditor : Editor
{
    private const string FLATTEN_WAYPOINTS = "Flatten Waypoints";
    private const float DISTANCE = 10000f;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Flatten Waypoints"))
        { 
            var waypointGizmos = target as WayPointGizmos;
            var gameObj = waypointGizmos.gameObject;
            var waypoints = gameObj.GetComponentsInChildren<Transform>();

            for (int i = 1; i < waypoints.Length; i++) 
            {
                Physics.Raycast(new Ray(waypoints[i].position + Vector3.up * DISTANCE,
                    Vector3.down * float.MaxValue), out RaycastHit hit);
                waypoints[i].position = hit.point + Vector3.up * waypointGizmos.FlattenHeight;

            
            }



    }
    }
}
