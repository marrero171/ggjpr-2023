using UnityEngine;
using Apex.AI;
using Apex.Serialization;
public class NeedyActorContext : IAIContext
{
    public ActorWithNeeds baseParent;
    public Villager villager;

    public NeedyActorContext(ActorWithNeeds ctx, Villager villager = null) { this.baseParent = ctx; this.villager = villager; }
}