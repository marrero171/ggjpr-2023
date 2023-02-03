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
        ctx.baseParent.ScrollSelectItem(-10, true);
        MonoBehaviour.print("Attempting Grab");
        ctx.baseParent.
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

public sealed class ActorWalkAround : ActionBase
{
    [ApexSerialization, FriendlyName("Minimm Distance if no Home?")] public float minDist = 5;
    [ApexSerialization, FriendlyName("Maximum Distance if no home?")] public float maxDist = 5;

    public override void Execute(IAIContext context)
    {
        NeedyActorContext ctx = (NeedyActorContext)context;
        if (Random.Range(0, 10) > 5 && ctx.baseParent.home != null) ctx.baseParent.navMeshAgent.SetDestination(ctx.baseParent.home.GetPointWithintBounds());
        else if (Random.Range(0, 5) < 8) ctx.baseParent.navMeshAgent.SetDestination(ctx.baseParent.transform.position + Random.onUnitSphere * Random.Range(8, 30));
    }
}

public sealed class ActorChangeSpeed : ActionBase
{
    [ApexSerialization, FriendlyName("Running?")] public bool Run;

    public override void Execute(IAIContext context)
    {
        NeedyActorContext ctx = (NeedyActorContext)context;
        ctx.baseParent.ChangeSpeed(Run);
    }
}

public sealed class ActorBargeAttack : ActionBase
{
    public override void Execute(IAIContext context)
    {
        NeedyActorContext ctx = (NeedyActorContext)context;
        ctx.baseParent.ChargeTowardsTarget();
    }
}