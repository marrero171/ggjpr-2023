using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.AIHelpers;
using Apex.AI;
using Apex.AI.Components;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(UtilityAIComponent))]
public class ActorWithNeeds : Actor, IContextProvider
{
    [Header("Needs")]
    public BaseNeeds basicNeeds;

    [Tooltip("Ammount to reduce per seccond")]
    [Range(0, 1)]
    public float needDecreaseRate = 0.15f;

    [Space(5)]
    [Header("Behaviours")]
    public NormalBehaviour WhenBored;
    public HungryBehaviour WhenHungry;
    public ThirstyBehaviour WhenThirsty;
    public LowHealthBehavior WhenHealthCritical;
    [Header("In Relation to In Regards to Its Enemies")]
    public OpponentSpotted WhenSpotted;
    public OpponentCloseEnough WhenCloseEnough;
    public OpponentLowHealth WhenEnemyLowHealth;

    [HideInInspector] public UnityEngine.AI.NavMeshAgent navMeshAgent;
    [HideInInspector] public Collider collider;
    [HideInInspector] public Transform target;
    [HideInInspector] public Transform lastTarget;
    [HideInInspector] public HomeArea home;

    public NeedyActorContext ctx;
    public IAIContext GetContext(System.Guid id) { return ctx; }

    public void OnEnable()
    {
        ctx = new NeedyActorContext(this);
        print(ctx);
    }

    public new void Start()
    {
        base.Start();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (needDecreaseRate > 0) StartCoroutine(UpdateNeeds());
    }

    /// <summary>
    /// Should be used at start up
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateNeeds()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            basicNeeds.Hunger -= needDecreaseRate;
            basicNeeds.Thirst -= needDecreaseRate;
            // Emotion-=needDecreaseRate;
        }
    }


    public override void Consume(ItemInfo item, bool useItem = true)
    {
        if (item == null) return;
        Health += (int)(item.effectiveAmount / 4);
        if (item.itemType == ItemType.Food) basicNeeds.Hunger += item.effectiveAmount;
        if (item.itemType == ItemType.Water) basicNeeds.Thirst += item.effectiveAmount;
        if (useItem && Inventory.ContainsKey(item)) RemoveItem(item, 1);
    }

}

[System.Serializable]
public class BaseNeeds
{
    [Tooltip("Starving ~ Sated ~ Overstuffed")]
    [Range(-100, 100)]
    public float Hunger = 0;

    [Tooltip("Dehydrated ~ Hydrated ~ Drank too much")]
    [Range(-100, 100)]
    public float Thirst = 0;

    [Tooltip("Angry/Frustrated, Sad, Happy, Fuffiled")]
    [Range(-100, 100)]
    public float Emotion = 0;
}