using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AIHearingSense : AISense
{
    [SerializeField] public float _hearRadius;

    protected override IEnumerator BeginSense()
    {
        while (true)
        {
            yield return new WaitForSeconds(_senseInterval);
            ListenForTargetsInRange();
        }
    }

    private void Start()
    {
        _agent.IsAwareOfTarget["Hearing"] = false;
    }

    private void OnDisable()
    {
        _agent.IsAwareOfTarget.Remove("Hearing");
    }

    // (1 - (dstToSource / _hearRadius)) - distance multiplier
    // velocity / 10 - velocity multiplier

    private void ListenForTargetsInRange()
    {
        Collider[] targetsInHearRange = Physics.OverlapSphere(transform.position, _hearRadius, TargetMask);

        for (int i = 0; i < targetsInHearRange.Length; i++)
        {
            if (targetsInHearRange[i] != _agent.GetComponent<Collider>()) 
            {
                Transform target = targetsInHearRange[i].transform;
                float dstToSource = Vector3.Distance(transform.position, target.position);

                if (target.tag == "Player") 
                {

                    float velocityMagnitude = target.GetComponent<CharacterController>().velocity.magnitude;
                    if (velocityMagnitude > 2.5)
                    {
                        float intensity = (velocityMagnitude / 10) * (1 - (dstToSource / _hearRadius));
                        _agent.UpdateAwarness(target.gameObject, intensity, true);
                        _agent.IsAwareOfTarget["Hearing"] = true;
                    }
                    else 
                    {
                        _agent.UpdateAwarness(target.gameObject, -0.08f, false);
                        _agent.IsAwareOfTarget["Hearing"] = false;
                    }
                    
                }
                
            }
        }
    }
}
