using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpriteRenderer))]
public class DroppedItem : Interactable
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

    private void LateUpdate() => transform.LookAt(Camera.main.transform);
    void OnEnable() => StartCoroutine(SetDropDetails());
    IEnumerator SetDropDetails()
    {
        yield return new WaitUntil(() => item != null);
        renderer.sprite = item.itemSprite;
        switch (item.itemType)
        {
            case ItemType.Food: gameObject.layer = LayerMask.NameToLayer("Food"); break;
            case ItemType.Throwable: gameObject.layer = LayerMask.NameToLayer("Throwable"); break;
            case ItemType.Plantable: gameObject.layer = LayerMask.NameToLayer("Plants"); break;
            case ItemType.Water: gameObject.layer = LayerMask.NameToLayer("Water"); break;
            case ItemType.Resource: gameObject.layer = LayerMask.NameToLayer("Resources"); break;
        }
        rigidbody.isKinematic = false;
        collider.isTrigger = false;
        rigidbody.AddForce(new Vector3(Random.Range(-2, 2), 8, Random.Range(-2, 2)), ForceMode.VelocityChange);

    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            rigidbody.isKinematic = true;
            collider.isTrigger = true;
        }
    }

    public void GrabItem()
    {
        print("Giving an item");
        activeActor.AddItem(item);
        gameObject.SetActive(false);
    }
}
