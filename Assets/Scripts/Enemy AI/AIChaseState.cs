using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        GameObject[] AwareOfTargetArray = _ctx.AwareForTargets.Keys.ToArray();
        for (int i = 0; i < _ctx.AwareForTargets.Count; i++)
        {
            ResourceManager targetResourceManager;
            if (AwareOfTargetArray[i].TryGetComponent<ResourceManager>(out targetResourceManager))
            {
                if (targetResourceManager.CurrentHealth <= 0)
                {
                    SwitchState(_factory.MoveToPost());
                }
            }
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
