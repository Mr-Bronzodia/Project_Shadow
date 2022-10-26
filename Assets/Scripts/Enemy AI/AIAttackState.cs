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
