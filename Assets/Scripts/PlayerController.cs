using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    //public float speed = 5;
    public CharacterController controller;
    public DialogManager dialogManager;
    //PlayerControls controls;

    public string playerName = "Chinmay";
    
    [SerializeField]
    private InputActionReference movement;
    [SerializeField]
    private InputActionReference jump;
    [SerializeField]
    private InputActionReference interact;
    [SerializeField]
    private GameObject textReference;
    [SerializeField]
    private TMP_Text dialog;

    private Transform cameraMainTransform;
    private Camera mainCam;
    private GameObject activeNPC;


    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 4f;

    private void OnEnable()
    {
        movement.action.Enable();
        jump.action.Enable();
        interact.action.Enable();
    }

    private void OnDisable()
    {
        movement.action.Disable();
        jump.action.Disable();
        interact.action.Disable();
    }

    private void Start()
    {
        mainCam = Camera.main;
        cameraMainTransform = mainCam.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("camera is" + mainCam.name);
        textReference.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        textReference.SetActive(true);
        activeNPC = other.gameObject;
        Debug.Log("player collides with " + other.gameObject.name);
        activeNPC.GetComponent<NPC>().lookAtMe = true;
        dialog.text = activeNPC.GetComponent<NPC>().calloutString;
        //other.gameObject.transform.LookAt(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        textReference.SetActive(false); 
        other.gameObject.GetComponent<NPC>().lookAtMe = false;
        activeNPC = null;
    }

    void FixedUpdate()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 mover = movement.action.ReadValue<Vector2>();

        Vector3 move = new Vector3(mover.x, 0, mover.y);
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (jump.action.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        if (interact.action.triggered)
        {
            Debug.Log("interaction button pressed");
            if(activeNPC != null)
            {
                StartConversation(activeNPC);
            }
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (mover != Vector2.zero)
        {
            float targetangle = Mathf.Atan2(mover.x, mover.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetangle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void StartConversation(GameObject activeNPC)
    {
        movement.action.Disable();
        jump.action.Disable();
        interact.action.Disable();
        mainCam.GetComponent<CinemachineBrain>().enabled = false;
        Vector3 boi = transform.position;
        boi.y += 2;
        mainCam.transform.position = boi;
        

    }

    /*
    private void Awake()
    {
        controls = new PlayerControls();
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }



    private void Update()
    {
        Vector2 moveVec = controls.Player.Move.ReadValue<Vector2>();
        Vector3 moveVec3 = new Vector3(moveVec.x,0,moveVec.y) * Time.deltaTime * speed;
        controller.Move(moveVec3);
    }*/
}
