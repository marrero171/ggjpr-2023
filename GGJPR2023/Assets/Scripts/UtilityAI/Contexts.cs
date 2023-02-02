using UnityEngine;
using Apex.AI;
using Apex.Serialization;
public class NeedyActorContext : IAIContext
{
    public ActorWithNeeds baseParent;
    public NeedyActorContext(ActorWithNeeds ctx) => this.baseParent = ctx;
}

public class VillagerContext : NeedyActorContext
{
    public Villager instance;
    public VillagerContext(Villager ctx) : base(ctx)
    {
        this.instance = (Villager)ctx;
        this.baseParent = (ActorWithNeeds)ctx;
    }
}