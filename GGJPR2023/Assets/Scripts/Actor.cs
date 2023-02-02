using System.Linq;
using UnityEngine;
using ExtEvents;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animator))]
public abstract class Actor : MonoBehaviour, IDamageable
{
    public ExtEvent<bool> onIsDead;
    public ExtEvent<int> OnHealthUpdated;

    public int MaxHealth = 10, Damage = 1;
    public float interactionRadius = 3;
    public Collider AttackCollider;
    //TODO: Inventory
    // public Dictionary<ItemInfo, int> Inventory;
    public GenericDictionary<ItemInfo, int> Inventory;
    public Interactable activeIntractable = null;
    public ItemInfo selectedItem = null;
    int selectedItemIndex = 0;
    [HideInInspector] public Actor referenceActor, lastAttacker;
    [HideInInspector] public Vector3 moveDir = Vector3.zero;
    [HideInInspector] public SpriteRenderer renderer;
    [HideInInspector] public Animator animator;
    Coroutine lastAttackerCooldown;

    public bool isDead = false;
    public bool isPlayer = false;
    [SerializeField] private int health = 0;
    public int Health
    {
        get { return health; }
        set
        {
            int oldHealth = health;
            health = Mathf.Clamp(value, 0, MaxHealth);
            if (health != oldHealth)
            {
                OnHealthUpdated?.Invoke(health);
            }
            if (health <= 0)
            {
                Die();
                onIsDead?.Invoke(isPlayer);
            }
        }
    }

    public void Start()
    {
        Inventory = new GenericDictionary<ItemInfo, int>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }
    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        
        if (Mathf.Abs(moveDir.x) >= 0.1f)
            renderer.flipX = moveDir.x > 0;
        // animator.SetFloat("DirX", moveDir.x);
        // animator.SetFloat("DirZ", moveDir.z);
    }


    private void OnEnable()
    {
        Health = MaxHealth;
    }

    public void ApplyDamage(int dmg)
    {
        Health -= dmg;
    }

    public void Die()
    {
        isDead = true;
        gameObject.SetActive(false);
    }

    public void AttackOn() => AttackCollider?.gameObject.SetActive(true);
    public void AttackOff() => AttackCollider?.gameObject.SetActive(false);

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HitCollider")
        {
            print("I poop on your mother and I poop on you.");
            try //If its an animal, like a Lechon.
            {
                Actor parent = other.transform.parent.GetComponent<Actor>();
                ApplyDamage(parent.Damage);
                lastAttackerCooldown = StartCoroutine(SetLastAttacker(parent));
            }
            catch
            {
                print("Ese Pobre Lechon 2, lessgo");
                AttackProjectile projectile = other.GetComponent<AttackProjectile>();
                ApplyDamage(projectile.DamageAmmount);
                lastAttackerCooldown = StartCoroutine(SetLastAttacker(projectile.attacker));
            }
        }
        if (other.tag == "Soil") activeIntractable = other.GetComponent<Interactable>();
        if (other.name.StartsWith("DroppedItem")) activeIntractable = other.GetComponent<Interactable>();
    }

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

    public void TryInteract()
    {
        //if (activeIntractable != null) activeIntractable.RequestByActor(ev, this);
        if (activeIntractable)
        {
            if (activeIntractable.GetComponent<DroppedItem>())
            {
                activeIntractable.RequestByActor(this, "Grab");
                return;
            }
            switch (selectedItem.itemType)
            {
                //Plant tree if is not planted
                case ItemType.Plantable:
                    activeIntractable.RequestByActor(this, "Plant"); 
                    break;

                case ItemType.Food: Consume(selectedItem, true); break;

                case ItemType.Water:
                    if (activeIntractable?.tag == "Soil") print("Water plant");
                    else Consume(selectedItem, true);
                    break;

                case ItemType.Resource:
                    break;
                case ItemType.Throwable: //Attack
                    AttackProjectile projectile = Utils.PoolingSystem.instance.GetObject(ReferenceMaster.instance.Projectile.gameObject).GetComponent<AttackProjectile>();
                    projectile.refSprite = selectedItem.itemSprite;
                    projectile.direction = transform.forward;
                    projectile.DamageAmmount = selectedItem.effectiveAmount;
                    projectile.Speed = 8;
                    projectile.gameObject.SetActive(true);
                    break;

                default:
                    //activeIntractable.RequestByActor(this);

                    break;
            }
        }
    }
    public void AddItem(ItemInfo item, int ammount = 1)
    {
        if (Inventory.ContainsKey(item)) Inventory[item] += ammount;
        else Inventory.Add(item, ammount);
    }

    // Uses up item (without dropping)
    public void UseItem(ItemInfo item, int amount = 1)
    {
        RemoveItem(item, amount);
    }

    public void DropItem(ItemInfo item, int amount = 1)
    {
        RemoveItem(item, amount);
        //TODO: Request DroppedItem from PoolManager
    }

    public bool RemoveItem(ItemInfo item, int ammount = 1)
    {
        if (Inventory.ContainsKey(item))
        {
            if (Inventory[item] >= ammount)
            {
                Inventory[item] -= ammount;
                if (Inventory[item] <= 0)
                {
                    if (selectedItem == item) selectedItem = null;
                    Inventory.Remove(item);
                }
                return true;
            }
            return false;
        }
        return false;
    }

    public void ScrollSelectItem(int byAmmount, bool specify = false)
    {
        if (!specify) selectedItemIndex += byAmmount;
        else selectedItemIndex = byAmmount;
        if (selectedItemIndex > 10 || selectedItemIndex > Inventory.Count - 1) selectedItemIndex = 0;
        if (selectedItemIndex < 0) selectedItemIndex = Inventory.Count - 1;
        // print(selectedItemIndex);
        //Lazy Check, rework later?
        if (Inventory.Count > 0 && Inventory.Count >= selectedItemIndex) selectedItem = Inventory.ElementAt(selectedItemIndex).Key;
        else selectedItem = null;
    }

    IEnumerator SetLastAttacker(Actor attacker)
    {
        lastAttacker = attacker;
        yield return new WaitForSeconds(30);
        lastAttacker = null;
    }

    public abstract void Consume(ItemInfo item, bool useItem = false);
    public void GiveSelectedItem(bool instaUse)
    {
        if (referenceActor == null || selectedItem == null) return;
        if (!instaUse) referenceActor.AddItem(selectedItem);
        if (instaUse) referenceActor.Consume(selectedItem, false);
        RemoveItem(selectedItem);
        /// --- THIS IS MOVED TO ACTIONS! This is here for testing outside AI
        //Warning, Free-Fall errors <3
        //Cyclic Dependencies? What's that??
        // ((ActorWithNeeds)lastAttacker).basicNeeds.Emotion += (Random.Range(10, 20));
        // ((Villager)lastAttacker).BehaviorVector.x += Random.Range(1, 15);
    }
}