
using System.Collections.Generic;

public class ShadowFactory 
{
    ShadowAbility _context;
    Dictionary<string, ShadowState> _states = new Dictionary<string, ShadowState>();

    public ShadowFactory(ShadowAbility context)
    {
        _context = context;
        _states["Spawning"] = new ShadowSpawningState(context, this);
        _states["Active"] = new ShadowActiveState(context, this);
        _states["Inactive"] = new ShadowInactiveState(context, this);
        _states["Swap"] = new ShadowSwap(context, this);
    }

    public ShadowState Spawning()
    {
        return _states["Spawning"];
    }

    public ShadowState Active() 
    {
        return _states["Active"];
    }

    public ShadowState Inactive()
    {
        return _states["Inactive"];
    }

    public ShadowState Swap()
    {
        return _states["Swap"];
    }
}
