using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : Actor
{
    CharacterController controller;

    [Header("Player Attributes")]
    [SerializeField] float speed = 10;
    [SerializeField] float gravity = 9.81f;
    [SerializeField] float mass = 2;
    private float vSpeed = 0;

    public Vector3 moveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement for player
        // Gets horizontal and vertical axis of input
        // Vector3 playerInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // playerInput = Vector3.ClampMagnitude(playerInput, 1f);

        controller.Move(moveDirection * speed * Time.deltaTime);

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



    void TDPMovement()
    {
        Transform Cam = Camera.main.transform;
        Vector3 CamForward = Cam.forward, moveDir = Vector3.zero; //Camera Forawrd and Move Direction
        Vector2 JoyDir = Vector2.zero; //Joystick Direction;
        if (Cam != null) { CamForward = Vector3.Scale(Cam.forward, new Vector3(1, 0, 1)).normalized; }
        //Annoying move hack
        if (JoyDir.magnitude > 0) moveDir = JoyDir.y * CamForward + JoyDir.x * Cam.right;
        else if (moveDir.magnitude > 0) moveDir = Vector3.zero;

        if (moveDir.magnitude > 0)
        {
            // agent.Move(moveDir * Time.deltaTime * Speed); //If Using NavMeshAgent
            // if (moveDir != Vector3.zero)
            // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir, transform.up), Time.deltaTime * Speed);
        }
    }
}
