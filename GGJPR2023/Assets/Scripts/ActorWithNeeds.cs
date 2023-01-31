using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.AIHelpers;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class ActorWithNeeds : Actor
{
    [Header("Needs")]
    public BaseNeeds BasicNeeds;

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
    [HideInInspector] public Actor lastAttacker;
    [HideInInspector] public Transform target;

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
            BasicNeeds.Hunger -= needDecreaseRate;
            BasicNeeds.Thirst -= needDecreaseRate;
            // Emotion-=needDecreaseRate;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon" || other.tag == "Attack")
        {
            lastAttacker = other.GetComponentInParent<Actor>();
            ApplyDamage(lastAttacker.Damage);
        }
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