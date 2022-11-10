using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeadState : AIState
{
    public AIDeadState(AIAgent agent, AIStateFactory factory) : base(agent, factory) { }

    public override void CheckSwichState()
    {
        
    }

    public override void EnterState()
    {
        _ctx.NavMeshAgent.isStopped = true;
    }

    public override void ExitState()
    {
        
    }

    public override void InitializeSubState()
    {
        
    }

    public override void UpdateState()
    {
        
    }
}
