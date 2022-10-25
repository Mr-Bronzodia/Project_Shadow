using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIAttackState : AIState
{
    public AIAttackState(AIAgent agent, AIStateFactory factory) : base(agent, factory) 
    {

    }

    public override void CheckSwichState()
    {
        if (_ctx.IsDead)
        {
            SwitchState(_factory.Dead());
        }

        if (_ctx.LastSeenTargetLocation != null)
        {
            if (Vector3.Distance(_ctx.transform.position, _ctx.LastSeenTargetLocation.position) > _ctx.NavMeshAgent.stoppingDistance)
            {
                SwitchState(_factory.Chase());
            }
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
        if(_ctx.LastSeenTargetLocation != null) 
        {
            _ctx.transform.LookAt(_ctx.LastSeenTargetLocation.position);
        }

        _ctx.OnCombatContinue?.Invoke();

        CheckSwichState();
    }
}
