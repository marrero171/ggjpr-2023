using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class HomeArea : MonoBehaviour
{
    Collider collider;
    public Actor spawnableActor;
    public int maxCount;
    public bool isVillage = false;
    public List<Actor> actors;
    void Start()
    {
        collider = GetComponent<Collider>();
        actors = new List<Actor>();
        for (int i = 0; i < Random.Range(1, maxCount / 2); i++) RequestNewActor();
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
        Vector3 bounds = collider.bounds.extents;
        actor.transform.position = new Vector3(pos.x + Random.Range(-bounds.x, bounds.x),
                                             pos.y + 3,
                                             pos.z + Random.Range(-bounds.z, bounds.z));
        if (isVillage)
        {
            var villager = (Villager)actor;
            villager.BehaviorVector = Random.insideUnitSphere * 50;

        }
    }




}
