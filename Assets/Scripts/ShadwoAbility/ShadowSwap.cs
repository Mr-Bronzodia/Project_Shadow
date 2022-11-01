using UnityEngine;


public class ShadowSwap : ShadowState
{
    private Vector3 _shadowPos;
    private bool _canSwap = false;

    public ShadowSwap(ShadowAbility context, ShadowFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        _shadowPos = _ctx.ShadowInstance.transform.position;
        Vector3 playerWithHeadOffset = _ctx.FirstPersonController.transform.position;
        playerWithHeadOffset.y += 1.7f;


        Vector3 shadowWithHeadOffset = _shadowPos;
        shadowWithHeadOffset.y += 1.7f;

        RaycastHit hit;
        if (!Physics.Raycast(playerWithHeadOffset, shadowWithHeadOffset - playerWithHeadOffset, out hit, Vector3.Distance(playerWithHeadOffset, shadowWithHeadOffset), _ctx.AbilityLayerMask))
        {
            _canSwap = true;
            _ctx.FirstPersonController.LockControls = true;
            _ctx.ShadowInstance.transform.position = _ctx.FirstPersonController.transform.position;
        }
        else
        {
            Debug.Log(hit.collider.name);
        }
         
    }

    protected override void CheckSwitchStates()
    {
        if (Vector3.Distance(_ctx.FirstPersonController.transform.position, _shadowPos) < 0.1f)
        { 
            SwitchState(_factory.Active());  
        }

        if (!_canSwap)
        {
            SwitchState(_factory.Active());
        }
    }

    protected override void ExitState()
    {
        _ctx.FirstPersonController.LockControls = false;
        _canSwap = false;
        _shadowPos = Vector3.zero;
    }

    private void SwapPlace()
    {
        if (Vector3.Distance(_ctx.FirstPersonController.transform.position, _shadowPos) > 0.1f && _canSwap) 
        {
            Vector3 moveDir = _shadowPos - _ctx.FirstPersonController.transform.position;
            _ctx.MovePlayer(moveDir * (Time.deltaTime * 3.5f));
        }
        else
        {
            Debug.Log("cant swap");
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
