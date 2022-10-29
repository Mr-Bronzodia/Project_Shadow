using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AIInvestigateArea : AIState
{
    private Vector3[] _investigationsPoint = new Vector3[4];
    private int _investigationsCount = 0;
    private float _timeInvestigatingPoint = 0;
    private float _investigationAreaRadius = 5f;

    public AIInvestigateArea(AIAgent agent, AIStateFactory factory) : base(agent, factory)
    {
    }


    public override void CheckSwichState()
    {
        if (_ctx.IsDead)
        {
            SwitchState(_factory.Dead());
        }

        foreach (AwareTarget target in _ctx.AwareForTargets) 
        {
            if (target.IsAware()) 
            {
                Debug.Log("soy Investigate to chase");
                SwitchState(_factory.Chase());
            }
        }


        if(_ctx.CurrentTarget == null) 
        {
            SwitchState(_factory.MoveToPost());
        }
    }

    private Vector3 SampleRandomNavPosition(float radius)
    {
        Vector3 randomPointInRadius = _ctx.CurrentTarget.Target.transform.position + Random.insideUnitSphere * radius;
        Vector3 finalPoint = Vector3.zero;

        NavMeshHit navHit;

        if (NavMesh.SamplePosition(randomPointInRadius, out navHit, radius, 1))
        {
            finalPoint = navHit.position;
        }

        return finalPoint;
    }

    public override void EnterState()
    {
        _ctx.ChangeAlertState(AlertState.Suspecious);

        for (int i = 0; i <= 2; i++)
        {
            _investigationsPoint[i] = SampleRandomNavPosition(_investigationAreaRadius);
        }

        _ctx.NavMeshAgent.destination = _investigationsPoint[0];
        _investigationsCount++;

    }


    public override void ExitState()
    {
        _ctx.ChangeAlertState(AlertState.Unalerted);
        _investigationsPoint = new Vector3[4];
        _investigationsCount = 0;
        _timeInvestigatingPoint = 0;
    }

    public override void InitializeSubState()
    {

    }

    public override void UpdateState()
    {
        CheckSwichState();


        if (_ctx.NavMeshAgent.remainingDistance < _ctx.NavMeshAgent.stoppingDistance + 1)
        {
            _timeInvestigatingPoint += Time.deltaTime;

            if (!(_investigationsCount == _investigationsPoint.Length))
            {
                if (_timeInvestigatingPoint > 5f)
                {
                    if (_investigationsPoint.Length >= _investigationsCount + 1)
                    {
                        _ctx.NavMeshAgent.destination = _investigationsPoint[_investigationsCount++];
                    }
                    else
                    {
                        SampleRandomNavPosition(_investigationAreaRadius);
                    }

                    _timeInvestigatingPoint = 0;
                }
            }
            else
            {
                _ctx.AddSuspeciousLocaton(null); 
            }
        }


    }
}
