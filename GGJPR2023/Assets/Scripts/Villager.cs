using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : ActorWithNeeds
{

    /// <summary>
    /// X: Rudeness v Kindness
    /// Y: Rushy v Patient/Steady
    /// Z: Laziness v Eagerness
    /// </summary>
    [Tooltip("Rude/Kind | Rush/Steady | Lazy/Eager")]
    public Vector3 BehaviorVector = Vector3.zero;




}
