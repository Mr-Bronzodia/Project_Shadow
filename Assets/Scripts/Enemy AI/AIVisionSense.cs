using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIVisionSense : AISense
{
    public float _viewRadius;
    [Range(0, 360)]
    public float _viewAngle;
    private List<Transform> _transformList = new List<Transform>();


    public Vector3 DirFromAngle(float angleInDegrees, bool isGlobal)
    {
        if (!isGlobal) angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad)); 
    }

    public Vector3 DirFromAngleUp(float angleInDegrees, bool isGlobal) {
        if (!isGlobal) angleInDegrees += transform.eulerAngles.y;
        Vector3 dir = new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        dir += new Vector3(0, Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);

        return dir;
    }

    private void FindTargetsInVew()
    {
        _transformList.Clear();
        //Get all possible targest in view range
        Collider[] targetsInVewRange = Physics.OverlapSphere(transform.position, _viewRadius, TargetMask);

        for (int i = 0; i < targetsInVewRange.Length; i++) 
        {
            Transform target = targetsInVewRange[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            //Check if target is in view frostom
            if (Vector3.Angle(transform.forward, dirToTarget) < _viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                //Check if target is obstructed
                if (!Physics.Raycast(transform.position + new Vector3(0, 1f , 0), dirToTarget, dstToTarget, ObsticleLayer))
                {
                    _transformList.Add(target);
                }

            }
        }
    }

    private void Update()
    {
        foreach(Transform t in _transformList) 
        {
            Debug.DrawLine(transform.position, t.position);
        }
    }

    protected override IEnumerator BeginSense()
    {
        while (true) 
        {
            yield return new WaitForSeconds(_senseInterval);
            FindTargetsInVew();
        }
    }
}
