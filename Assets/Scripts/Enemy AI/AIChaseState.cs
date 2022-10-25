using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChaseState : AIState
{
    public AIChaseState(AIAgent agent, AIStateFactory factory) : base(agent, factory) { }

    public override void CheckSwichState()
    {
        if (_ctx.IsDead)
        {
            SwitchState(_factory.Dead());
        }

        if (_ctx.NavMeshAgent.remainingDistance < _ctx.NavMeshAgent.stoppingDistance && _ctx.IsAware())
        {
            SwitchState(_factory.Attack());  
        }


        if (_ctx.IsAware() == false)
        {
            SwitchState(_factory.Investigate());
        }
    }

    public override void EnterState()
    {
        _ctx.ChangeAlertState(AlertState.Alerted);
    }

    public override void ExitState()
    {
        _ctx.UpdateLastSeen(null);
    }

    public override void InitializeSubState()
    {
        
    }

    public override void UpdateState()
    {
        _ctx.NavMeshAgent.destination = _ctx.LastSeenTargetLocation.position;
        CheckSwichState();
    }
}
