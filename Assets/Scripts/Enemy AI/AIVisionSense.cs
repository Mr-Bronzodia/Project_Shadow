using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using StarterAssets;

public class AIVisionSense : AISense
{
    public float _viewRadius;
    [Range(0, 360)]
    public float _viewAngle;
    private List<GameObject> _seenThisFrame = new List<GameObject>();
    public Vector3 EyeOffset;


    [SerializeField] private AnimationCurve _alertnessAtPoint;


    public Vector3 DirFromAngle(float angleInDegrees, bool isGlobal)
    {
        if (!isGlobal) angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad)); 
    }

    public Vector3 DirFromAngleUp(float angleInDegrees, bool isGlobal) {
        Quaternion rot = Quaternion.AngleAxis(angleInDegrees, Vector3.right);
        Vector3 dir = rot * Vector3.forward;

        return transform.TransformDirection(dir);
    }

    private void FindTargetsInVew()
    {
        _seenThisFrame.Clear();
        //Get all possible targest in view range
        Collider[] targetsInVewRange = Physics.OverlapSphere(transform.position, _viewRadius, TargetMask);

        for (int i = 0; i < targetsInVewRange.Length; i++) 
        {
            Transform target = targetsInVewRange[i].transform;
            Vector3 dirToTarget = (targetsInVewRange[i].bounds.center - new Vector3(0, 1.2f, 0) - transform.position).normalized;

            //Check if target is in view frostom
            float angleToTarget = Vector3.Angle(transform.forward, dirToTarget);

            if (angleToTarget < _viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                // Check if target isnt ocludec 
                if (!Physics.Raycast(transform.position + EyeOffset, dirToTarget, dstToTarget, ObsticleLayer))
                {
                    _seenThisFrame.Add(target.gameObject);
                    BuildAwarnessOnSeen(target.gameObject, angleToTarget, dstToTarget);
                }
            }

            if (_agent.AwareForTargets.Keys.ToList().Contains(target.gameObject) && !_seenThisFrame.Contains(target.gameObject))
            {
                _agent.UpdateAwarness(target.gameObject, -0.08f, false);
            }

        }
    }

    private void BuildAwarnessOnSeen(GameObject target, float seenAtAngle, float distance)
    {
        float sneakMultiplier = 0;
        
        if(target.tag == "Player")
        {
            sneakMultiplier = target.GetComponentInChildren<FirstPersonController>().SneakMultiplier;
        }
        else
        {
            Debug.Log("Target didnt have FPS Controller");
        }

        float awarnessIncrease = Mathf.Max((_alertnessAtPoint.Evaluate(seenAtAngle) * (1 - (distance / _viewRadius))) - sneakMultiplier, 0);

        _agent.UpdateAwarness(target, awarnessIncrease, true);
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
