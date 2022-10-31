using UnityEngine;


public class ShadowSwap : ShadowState
{
    private Vector3 _shadowPos;

    public ShadowSwap(ShadowAbility context, ShadowFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        _shadowPos = _ctx.ShadowInstance.transform.position;
        _ctx.FirstPersonController.LockControls = true;
        _ctx.ShadowInstance.transform.position = _ctx.FirstPersonController.transform.position;
    }

    protected override void CheckSwitchStates()
    {
        if (Vector3.Distance(_ctx.FirstPersonController.transform.position, _shadowPos) < 0.1f)
        { 
            SwitchState(_factory.Active());  
        }
    }

    protected override void ExitState()
    {
        _ctx.FirstPersonController.LockControls = false;
    }

    private void SwapPlace()
    {
        if (Vector3.Distance(_ctx.FirstPersonController.transform.position, _shadowPos) > 0.1f) 
        {
            Vector3 moveDir = _shadowPos - _ctx.FirstPersonController.transform.position;
            _ctx.MovePlayer(moveDir * (Time.deltaTime * 3f));
        }
    }

    public override void UpdateState()
    {
        SwapPlace();
        CheckSwitchStates();
    }

    protected override void InitializeSubState()
    {
        throw new System.NotImplementedException();
    }
}
