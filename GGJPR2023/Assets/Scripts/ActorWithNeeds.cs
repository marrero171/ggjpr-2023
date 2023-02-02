using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.AIHelpers;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Apex.AI.Components.UtilityAIComponent))]
public class ActorWithNeeds : Actor, Apex.AI.Components.IContextProvider
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

    public Apex.AI.IAIContext ctx;
    public Apex.AI.IAIContext GetContext(System.Guid id) { return ctx; }


    public void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        collider = GetComponent<Collider>();
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon" || other.tag == "Attack")
        {
            referenceActor = other.GetComponentInParent<Actor>();
            ApplyDamage(referenceActor.Damage);
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