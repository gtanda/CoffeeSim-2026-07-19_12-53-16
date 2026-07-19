using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")] 
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 7f;
    private float currentSpeed;
    private bool isSprinting;

    [Header("Jump")] [SerializeField] private float jumpHeight = 1.2f;
    
    private CharacterController _characterController;
    private PlayerControls _playerControls;



    private Vector2 moveInput;
    private Vector2 lookInput;

    public Vector2 LookInput => lookInput;

    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    private Vector3 velocity;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerControls = new PlayerControls();

        _playerControls.Player.Movement.performed += OnMove;
        _playerControls.Player.Movement.canceled += OnMoveCancelled;

        _playerControls.Player.Rotation.performed += OnLook;
        _playerControls.Player.Rotation.canceled += OnLookCancelled;
        
        _playerControls.Player.Sprint.performed += OnSprint;
        _playerControls.Player.Sprint.canceled += OnSprintCancelled;
        
        _playerControls.Player.Jump.performed += OnJump;

    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCancelled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void OnLookCancelled(InputAction.CallbackContext context)
    {
        lookInput = Vector2.zero;
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = true;
        currentSpeed = sprintSpeed;
    }
    
    private void OnSprintCancelled(InputAction.CallbackContext context)
    {
        isSprinting = false;
        currentSpeed = moveSpeed;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void Update()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        _characterController.Move(move * currentSpeed * Time.deltaTime);

        if (_characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }
        
        velocity.y += gravity * Time.deltaTime;
        _characterController.Move(velocity * Time.deltaTime);
    }
}
