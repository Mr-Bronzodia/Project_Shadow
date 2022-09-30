using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGuardPostState : AIState
{
    private GuardShedue _currentShedue;
    private float _currentGuardTime;

    public AIGuardPostState(AIAgent agent, AIStateFactory factory) : base(agent, factory) { }

    public override void CheckSwichState()
    {
        if (_currentGuardTime > _currentShedue.WaitTime[_currentShedue.CurrentStop])
        {
            SwitchState(_factory.MoveToPost());
        }
    }

    public override void EnterState()
    {
        Debug.Log("Entered Guard State");

        _currentShedue = _ctx.GuardShedue;
        _currentGuardTime = 0;
    }

    public override void ExitState()
    {
        
    }

    public override void InitializeSubState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        CheckSwichState();
        _currentGuardTime += Time.deltaTime;
    }
}
