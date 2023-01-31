using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpriteRenderer))]
public class DroppedItem : TriggerEvent
{
    SpriteRenderer renderer;
    Rigidbody rigidbody;
    Collider collider;
    //TODO: Reference ScriptableObject
    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    void OnEnable()
    {
        // renderer.sprite=Item.img;
        rigidbody.isKinematic = false;
        collider.isTrigger = false;
        rigidbody.AddForce(new Vector3(Random.Range(-2, 2), 8, Random.Range(-1, -5)), ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision other)
    {
        rigidbody.isKinematic = true;
        collider.isTrigger = true;
    }

    //TODO: Grab
}
