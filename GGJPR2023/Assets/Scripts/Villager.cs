using System.Linq;
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
    public int Age = 0, maxAge = 50;
    public new VillagerContext ctx;
    public VillagerRequest request = null;

    Coroutine reqExp;
    public new IAIContext GetContext(System.Guid id) { return ctx; }

    public new void OnEnable()
    {
        ctx = new VillagerContext(this);
        Age = 0;
        StartCoroutine(AgingCoroutine());
    }

    IEnumerator AgingCoroutine()
    {
        yield return new WaitForSeconds(10);
        Age++;
        if (Age >= maxAge) Die();
    }
    IEnumerator expireRequest()
    {
        yield return new WaitForSeconds(30 + (BehaviorVector.x * .10f));
        request = null;
    }

    // Took a massive shortcut.
    public void RequestHelp(ItemType type)
    {
        var villagers = Physics.OverlapSphere(transform.position, 50, LayerMask.NameToLayer("Villagers"));
        VillagerRequest nRequest = new VillagerRequest(this, type);
        villagers.ToList().ForEach(v => v.SendMessage("HearOutcry", nRequest));
    }
    public void HearOutcry(VillagerRequest req)
    {
        if (BehaviorVector.x < -50) return;
        StopCoroutine(reqExp);
        request = req;
        referenceActor = req.who;
        reqExp = StartCoroutine(expireRequest());
    }


}
public class VillagerRequest
{
    public VillagerRequest(Villager who, ItemType requestType)
    {
        this.who = who; this.requestType = requestType;
    }
    public Villager who;
    public ItemType requestType;
}
