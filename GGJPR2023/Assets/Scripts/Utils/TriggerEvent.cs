using System.Linq;
using UnityEngine;
using ExtEvents;
using System.Collections.Generic;

[System.Serializable]
public class NamedEvent
{
    public string name;
    public ExtEvent Event;
}

[RequireComponent(typeof(Collider))]
public class TriggerEvent : MonoBehaviour
{
    public List<NamedEvent> events;
    Actor activeActor;

    /// <summary>
    /// An Actor makes a request!
    /// </summary>
    /// <param name="name">event name</param>
    /// <param name="actor">actor in question, usually "this"</param>
    public void RequestByActor(string name, Actor actor)
    {
        activeActor = actor;
        requestEvent(name);
    }

    /// <summary>
    /// Will look for an event and fire it up
    /// </summary>
    /// <param name="name">name of event</param>
    public void requestEvent(string name)
    {
        if (activeActor != null)
        {
            try { events.Find(ev => ev.name == name).Event.Invoke(); }
            catch { Debug.LogError("Couldnt find event:" + name); }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Not Great.
        if (other.tag == "Player" || other.tag == "Villager") activeActor = other.GetComponent<Actor>();
    }

    private void OnTriggerExit(Collider other)
    {
        //Still not great.
        if (other.tag == "Player" || other.tag == "Villager") activeActor = null;
    }
}