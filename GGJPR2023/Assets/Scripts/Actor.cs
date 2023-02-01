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
    public Dictionary<int, int> Inventory;
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

    protected IInteractable FindClosestInteraction()
    {
        float distanceToClosest = Mathf.Infinity;
        Collider closest = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius);

        foreach (Collider nearby in colliders)
        {
            if (nearby.TryGetComponent(out IInteractable interactable)) 
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
            closest.TryGetComponent(out IInteractable interactable);
            return interactable;
        }
    }
}