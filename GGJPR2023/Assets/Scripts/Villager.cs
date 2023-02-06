using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI;
using Apex.AI.Components;
using Unity.VisualScripting;

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
    // public new VillagerContext ctx;
    public VillagerRequest request = null;

    Coroutine reqExp;
    public new IAIContext GetContext(System.Guid id) { return ctx; }

    public LayerMask villagerMask;

    public new void OnEnable()
    {
        ctx = new NeedyActorContext(this, this);
        Age = 0;
        GameObject.Find("Home").TryGetComponent(out HomeArea homeArea);
        home = homeArea;
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
        var villagers = Physics.OverlapSphere(transform.position, 50, villagerMask);
        VillagerRequest nRequest = new VillagerRequest(this, type);
        print(gameObject.name + " is requesting help of type " + type);
        villagers.ToList().ForEach(v => v.GetComponent<Villager>().HearOutcry(nRequest));
    }
    public void HearOutcry(VillagerRequest req)
    {
        print("I have heard " + req.who.name);
        if (req.who == this || req.who == null) return;
        if (BehaviorVector.x < -50) return;
        if (reqExp != null) StopCoroutine(reqExp);
        request = req;
        referenceActor = req.who;
        reqExp = StartCoroutine(expireRequest());
    }
}
[System.Serializable]
public class VillagerRequest
{
    public VillagerRequest(Villager who, ItemType requestType)
    {
        this.who = who; this.requestType = requestType;
    }
    public Villager who;
    public ItemType requestType;
}
