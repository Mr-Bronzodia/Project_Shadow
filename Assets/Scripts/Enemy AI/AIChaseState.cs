using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChaseState : AIState
{
    public AIChaseState(AIAgent agent, AIStateFactory factory) : base(agent, factory) { }

    public override void CheckSwichState()
    {
        if (!_ctx.IsAwareOfTarget)
        {
            SwitchState(_factory.Investigate());
        }
    }

    public override void EnterState()
    {

    }

    public override void ExitState()
    {
        
    }

    public override void InitializeSubState()
    {
        
    }

    public override void UpdateState()
    {
        CheckSwichState();
        _ctx.NavMeshAgent.destination = _ctx.LastSeenTargetLocation.position;
    }
}
