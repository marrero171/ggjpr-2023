using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class ActorWithNeeds : Actor
{
    [Header("Needs")]
    public BaseNeeds BasicNeeds;

    [Tooltip("Ammount to reduce per seccond")]
    [Range(0, 1)]
    public float needDecreaseRate = 0.15f;

    [HideInInspector] public UnityEngine.AI.NavMeshAgent navMeshAgent;
    [HideInInspector] public Collider collider;

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