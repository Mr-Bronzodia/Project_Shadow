using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChaseState : AIState
{
    public AIChaseState(AIAgent agent, AIStateFactory factory) : base(agent, factory) { }

    public override void CheckSwichState()
    {
        if (_ctx.IsAware() == false)
        {
            SwitchState(_factory.Investigate());
        }
    }

    public override void EnterState()
    {

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
