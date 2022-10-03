using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AIVisionSense))]
public class NewBehaviourScript : Editor
{
    private void OnSceneGUI()
    {
        AIVisionSense aIVisionSense = (AIVisionSense)target;
        Handles.color = Color.blue;
        Handles.DrawWireArc(aIVisionSense.transform.position, Vector3.up, Vector3.forward, 360, aIVisionSense._viewRadius);

        Vector3 viewAngleA = aIVisionSense.DirFromAngle(-aIVisionSense._viewAngle / 2, false);
        Vector3 viewAngleB = aIVisionSense.DirFromAngle(aIVisionSense._viewAngle / 2, false);
        Handles.DrawLine(aIVisionSense.transform.position, aIVisionSense.transform.position + viewAngleA * aIVisionSense._viewRadius);
        Handles.DrawLine(aIVisionSense.transform.position, aIVisionSense.transform.position + viewAngleB * aIVisionSense._viewRadius);
        Vector3 viewAngleUp = aIVisionSense.DirFromAngleUp(aIVisionSense._viewAngle / 2, false);
        Handles.color = Color.magenta;
        Handles.DrawLine(aIVisionSense.transform.position, aIVisionSense.transform.position + viewAngleUp * aIVisionSense._viewRadius);


    }
}
