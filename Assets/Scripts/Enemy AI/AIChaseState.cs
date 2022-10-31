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

        for (int i = 0; i < _ctx.AwareForTargets.Count; i++)
        {
            ResourceManager targetResourceManager;
            if (_ctx.AwareForTargets[i].Target.TryGetComponent<ResourceManager>(out targetResourceManager))
            {
                if (targetResourceManager.CurrentHealth <= 0)
                {
                    _ctx.AwareForTargets.RemoveAt(i);
                    return;
                }
            }

            //if (_ctx.NavMeshAgent.remainingDistance < _ctx.NavMeshAgent.stoppingDistance && _ctx.AwareForTargets[i].IsAware())
            if (Vector3.Distance(_ctx.AwareForTargets[i].Target.transform.position, _ctx.transform.position) < _ctx.NavMeshAgent.stoppingDistance && _ctx.AwareForTargets[i].IsAware())
            {
                SwitchState(_factory.Attack());
            }
        } 


        if (_ctx.CurrentTarget == null)
        {
            SwitchState(_factory.MoveToPost());
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

    public void FindSuitableTartget()
    {
        foreach (AwareTarget target in _ctx.AwareForTargets) 
        {
            if (target != null) 
            {
                if (target.IsAware())
                {
                    _ctx.NavMeshAgent.destination = target.Target.transform.position;
                }
            }   
        }
    }

    public override void InitializeSubState()
    {
        
    }

    public override void UpdateState()
    {
        FindSuitableTartget();
        CheckSwichState();
    }
}
