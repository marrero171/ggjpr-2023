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

public sealed class HungerLevel : ContextualScorerBase
{
    [ApexSerialization, FriendlyName("Comparable Value")] public float refVal = 0;
    [ApexSerialization, FriendlyName("Use Satiation instead")] public bool sated = true;
    [ApexSerialization, FriendlyName("Use Raw Value instead")] public bool raw = true;
    public override float Score(IAIContext context)
    {
        NeedyActorContext ctx = (NeedyActorContext)context;
        if (sated && ctx.baseParent.BasicNeeds.Hunger >= refVal) return raw ? ctx.baseParent.BasicNeeds.Hunger * .1f : score;
        if (!sated && ctx.baseParent.BasicNeeds.Hunger <= refVal) return raw ? -ctx.baseParent.BasicNeeds.Hunger * .1f : score;
        return 0;
    }
}

public sealed class ThirstLevel : ContextualScorerBase
{
    [ApexSerialization, FriendlyName("Comparable Value")] public float refVal = 0;
    [ApexSerialization, FriendlyName("Use Satiation instead")] public bool sated = true;
    [ApexSerialization, FriendlyName("Use Raw Value instead")] public bool raw = true;
    public override float Score(IAIContext context)
    {
        NeedyActorContext ctx = (NeedyActorContext)context;
        if (sated && ctx.baseParent.BasicNeeds.Thirst >= refVal) return raw ? ctx.baseParent.BasicNeeds.Thirst * .1f : score;
        if (!sated && ctx.baseParent.BasicNeeds.Thirst <= refVal) return raw ? -ctx.baseParent.BasicNeeds.Thirst * .1f : score;
        return 0;
    }
}

public sealed class EmotionLevel : ContextualScorerBase
{
    [ApexSerialization, FriendlyName("Comparable Value")] public float refVal = 0;
    [ApexSerialization, FriendlyName("Use Positive Vibes Only")] public bool positive = true;
    [ApexSerialization, FriendlyName("Use Raw Value instead")] public bool raw = true;
    public override float Score(IAIContext context)
    {
        NeedyActorContext ctx = (NeedyActorContext)context;
        if (positive && ctx.baseParent.BasicNeeds.Emotion >= refVal) return raw ? ctx.baseParent.BasicNeeds.Emotion * .1f : score;
        if (!positive && ctx.baseParent.BasicNeeds.Emotion <= refVal) return raw ? -ctx.baseParent.BasicNeeds.Emotion * .1f : score;
        return 0;
    }
}

public sealed class KindRudeRatio : ContextualScorerBase
{
    [ApexSerialization, FriendlyName("Comparable Value")] public float refVal = 0;
    [ApexSerialization, FriendlyName("Use Positive Vibes Only")] public bool positive = true;
    [ApexSerialization, FriendlyName("Use Raw Value instead")] public bool raw = true;
    public override float Score(IAIContext context)
    {
        VillagerContext ctx = (VillagerContext)context;
        if (positive && ctx.instance.BehaviorVector.x >= 0) return raw ? ctx.instance.BehaviorVector.x : score;
        if (!positive && ctx.instance.BehaviorVector.x <= 0) return raw ? -ctx.instance.BehaviorVector.x : score;
        return 0;
    }
}

public sealed class RushRatio : ContextualScorerBase
{
    [ApexSerialization, FriendlyName("Comparable Value")] public float refVal = 0;
    [ApexSerialization, FriendlyName("Use Patience")] public bool positive = true;
    [ApexSerialization, FriendlyName("Use Raw Value instead")] public bool raw = true;
    public override float Score(IAIContext context)
    {
        VillagerContext ctx = (VillagerContext)context;
        if (positive && ctx.instance.BehaviorVector.y >= 0) return raw ? ctx.instance.BehaviorVector.y : score;
        if (!positive && ctx.instance.BehaviorVector.y <= 0) return raw ? -ctx.instance.BehaviorVector.y : score;
        return 0;
    }
}

public sealed class EagerRatio : ContextualScorerBase
{
    [ApexSerialization, FriendlyName("Comparable Value")] public float refVal = 0;
    [ApexSerialization, FriendlyName("Use Laziness")] public bool positive = true;
    [ApexSerialization, FriendlyName("Use Raw Value instead")] public bool raw = true;
    public override float Score(IAIContext context)
    {
        VillagerContext ctx = (VillagerContext)context;
        if (!positive && ctx.instance.BehaviorVector.z >= 0) return raw ? ctx.instance.BehaviorVector.z : score;
        if (positive && ctx.instance.BehaviorVector.z <= 0) return raw ? -ctx.instance.BehaviorVector.z : score;
        return 0;
    }
}

// public sealed class HasItemOfTypeNearBy: ContextualScorerBase{
//     [ApexSerialization, FriendlyName("")]
// }