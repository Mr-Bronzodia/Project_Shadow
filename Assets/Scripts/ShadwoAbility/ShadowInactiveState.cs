using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class ShadowInactiveState : ShadowState
{
    private float _timeSinceLastCancel = 0;
    private float _cooldownForAllowSpawning = 2f;

    public ShadowInactiveState(ShadowAbility context, ShadowFactory factory) : base(context, factory)
    {
    }

    protected override void CheckSwitchStates()
    {
        if (_ctx.Inputs.ability && _timeSinceLastCancel > _cooldownForAllowSpawning)
        {
            SwitchState(_factory.Spawning());
        }
        
    }

    public override void EnterState()
    {
    }

    protected override void ExitState()
    {
    }

    protected override void InitializeSubState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();

        if (_ctx.Inputs.cancel)
        {
            _timeSinceLastCancel = 0;
        }
        else
        {
            _timeSinceLastCancel += Time.deltaTime;
        }
    }
}
