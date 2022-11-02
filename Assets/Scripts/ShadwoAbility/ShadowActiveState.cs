using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowActiveState : ShadowState
{
    public ShadowActiveState(ShadowAbility context, ShadowFactory factory) : base(context, factory)
    {

    }

    protected override void CheckSwitchStates()
    {
        if (_ctx.Inputs.cancel)
        {
            _ctx.DestroyShadow();
            SwitchState(_factory.Inactive());  
        }

        if (_ctx.PlayerReourceManager.CurrentMana <= 1 ) 
        {
            _ctx.DestroyShadow();
            SwitchState(_factory.Inactive());
        }

        if (_ctx.Inputs.ability && !_ctx.FirstPersonController.LockControls)
        {
            SwitchState(_factory.Swap());
        } 

        if(_ctx.ShadowInstance == null) 
        {
            _ctx.DestroyShadow();
            SwitchState(_factory.Inactive());
        }
    }

    public override void EnterState()
    {
        _ctx.PlayerReourceManager.ApplyManaLose(10f);
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
        _ctx.PlayerReourceManager.ApplyManaLose(20f * Time.deltaTime);
    }
}
