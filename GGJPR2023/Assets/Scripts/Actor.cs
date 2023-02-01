using System.Linq;
using UnityEngine;
using ExtEvents;
using System.Collections.Generic;

public class Actor : MonoBehaviour, IDamageable
{
    public int Health { set; get; } = 10;
    public int MaxHealth = 10, Damage = 1;
    public float interactionRadius = 3;
    public Collider AttackCollider;
    //TODO: Inventory
    // public Dictionary<ItemInfo, int> Inventory;
    public GenericDictionary<ItemInfo, int> Inventory;
    public Interactable activeIntractable = null;

    private void OnEnable()
    {
        Health = MaxHealth;
    }

    public void ApplyDamage(int dmg)
    {
        Health -= dmg;
        if (Health <= 0) gameObject.SetActive(false);
    }

    public void AttackOn() => AttackCollider?.gameObject.SetActive(true);
    public void AttackOff() => AttackCollider?.gameObject.SetActive(false);
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HitCollider") print("Mira canto'e");
        if (other.tag == "Soil") activeIntractable = other.GetComponent<Interactable>();
        if (other.name.StartsWith("DroppedItem")) activeIntractable = other.GetComponent<Interactable>();
    }
    */

    protected Interactable FindClosestInteraction()
    {
        float distanceToClosest = Mathf.Infinity;
        Collider closest = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius);

        foreach (Collider nearby in colliders)
        {
            if (nearby.TryGetComponent(out Interactable interactable))
            {
                float distanceToObject = (nearby.transform.position - gameObject.transform.position).sqrMagnitude;
                if (distanceToObject < distanceToClosest)
                {
                    distanceToClosest = distanceToObject;
                    closest = nearby;
                }
            }
        }

        if (closest == null)
        {
            return null;
        }
        else
        {
            closest.TryGetComponent(out Interactable interactable);
            return interactable;
        }
    }
    /*
    public void TryInteract(string ev = "default")
    {
        if (activeIntractable != null) activeIntractable.RequestByActor(ev, this);
    }
    */
    public void AddItem(ItemInfo item, int ammount = 1)
    {
        if (Inventory.ContainsKey(item)) Inventory[item] += ammount;
        else Inventory.Add(item, ammount);
    }

    public bool DropItem(ItemInfo item, int ammount = 1)
    {
        if (Inventory.ContainsKey(item))
        {
            if (Inventory[item] >= ammount)
            {
                //TODO: Request DroppedItem from PoolManager
                Inventory[item] -= ammount;
                if (Inventory[item] <= 0) Inventory.Remove(item);
                return true;
            }
            return false;
        }
        return false;
    }
}