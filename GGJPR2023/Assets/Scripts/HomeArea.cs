using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BoxCollider))]
public class HomeArea : MonoBehaviour
{
    public Collider bounds;
    public Actor spawnableActor;
    public bool spawnAtStart = false;
    public int maxCount = 20;
    // public int StartingCount = 10;
    public bool isVillage = false;
    [HideInInspector] public Vector3 min, max;
    public List<ActorWithNeeds> actors;
    public List<ItemInfo> spwanables;
    IEnumerator Start()
    {
        yield return new WaitUntil(() => ReferenceMaster.instance != null);
        bounds = GetComponent<Collider>();
        actors = new List<ActorWithNeeds>();
        min = new Vector3(-bounds.bounds.extents.x, -bounds.bounds.extents.y, -bounds.bounds.extents.z);
        max = new Vector3(bounds.bounds.extents.x, bounds.bounds.extents.y, bounds.bounds.extents.z);
        if (spawnAtStart) for (int i = 0; i < (isVillage ? maxCount / 4 : (Random.Range(1, (maxCount / maxCount / 2)))); i++) RequestNewActor();
        if (spawnAtStart) for (int i = 0; i < maxCount; i++)
            {
                DroppedItem newItem = Utils.PoolingSystem.instance.GetObject(ReferenceMaster.instance.DroppedItem.gameObject).GetComponent<DroppedItem>();
                newItem.transform.position = transform.position + new Vector3(Random.Range(-1, 1), 1, Random.Range(-1, 1));
                newItem.item = spwanables[Random.Range(0, spwanables.Count)];
                newItem.gameObject.SetActive(true);
            }
    }

    public void RequestNewActor()
    {
        if (actors.Count >= maxCount) return;
        var actor = actors.Find(a => !a.gameObject.activeInHierarchy);
        if (actor == null)
        {
            actor = (ActorWithNeeds)Instantiate(spawnableActor);
        }
        Vector3 pos = transform.position;
        Vector3 boundary = bounds.bounds.extents;
        Vector3 refVec = transform.position + Random.onUnitSphere * 50;
        refVec.y = transform.position.y;
        // NavMeshHit hit;
        // if (NavMesh.SamplePosition(refVec, out hit, 10000, NavMesh.AllAreas))
        // actor.navMeshAgent.Warp(hit.position);
        // transform.position = hit.position;
        actor.transform.position = refVec;
        // actor.transform.position = GetPointWithintBounds();
        // actor.transform.position = new Vector3(pos.x + Random.Range(-bounds.x, bounds.x),
        //                                      pos.y + 3,
        //                                      pos.z + Random.Range(-bounds.z, bounds.z));

        actor.basicNeeds.Hunger = Random.Range(-20, 20);
        actor.basicNeeds.Thirst = Random.Range(-20, 20);
        actor.basicNeeds.Emotion = Random.Range(-20, 20);
        if (isVillage)
        {
            var villager = (Villager)actor;
            villager.BehaviorVector = Random.insideUnitSphere * 50;

        }
    }

    public Vector3 GetPointWithintBounds()
    {
        Vector3 pos = transform.position;
        return new Vector3(pos.x + Random.Range(min.x, max.x),
                            pos.y + 3,
                            pos.z + Random.Range(min.z, max.z));
    }

    public Vector3 GetOverallCulture()
    {
        Vector3 BehaviourVector = Vector3.zero;
        if (!isVillage) return BehaviourVector;
        actors.ForEach(actor =>
        {
            BehaviourVector += ((Villager)actor).BehaviorVector;
        });
        return BehaviourVector;
    }

    public Vector3 GetVillagerBehaviourAverage()
    {
        if (actors.Count == 0) return Vector3.zero;
        Vector3 bVector = Vector3.zero;
        actors.ForEach(actor => { bVector += ((Villager)actor).BehaviorVector; });
        return (bVector / actors.Count);
    }
    public Vector3 GetActorNeedsAverage()
    {
        if (actors.Count == 0) return Vector3.zero;
        Vector3 nVector = Vector3.zero;
        actors.ForEach(actor =>
        {
            Vector3 needs = new Vector3(actor.basicNeeds.Hunger, actor.basicNeeds.Thirst, actor.basicNeeds.Emotion);
            nVector += Vector3.zero;
        });
        return (nVector / actors.Count);
    }


}
