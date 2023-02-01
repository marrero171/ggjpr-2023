using System.Linq;
using UnityEngine;
using ExtEvents;
using System.Collections.Generic;
using System.Collections.Generic;
using System;

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
    public void RequestByActor(Actor actor, string name = "default")
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
        if (System.String.IsNullOrEmpty(name))
        {
            ExtEvent ev = events.FirstOrDefault().Value;
            ev.Invoke();
        }
        if (events.ContainsKey(name)) events[name].Invoke();

    }

    private object KeyValuePair<T1, T2>()
    {
        throw new NotImplementedException();
    }
}