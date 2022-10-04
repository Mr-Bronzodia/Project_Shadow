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
    private Dictionary<GameObject, float> _awareOfTargets = new Dictionary<GameObject, float>();
    public Vector3 EyeOffset;
    [SerializeField] private AnimationCurve _alertnessAtPoint;
    public TMP_Text DebugView;


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
                    BuildAwarness(target.gameObject, angleToTarget, dstToTarget);
                }

            }

            ClearAwarnes();
        }
    }

    private void BuildAwarness(GameObject target, float seenAtAngle, float distance)
    {
        float sneakMultiplier = 0;
        
        if(target.tag == "Player")
        {
            sneakMultiplier = target.GetComponentInChildren<FirstPersonController>().SneakMultiplier;
        }


        if (!_awareOfTargets.ContainsKey(target)) 
        {
            _awareOfTargets[target] =  Mathf.Max((_alertnessAtPoint.Evaluate(seenAtAngle) * (1 - (distance / _viewRadius))) - sneakMultiplier, 0);
        }
        else
        {
            _awareOfTargets[target] += Mathf.Max((_alertnessAtPoint.Evaluate(seenAtAngle) * (1 - (distance / _viewRadius))) - sneakMultiplier, 0);
        }

    }

    private void ClearAwarnes()
    {
        foreach (GameObject target in _awareOfTargets.Keys.ToList())
        {
            if (!_seenThisFrame.Contains(target))
            {
                _awareOfTargets[target] -= 0.08f;
            }

            if (_awareOfTargets[target] > 1.5)
            {
                _agent.AddSuspeciousLocaton(target.transform);
            }

            if (_awareOfTargets[target] > 2.5 & _seenThisFrame.Contains(target))
            {
                _agent.IsAwareOfTarget = true;
                _agent.UpdateLastSeen(target.transform);
            }
            else
            {
                _agent.IsAwareOfTarget = false;
            }

            if (_awareOfTargets[target] < 0)
            {
                _awareOfTargets.Remove(target);
            }

        }

        
    }


    private void Update()
    {
        DebugView.text = "";

        DebugView.text += "Current AI State: " + _agent.GetCurrentState() + "\n";
        DebugView.text += "Is AI seeing Target: " + _agent.IsAwareOfTarget + "\n";
        foreach (GameObject target in _awareOfTargets.Keys.ToList())
        {
            DebugView.color = new Color(0.5f, 1f, 0f, 1f);
            DebugView.text += target + ": " + _awareOfTargets[target] + " From Vision";
            
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
