using Apex.AI;
using Apex.Serialization;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Or is of sort of a dickward.
/// </summary>
public sealed class IsKindEnough : ContextualScorerBase
{

    [ApexSerialization, FriendlyName("Compare Value")] public float compareValue = .5f;
    [ApexSerialization, FriendlyName("Use Negative instead")] public bool useNegative = true;
    public override float Score(IAIContext context)
    {
        VillagerContext ctx = (VillagerContext)context;
        if (ctx.instance.BehaviorVector.x >= compareValue && !useNegative) return score;
        if (ctx.instance.BehaviorVector.x <= compareValue && useNegative) return score;
        return 0;
    }
}

public sealed class IsItSteadyEnough : ContextualScorerBase
{

    [ApexSerialization, FriendlyName("Compare Value")] public float compareValue = .5f;
    [ApexSerialization, FriendlyName("Use Negative instead")] public bool useNegative = true;
    public override float Score(IAIContext context)
    {
        VillagerContext ctx = (VillagerContext)context;
        if (ctx.instance.BehaviorVector.y >= compareValue && !useNegative) return score;
        if (ctx.instance.BehaviorVector.y <= compareValue && useNegative) return score;
        return 0;
    }
}

public sealed class IsEagerEnough : ContextualScorerBase
{

    [ApexSerialization, FriendlyName("Compare Value")] public float compareValue = .5f;
    [ApexSerialization, FriendlyName("Use Negative instead")] public bool useNegative = true;
    public override float Score(IAIContext context)
    {
        VillagerContext ctx = (VillagerContext)context;
        if (ctx.instance.BehaviorVector.z >= compareValue && !useNegative) return score;
        if (ctx.instance.BehaviorVector.z <= compareValue && useNegative) return score;
        return 0;
    }
}