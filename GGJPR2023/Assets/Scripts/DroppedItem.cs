using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpriteRenderer))]
public class DroppedItem : TriggerEvent, IInteractable
{
    public ItemInfo item;
    SpriteRenderer renderer;
    Rigidbody rigidbody;
    Collider collider;
    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    void OnEnable()
    {
        renderer.sprite = item.itemSprite;
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
    public void Interact()
    {
        gameObject.SetActive(false);
    }

    public void Illuminate()
    {

    }
    public void GrabItem(Actor actor)
    {
        actor.AddItem(item);
        gameObject.SetActive(false);
    }
}
