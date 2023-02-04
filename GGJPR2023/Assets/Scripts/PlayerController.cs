using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : Actor
{
    CharacterController controller;

    [Header("Player Attributes")]
    [SerializeField] float speed = 10;
    [SerializeField] float gravity = 9.81f;
    // Vertical speed
    private float vSpeed = 0;

    //For inputs and moving
    private Vector3 smoothInputVelocity;
    private Vector3 lastDirection = Vector3.zero;
    [SerializeField] float smoothInputSpeed = 0.15f;

    // Mouse stuff
    [Tooltip("What layers the mouse will detect.")]
    [Header("Mouse Detection Attributes")]
    [SerializeField] LayerMask layersToDetect;

    private void OnEnable()
    {
        isPlayer = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement for player
        lastDirection = Vector3.SmoothDamp(lastDirection, moveDir, ref smoothInputVelocity, smoothInputSpeed);
        controller.Move(lastDirection * speed * Time.deltaTime);

        if (controller.isGrounded)
        {
            vSpeed = 0;
        }
        // Apply gravity
        vSpeed -= gravity * Time.deltaTime;
        controller.Move(new Vector3(0, vSpeed * Time.deltaTime, 0));
    }

    public void OnMove(InputValue val)
    {
        Vector2 dir = val.Get<Vector2>();
        moveDir = new Vector3(dir.x, 0, dir.y);
    }

    public void OnInteract()
    {
        // Debug.Log("Interacted");
        activeIntractable = FindClosestInteraction();
        TryInteract();
    }

    public void OnFire()
    {
        if (!selectedItem) return;
        if (selectedItem.itemType == ItemType.Throwable) ThrowProjectile(selectedItem, (GetMousePosition() - transform.position).normalized);
        // UseItem(selectedItem); //No need to remove item
    }

    //For Touch and/or Controller support. (Not needed hopefully)
    //public void OnScrollUp() => ScrollSelectItem(1);
    //public void OnScrollDown() => ScrollSelectItem(-1);
    public void OnScrollInventory(InputValue val)
    {
        if (val.Get<Vector2>().y != 0)
        {
            ScrollSelectItem((int)Mathf.Clamp(val.Get<Vector2>().y, -1, 1));
        }
    }
    public void OnPutAway() => ScrollSelectItem(-10, true);

    public override void Consume(ItemInfo item, bool useItem = true)
    {
        if (item == null) return;
        Heal(item.effectiveAmount);
        if (useItem && Inventory.ContainsKey(item)) RemoveItem(item, 1);
    }

    // Gets world space of mouse
    private Vector3 GetMousePosition()
    {
        Vector3 screenPosition = Input.mousePosition;
        Vector3 worldPosition;
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitData, 100, layersToDetect)) worldPosition = hitData.point;
        else return transform.position + Vector3.forward;
        return new Vector3(worldPosition.x, transform.position.y, worldPosition.z);
    }
}
