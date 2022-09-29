
public abstract class AIState 
{
    protected AIAgent _ctx;
    protected AIStateFactory _factory;

    public AIState(AIAgent agent, AIStateFactory factory) 
    {
        _ctx = agent;
        _factory = factory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwichState();
    public abstract void InitializeSubState();

    protected void UpdateStates() { }
    protected void SwitchState(AIState newState) 
    {
        ExitState();

        newState.EnterState();

        _ctx.CurrentAIState = newState;
    }
    protected void SetSuperState() { }
    protected void SetSubState() { }
}
