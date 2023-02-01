using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void RequestByActor(string name, Actor actor);
}
