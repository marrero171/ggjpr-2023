using System.Linq;
using UnityEngine;
using ExtEvents;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    public GenericDictionary<string, ExtEvent> events;
    [HideInInspector] public Actor activeActor;

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
        print("Requesting " + name);
        if (events.ContainsKey(name)) events[name].Invoke();
    }
}