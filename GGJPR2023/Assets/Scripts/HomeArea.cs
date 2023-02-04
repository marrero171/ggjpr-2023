using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class HomeArea : MonoBehaviour
{
    public Collider bounds;
    public Actor spawnableActor;
    public int maxCount = 20;
    // public int StartingCount = 10;
    public bool isVillage = false;
    [HideInInspector] public Vector3 min, max;
    public List<Actor> actors;
    void Start()
    {
        bounds = GetComponent<Collider>();
        actors = new List<Actor>();
        min = new Vector3(-bounds.bounds.extents.x, -bounds.bounds.extents.y, -bounds.bounds.extents.z);
        max = new Vector3(bounds.bounds.extents.x, bounds.bounds.extents.y, bounds.bounds.extents.z);
        for (int i = 0; i < Random.Range(1, maxCount / 4); i++) RequestNewActor();
    }

    public void RequestNewActor()
    {
        if (actors.Count >= maxCount) return;
        var actor = actors.Find(a => !a.gameObject.activeInHierarchy);
        if (actor == null)
        {
            actor = Instantiate(spawnableActor);
        }
        Vector3 pos = transform.position;
        Vector3 boundary = bounds.bounds.extents;
        actor.transform.position = GetPointWithintBounds();
        // actor.transform.position = new Vector3(pos.x + Random.Range(-bounds.x, bounds.x),
        //                                      pos.y + 3,
        //                                      pos.z + Random.Range(-bounds.z, bounds.z));
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

}
