using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI;
using Apex.Serialization;

public sealed class ActorGoToTarget : ActionBase
{
    public override void Execute(IAIContext context)
    {
        NeedyActorContext ctx = (NeedyActorContext)context;
        ctx.baseParent.navMeshAgent.SetDestination(ctx.baseParent.target.position);
    }
}

public sealed class ActorGetItem : ActionBase
{
    public override void Execute(IAIContext context)
    {
        NeedyActorContext ctx = (NeedyActorContext)context;
        ctx.baseParent.selectedItem = null;
        ctx.baseParent.TryInteract();
    }
}

public sealed class ActorInteractOrUse : ActionBase
{
    public override void Execute(IAIContext context)
    {
        NeedyActorContext ctx = (NeedyActorContext)context;
        ctx.baseParent.TryInteract();
    }
}

public sealed class ActorGiveItem : ActionBase
{
    [ApexSerialization, FriendlyName("Instant use?")] public bool instaUse;

    public override void Execute(IAIContext context)
    {
        NeedyActorContext ctx = (NeedyActorContext)context;
        ctx.baseParent.GiveSelectedItem(instaUse);
        ((ActorWithNeeds)ctx.baseParent.referenceActor).basicNeeds.Emotion += (Random.Range(10, 20));
        ((Villager)ctx.baseParent.referenceActor).BehaviorVector.x += (Random.Range(1, 15));
    }
}


