using Apex.AI;
using Apex.Serialization;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

#region  Needs
public sealed class HungerLevel : ContextualScorerBase
{
    [ApexSerialization, FriendlyName("Comparable Value")] public float refVal = 0;
    [ApexSerialization, FriendlyName("Use Satiation instead")] public bool sated = true;
    [ApexSerialization, FriendlyName("Use Raw Value instead")] public bool raw = true;
    public override float Score(IAIContext context)
    {
        NeedyActorContext ctx = (NeedyActorContext)context;
        MonoBehaviour.print(ctx.baseParent.basicNeeds.Hunger);
        if (sated && ctx.baseParent.basicNeeds.Hunger >= refVal) return raw ? ctx.baseParent.basicNeeds.Hunger * .1f : score;
        if (!sated && ctx.baseParent.basicNeeds.Hunger <= refVal) return raw ? -ctx.baseParent.basicNeeds.Hunger * .1f : score;
        return 2;
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
        MonoBehaviour.print(ctx.baseParent.basicNeeds.Hunger);
        if (sated && ctx.baseParent.basicNeeds.Thirst >= refVal) return raw ? ctx.baseParent.basicNeeds.Thirst * .1f : score;
        if (!sated && ctx.baseParent.basicNeeds.Thirst <= refVal) return raw ? -ctx.baseParent.basicNeeds.Thirst * .1f : score;
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
        if (positive && ctx.baseParent.basicNeeds.Emotion >= refVal) return raw ? ctx.baseParent.basicNeeds.Emotion * .1f : score;
        if (!positive && ctx.baseParent.basicNeeds.Emotion <= refVal) return raw ? -ctx.baseParent.basicNeeds.Emotion * .1f : score;
        return 0;
    }
}
#endregion

#region Current Personality
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
#endregion

#region Items
public sealed class HasItemOfTypeNearBy : ContextualScorerBase
{
    [ApexSerialization, FriendlyName("Layer Mask")] public LayerMask mask;
    [ApexSerialization, FriendlyName("Max Distance")] public float radius;
    public override float Score(IAIContext context)
    {
        NeedyActorContext ctx = (NeedyActorContext)context;
        Collider[] hits = Physics.OverlapSphere(ctx.baseParent.transform.position, radius, mask);
        Transform curr = null;
        float lastDist = radius * 2, dist = lastDist;
        hits.ToList().ForEach(hit =>
        {
            dist = Vector3.Distance(ctx.baseParent.transform.position, hit.transform.position);
            if (dist < lastDist) { lastDist = dist; curr = hit.transform; }
        });
        if (curr != null)
        {
            ctx.baseParent.lastTarget = ctx.baseParent.target;
            ctx.baseParent.target = curr;
            return score;
        }
        return 0;
    }
}

public sealed class HasItemOfTypeInInventory : ContextualScorerBase
{
    [ApexSerialization, FriendlyName("Item Type")] public ItemType type;
    public override float Score(IAIContext context)
    {
        NeedyActorContext ctx = (NeedyActorContext)context;
        ItemInfo item = ctx.baseParent.Inventory.Where(it => it.Key.itemType == type).FirstOrDefault().Key;
        // ctx.baseParent.
        return item == null ? 0 : score;
    }

}

#endregion
#region Behaviour Enums

public sealed class IsBored : ContextualScorerBase
{
    [ApexSerialization, FriendlyName("Behaviour")] public Utils.AIHelpers.NormalBehaviour behaviour;
    public override float Score(IAIContext context)
    {
        NeedyActorContext ctx = (NeedyActorContext)context;
        return (ctx.baseParent.WhenBored == behaviour) ? score : 0;
    }
}

public sealed class WhenHungry : ContextualScorerBase
{
    [ApexSerialization, FriendlyName("Behaviour")] public Utils.AIHelpers.HungryBehaviour behaviour;
    public override float Score(IAIContext context)
    {
        NeedyActorContext ctx = (NeedyActorContext)context;
        return (ctx.baseParent.WhenHungry == behaviour) ? score : 0;
    }
}
public sealed class WhenThirsty : ContextualScorerBase
{
    [ApexSerialization, FriendlyName("Behaviour")] public Utils.AIHelpers.ThirstyBehaviour behaviour;
    public override float Score(IAIContext context)
    {
        NeedyActorContext ctx = (NeedyActorContext)context;
        return (ctx.baseParent.WhenThirsty == behaviour) ? score : 0;
    }
}
public sealed class HealthCrtical : ContextualScorerBase
{
    [ApexSerialization, FriendlyName("Behaviour")] public Utils.AIHelpers.LowHealthBehavior behaviour;
    public override float Score(IAIContext context)
    {
        NeedyActorContext ctx = (NeedyActorContext)context;
        return (ctx.baseParent.WhenHealthCritical == behaviour) ? score : 0;
    }
}
public sealed class SpottedAnOppenent : ContextualScorerBase
{
    [ApexSerialization, FriendlyName("Behaviour")] public Utils.AIHelpers.OpponentSpotted behaviour;
    public override float Score(IAIContext context)
    {
        NeedyActorContext ctx = (NeedyActorContext)context;
        return (ctx.baseParent.WhenSpotted == behaviour) ? score : 0;
    }
}
public sealed class CloseEnoughToOpponent : ContextualScorerBase
{
    [ApexSerialization, FriendlyName("Layer Mask")] public Utils.AIHelpers.OpponentCloseEnough behaviour;
    public override float Score(IAIContext context)
    {
        NeedyActorContext ctx = (NeedyActorContext)context;
        return (ctx.baseParent.WhenCloseEnough == behaviour) ? score : 0;
    }
}
public sealed class OpponentWeak : ContextualScorerBase
{
    [ApexSerialization, FriendlyName("Layer Mask")] public Utils.AIHelpers.OpponentLowHealth behaviour;
    public override float Score(IAIContext context)
    {
        NeedyActorContext ctx = (NeedyActorContext)context;
        return (ctx.baseParent.WhenEnemyLowHealth == behaviour) ? score : 0;
    }
}

#endregion

public sealed class HasTarget : ContextualScorerBase
{
    public override float Score(IAIContext context)
    {
        NeedyActorContext ctx = (NeedyActorContext)context;
        if (ctx.baseParent.target != null) return score;
        else { ctx.baseParent.target = ctx.baseParent.lastTarget; return 0; }
    }
}

public sealed class ActorHasAttacker : ContextualScorerBase
{
    public override float Score(IAIContext context)
    {
        NeedyActorContext ctx = (NeedyActorContext)context;
        return ctx.baseParent.referenceActor != null ? score : 0;
    }
}