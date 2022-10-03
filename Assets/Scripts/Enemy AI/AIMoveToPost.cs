using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveToPost : AIState
{
    private GuardShedue _currentShedue;

    public AIMoveToPost(AIAgent agent, AIStateFactory factory) : base(agent, factory) { }

    public override void CheckSwichState()
    {
        if (_ctx.NavMeshAgent.remainingDistance < _ctx.NavMeshAgent.stoppingDistance + 1)
        {
            SwitchState(_factory.GuardPost());
        }
    }

    public override void EnterState()
    {
        _currentShedue = _ctx.GuardShedue ;

        _ctx.NavMeshAgent.destination = _currentShedue.Targets[_currentShedue.CurrentStop].position;
    }

    public override void ExitState()
    {
        _ctx.GuardShedue.GoToNextStop();
    }

    public override void InitializeSubState()
    {

    }

    public override void UpdateState()
    {
        CheckSwichState();
    }
}
