using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GuardPostDebug : MonoBehaviour
{
    public float Radius;


    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, Radius);
#endif
    }
}
