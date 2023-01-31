using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : ActorWithNeeds
{
    public Collider AttackCollider;

    public void AttackOn() => AttackCollider?.gameObject.SetActive(true);
    public void AttackOff() => AttackCollider?.gameObject.SetActive(false);



}
