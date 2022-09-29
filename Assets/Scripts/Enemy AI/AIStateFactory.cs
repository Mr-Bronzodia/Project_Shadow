

public class AIStateFactory
{
    AIAgent _agent;

    public AIStateFactory(AIAgent agent)
    {
        _agent = agent;
    }

    public AIState MoveToPost()
    {
        return new AIMoveToPost(_agent, this);
    }

    public AIState GuardPost()
    {
        return new AIGuardPostState(_agent, this);
    }

    public AIState Dead()
    {
        return new AIDeadState(_agent, this);
    }

    public AIState Chase()
    {
        return new AIChaseState(_agent, this);
    }

    public AIState Attack()
    {
        return new AIAttackState(_agent, this);
    }
}
