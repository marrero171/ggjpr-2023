using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI;
using Apex.AI.Components;

public class Villager : ActorWithNeeds
{

    /// <summary>
    /// X: Rudeness v Kindness
    /// Y: Rushy v Patient/Steady
    /// Z: Laziness v Eagerness
    /// </summary>
    [Tooltip("Rude/Kind | Rush/Steady | Lazy/Eager")]
    public Vector3 BehaviorVector = Vector3.zero;

    public new VillagerContext ctx;

    public new IAIContext GetContext(System.Guid id) { return ctx; }

    public new void OnEnable()
    {
        ctx = new VillagerContext(this);
    }


}
