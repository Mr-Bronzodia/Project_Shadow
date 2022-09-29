using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeadState : AIState
{
    public AIDeadState(AIAgent agent, AIStateFactory factory) : base(agent, factory) { }

    public override void CheckSwichState()
    {
        throw new System.NotImplementedException();
    }

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void InitializeSubState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }
}
