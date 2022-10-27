public abstract class ShadowState 
{
    protected ShadowAbility _ctx;
    protected ShadowFactory _factory;

    public ShadowState(ShadowAbility context, ShadowFactory factory)
    {
        _ctx = context;
        _factory = factory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    protected abstract void ExitState();
    protected abstract void CheckSwitchStates();
    protected abstract void InitializeSubState();

    protected void UpdateStates()
    {

    }

    protected void SwitchState(ShadowState newState)
    {
        ExitState();

        newState.EnterState();

        _ctx.CurrentState = newState;
    }

    protected void SetSuperState()
    {

    }

    protected void SetSubState()
    {

    }
}
