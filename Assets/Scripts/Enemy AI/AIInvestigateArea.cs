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
        if (_ctx.LastSeenTargetLocation != null)
        {
            SwitchState(_factory.Chase());
        }


        if(_ctx.SuspeciousLocation == null) 
        {
            SwitchState(_factory.MoveToPost());
        }
    }

    private Vector3 SampleRandomNavPosition(float radius)
    {
        Vector3 randomPointInRadius = _ctx.SuspeciousLocation.position + Random.insideUnitSphere * radius;
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
        for (int i = 0; i <= 2; i++)
        {
            _investigationsPoint[i] = SampleRandomNavPosition(_investigationAreaRadius);
        }

        _ctx.NavMeshAgent.destination = _investigationsPoint[0];
        _investigationsCount++;

    }


    public override void ExitState()
    {
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
                    _ctx.NavMeshAgent.destination = _investigationsPoint[_investigationsCount++];
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
