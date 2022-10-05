using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AIHearingSense))]
public class AIHearingSenseEditor : Editor
{
    private void OnSceneGUI()
    {
        AIHearingSense aiHearing = (AIHearingSense)target;
        Handles.color = Color.red;

        Handles.DrawWireArc(aiHearing.transform.position, Vector3.up, Vector3.forward, 360, aiHearing._hearRadius);
    }
}
