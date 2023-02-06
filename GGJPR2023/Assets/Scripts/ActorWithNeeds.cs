using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.AIHelpers;
using Apex.AI;
using Apex.AI.Components;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(UtilityAIComponent))]
public class ActorWithNeeds : Actor, IContextProvider
{
    public float NormalSpeed = 4, RunSpeed = 8;
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

    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Collider collider;
    [Tooltip("Debugging of targets")]
    public Transform target;
    public Transform lastTarget;
    public HomeArea home;

    public NeedyActorContext ctx;
    public IAIContext GetContext(System.Guid id) { return ctx; }

    public void OnEnable()
    {
        ctx = new NeedyActorContext(this);
        // print(ctx);
    }

    public new void Start()
    {
        base.Start();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 50, NavMesh.AllAreas))
            navMeshAgent.Warp(hit.position);
        if (needDecreaseRate > 0) StartCoroutine(UpdateNeeds());
    }

    void LateUpdate()
    {
        moveDir = navMeshAgent.velocity.normalized;
        base.LateUpdate();
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
            if (basicNeeds.Hunger < -20 || basicNeeds.Thirst < -20) basicNeeds.Emotion -= needDecreaseRate;
            if (basicNeeds.Hunger > 50 || basicNeeds.Thirst > 80) basicNeeds.Emotion += needDecreaseRate;
            //Is this like, Consumtion Need Nirvana?
            if (basicNeeds.Hunger > 50 && basicNeeds.Thirst > 80 && basicNeeds.Emotion > 80 && Random.Range(0, 75) > 50) Health++;

        }
    }

    public override void Consume(ItemInfo item, bool useItem = true)
    {
        if (item == null) return;
        Health += (int)(item.effectiveAmount / 4);
        if (item.itemType == ItemType.Food) basicNeeds.Hunger += item.effectiveAmount;
        if (item.itemType == ItemType.Water) basicNeeds.Thirst += item.effectiveAmount;
        if (Random.Range(0, 10) > 4) Health++;
        if (item.externalReference != null) SpawnItem((ItemInfo)item.externalReference);
        if (useItem && Inventory.ContainsKey(item)) RemoveItem(item, 1);
    }

    public void ChangeSpeed(bool Running = false) => navMeshAgent.speed = Running ? RunSpeed : NormalSpeed;

    public void ChargeTowardsAttacker() => StartCoroutine(ChargeTowardsAttackerCoroutine());
    IEnumerator ChargeTowardsAttackerCoroutine()
    {
        if (AttackCollider != null && lastAttacker != null)
        {
            AttackOn();
            AttackCollider.transform.position = transform.position + (faceDir * 1.5f);
            navMeshAgent.speed = RunSpeed * 2;
            navMeshAgent.SetDestination(lastAttacker.transform.position);
            yield return new WaitUntil(() => !AttackCollider.gameObject.activeInHierarchy);
        }
        yield return null;
    }

    void Die()
    {
        if (home != null) home.RequestNewActor();
        base.Die();
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