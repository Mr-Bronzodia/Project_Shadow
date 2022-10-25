

using System.Collections.Generic;

public class AIStateFactory
{
    AIAgent _agent;
    Dictionary<string, AIState> _states = new Dictionary<string, AIState>();

    public AIStateFactory(AIAgent agent)
    {
        _agent = agent;

        _states["SubAttack"] = new AISubAttack(agent, this);
        _states["MoveToState"] = new AIMoveToPost(agent, this);
        _states["GuardPost"] = new AIGuardPostState(agent, this);
        _states["DeadState"] = new AIDeadState(agent, this);
        _states["ChaseState"] = new AIChaseState(agent, this);
        _states["AttackState"] = new AIAttackState(agent, this);
        _states["InvestigateState"] = new AIInvestigateArea(agent, this);
    }

    public AIState Investigate()
    {
        return _states["InvestigateState"];
    }

    public AIState MoveToPost()
    {
        return _states["MoveToState"];
    }

    public AIState GuardPost()
    {
        return _states["GuardPost"];
    }

    public AIState Dead()
    {
        return _states["DeadState"];
    }

    public AIState Chase()
    {
        return _states["ChaseState"];
    }

    public AIState Attack()
    {
        return _states["AttackState"];
    }

    public AIState SubAttack()
    {
        return _states["SubAttack"];
    }

}
