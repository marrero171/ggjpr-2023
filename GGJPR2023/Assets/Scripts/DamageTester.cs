using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTester : Interactable
{
    public int damage = 2;
    public void InflictDamage()
    {
        activeActor.ApplyDamage(damage);
    }
}
