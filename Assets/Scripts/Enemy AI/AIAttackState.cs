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

        if (_ctx.AwareForTargets.Count <= 0)
        {
            SwitchState(_factory.MoveToPost());
        }


        for (int i = 0; i < _ctx.AwareForTargets.Count; i++) 
        {
            ResourceManager targetResourceManager;
            if (_ctx.AwareForTargets[i].Target.TryGetComponent<ResourceManager>(out targetResourceManager))
            {
                if (targetResourceManager.CurrentHealth <= 0)
                {
                    return;
                }

                if (Vector3.Distance(_ctx.AwareForTargets[i].Target.transform.position, _ctx.transform.position) > _ctx.NavMeshAgent.stoppingDistance)
                {
                    SwitchState(_factory.Chase());
                    Debug.Log("soy");
                }
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
        if(_ctx.CurrentTarget != null) 
        {
            _ctx.transform.LookAt(_ctx.CurrentTarget.Target.transform.position);
        }

        _ctx.OnCombatContinue?.Invoke();

        CheckSwichState();
    }
}
