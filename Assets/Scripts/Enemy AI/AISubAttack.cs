using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISubAttack : AIState
{
    public AISubAttack(AIAgent agent, AIStateFactory factory) : base(agent, factory)
    {
        _isRootState = false;
    }

    public override void CheckSwichState()
    {
        
    }

    public override void EnterState()
    {
        Debug.Log("Entered Sub Attack");
    }

    public override void ExitState()
    {
        
    }

    public override void InitializeSubState()
    {
        
    }

    public override void UpdateState()
    {
        Debug.Log("soy");
    }
}
