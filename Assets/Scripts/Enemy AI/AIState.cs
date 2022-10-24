
public abstract class AIState 
{
    protected AIAgent _ctx;
    protected AIStateFactory _factory;
    protected bool _isRootState = false;
    private AIState _currentSuperState;
    private AIState _currentSubState;

    public AIState(AIAgent agent, AIStateFactory factory) 
    {
        _ctx = agent;
        _factory = factory;
        _isRootState = true;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwichState();
    public abstract void InitializeSubState();

    public void UpdateStates() 
    { 
        UpdateState();

        if(_currentSubState != null) 
        {
            _currentSubState.UpdateState();
        }
    }

    protected void SwitchState(AIState newState) 
    {
        ExitState();

        newState.EnterState();

        if ( _isRootState ) 
        {
            _ctx.CurrentAIState = newState;
        }
        else if ( _currentSuperState != null ) 
        {
            _currentSubState.SetSubState(newState);
        }
    }
    protected void SetSuperState(AIState newSuperState) 
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(AIState newSubState) 
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);    
    }
}
