using System.Linq;
using UnityEngine;
using ExtEvents;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animator))]
public abstract class Actor : MonoBehaviour
{

    public int Health = 10, MaxHealth = 10, Damage = 1;
    public float interactionRadius = 3;
    public Collider AttackCollider;
    //TODO: Inventory
    // public Dictionary<ItemInfo, int> Inventory;
    public GenericDictionary<ItemInfo, int> Inventory;
    public Interactable activeIntractable = null;
    public ItemInfo selectedItem = null;
    int selectedItemIndex = 0;
    [HideInInspector] public Actor referenceActor, lastAttacker;
    [HideInInspector] public Vector3 moveDir = Vector3.zero, faceDir = Vector3.zero;
    [HideInInspector] public SpriteRenderer renderer;
    [HideInInspector] public Animator animator;
    Coroutine lastAttackerCooldown;

    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool isPlayer = false;

    public ItemInfo defaultDrop;
    public int maxDrops = 1;

    public void Start()
    {
        if (Inventory == null) Inventory = new GenericDictionary<ItemInfo, int>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }
    public void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);

        if (Mathf.Abs(moveDir.x) >= 0.1f)
            renderer.flipX = moveDir.x > 0;
        if (moveDir.magnitude > .9f) faceDir = moveDir;
        // animator.SetFloat("DirX", moveDir.x);
        // animator.SetFloat("DirZ", moveDir.z);
    }


    private void OnEnable()
    {
        Health = MaxHealth;
    }

    int changeHealth
    {
        set
        {
            Health = Mathf.Clamp(value, 0, MaxHealth);
            Debug.Log(gameObject.name + " health changed");
            if (isPlayer) HUDAndMenu.instance.UpdateHealth();
            if (Health == 0) { Die(); }
        }
        get { return Health; }
    }
    public void ApplyDamage(int dmg) => changeHealth -= dmg;
    public void Heal(int amount)
    {
        changeHealth += amount;
        // Do pretty things or smth
    }

    public virtual void Die()
    {
        isDead = true;
        DropInventory();
        if (defaultDrop) SpawnItem(defaultDrop, Random.Range(0, maxDrops));
        if (!isPlayer) gameObject.SetActive(false);
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
        if (AttackCollider != null) AttackOff();
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


    //Refactored
    public void TryInteract()
    {
        if (selectedItem != null)
        {
            switch (selectedItem.itemType)
            {
                case ItemType.Food:
                    if (activeIntractable != null) activeIntractable.RequestByActor(this); //Whatever this is.
                    else Consume(selectedItem, true);
                    break;
                case ItemType.Water:
                    if (activeIntractable != null)
                        if (activeIntractable.tag == "Soil") activeIntractable.RequestByActor(this, "Water plant");
                        else activeIntractable.RequestByActor(this);
                    else Consume(selectedItem, true);
                    break;
                case ItemType.Plantable:
                    if (activeIntractable != null)
                        if (activeIntractable.tag == "Soil") activeIntractable.RequestByActor(this, "Plant");
                        else activeIntractable.RequestByActor(this); //Whatever this is.
                    break;
                case ItemType.Throwable: //Combat System Much?
                    ThrowProjectile(); //Isolated to Function as per Marrero's suggesiton.
                    break;
                case ItemType.Resource: //What do?
                default: //Ignore everything and just grab.
                    if (activeIntractable != null) activeIntractable.RequestByActor(this);
                    break;
            }
        }
        if (selectedItem == null && activeIntractable != null) activeIntractable.RequestByActor(this);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + (faceDir * 1.5f), 1);
    }

    public void ThrowProjectile()
    {
        if (selectedItem == null) return;
        AttackProjectile projectile = Utils.PoolingSystem.instance.GetObject(ReferenceMaster.instance.Projectile.gameObject).GetComponent<AttackProjectile>();
        projectile.transform.position = transform.position + (faceDir * 1.5f);
        projectile.refSprite = selectedItem.itemSprite;
        projectile.direction = faceDir;
        projectile.DamageAmmount = selectedItem.effectiveAmount;
        projectile.Speed = 70; //TODO: Look for speed.
        projectile.gameObject.SetActive(true);

    }
    public void AddItem(ItemInfo item, int ammount = 1)
    {
        print("Adding " + item.itemName);
        if (Inventory.ContainsKey(item)) Inventory[item] += ammount;
        else
        {
            Inventory.Add(item, ammount);
            if (!selectedItem && Inventory.Count == 1) ScrollSelectItem(1);
        }
        if (isPlayer) HUDAndMenu.instance.UpdateIcon();
    }

    // Uses up item (without dropping)
    public void UseItem(ItemInfo item, int amount = 1) => RemoveItem(item, amount);

    public void DropItem(ItemInfo item, int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnItem(item);
            RemoveItem(item, 1);
            if (!Inventory.ContainsKey(item)) return;
        }
    }

    public void SpawnItem(ItemInfo item, int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            DroppedItem newItem = Utils.PoolingSystem.instance.GetObject(ReferenceMaster.instance.DroppedItem.gameObject).GetComponent<DroppedItem>();
            newItem.transform.position = transform.position + new Vector3(Random.Range(-3, 3), 4, Random.Range(-3, 3));
            newItem.item = item;
            newItem.gameObject.SetActive(true);
        }
    }

    public void DropInventory() { while (Inventory.Count > 0) { DropItem(Inventory.ElementAt(0).Key, Inventory.ElementAt(0).Value); } }

    public bool RemoveItem(ItemInfo item, int ammount = 1)
    {
        if (Inventory.ContainsKey(item))
        {
            if (Inventory[item] >= ammount)
            {
                Inventory[item] -= ammount;
                if (Inventory[item] <= 0)
                {
                    print(item.name);
                    Inventory.Remove(item);
                    if (selectedItem == item) ScrollSelectItem(-10, true);
                }
                if (isPlayer) HUDAndMenu.instance.UpdateIcon();
                return true;
            }
            if (isPlayer) HUDAndMenu.instance.UpdateIcon();
            return false;
        }
        return false;
    }

    public void ScrollSelectItem(int byAmmount, bool specify = false)
    {
        if (!specify) selectedItemIndex += byAmmount;
        else if (byAmmount != -10) selectedItemIndex = byAmmount;
        print(selectedItemIndex);
        if (selectedItemIndex > 10 || selectedItemIndex > Inventory.Count - 1) selectedItemIndex = 0;
        if (selectedItemIndex == -1) selectedItemIndex = Inventory.Count - 1;
        if (Inventory.Count > 0 && Inventory.Count >= selectedItemIndex) selectedItem = Inventory.ElementAt(selectedItemIndex).Key;
        if (byAmmount == -10 && specify) { selectedItem = null; }
        else selectedItem = null;
        if (isPlayer) HUDAndMenu.instance.UpdateIcon();
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