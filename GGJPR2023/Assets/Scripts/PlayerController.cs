using UnityEngine;
using UnityEngine.InputSystem;
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
    private Vector3 moveDirection = Vector3.zero;
    [SerializeField] float smoothInputSpeed = 0.15f;
    public ItemInfo selectedItem = null;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement for player
        lastDirection = Vector3.SmoothDamp(lastDirection, moveDirection, ref smoothInputVelocity, smoothInputSpeed);
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
        moveDirection = new Vector3(dir.x, 0, dir.y);
    }

    public void OnInteract()
    {
        print("Trying to touch grass.");
        // FindClosestInteraction()?.Interact(); //Classic
        if (selectedItem == null) TryInteract("Grab");
        else TryInteract();
    }













    // void TDPMovement()
    // {
    //     Transform Cam = Camera.main.transform;
    //     Vector3 CamForward = Cam.forward, moveDir = Vector3.zero; //Camera Forawrd and Move Direction
    //     Vector2 JoyDir = Vector2.zero; //Joystick Direction;
    //     if (Cam != null) { CamForward = Vector3.Scale(Cam.forward, new Vector3(1, 0, 1)).normalized; }
    //     //Annoying move hack
    //     if (JoyDir.magnitude > 0) moveDir = JoyDir.y * CamForward + JoyDir.x * Cam.right;
    //     else if (moveDir.magnitude > 0) moveDir = Vector3.zero;

    //     if (moveDir.magnitude > 0)
    //     {
    //         // agent.Move(moveDir * Time.deltaTime * Speed); //If Using NavMeshAgent
    //         // if (moveDir != Vector3.zero)
    //         // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir, transform.up), Time.deltaTime * Speed);
    //     }
    // }
}
