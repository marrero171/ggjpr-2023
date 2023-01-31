using UnityEngine;
using Apex.AI;
using Apex.Serialization;
public class NeedyActorContext : IAIContext
{
    public ActorWithNeeds baseParent;
    public NeedyActorContext(object ctx) => this.baseParent = (ActorWithNeeds)ctx;
}

public class VillagerContext : NeedyActorContext
{
    public Villager instance;
    public VillagerContext(object ctx) : base(ctx)
    {
        this.instance = (Villager)ctx;
        this.baseParent = (ActorWithNeeds)ctx;
    }
}