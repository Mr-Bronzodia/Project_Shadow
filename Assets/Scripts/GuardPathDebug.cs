using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPathDebug : MonoBehaviour
{
    private void OnDrawGizmos()
    {

        Vector3 startPosition = transform.GetComponentsInChildren<Transform>()[0].position;
        Vector3 previousPosition = startPosition;
        foreach (Transform pathPoint in transform.GetComponentsInChildren<Transform>())
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pathPoint.position, 0.3f);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(previousPosition, pathPoint.position);
            previousPosition = pathPoint.position;  
        }

        Gizmos.DrawLine(previousPosition, startPosition);
    }
}
