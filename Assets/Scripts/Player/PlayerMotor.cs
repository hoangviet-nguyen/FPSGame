

using System.Linq;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{

    private CharacterController controller;
    private PlayerInput playerInput;
    public float speed;
    private bool isGrounded;
    private bool isSprinting;
    private Vector3 playerJump;
    public Transform cameraHolder;


    [Header("Player Movement")] [Header("Settings")]
    private Vector3 inputMovement;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float sprintSpeed = 10f;

    [Header("Camera Movement")] [Header("Settings")]
    private Vector3 cameraRotation;
    private Vector3 characterRotation;
    [SerializeField] private float xSensivity = 30f;
    [SerializeField] private float ySensivity = 30f;
    [HideInInspector] public Vector2 inputView;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = new PlayerInput();
        
        //Camera Init
        cameraRotation = cameraHolder.localRotation.eulerAngles;
        characterRotation = transform.localRotation.eulerAngles;
        
        playerInput.OnFoot.Jump.performed += ctx => Jump();
        playerInput.OnFoot.SprintStart.performed += ctx => SprintPressed();
        playerInput.OnFoot.SprintRelease.performed += ctx => SprintRelease();
        
        //Weapon Init;
        playerInput.OnFoot.AimingPressed.performed += e => AimingPressed();
        playerInput.OnFoot.AimingReleased.performed += e => AimingReleased();
        
        if (weapon)
        { 
            weapon.Initialize(this);
        }
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        isGrounded = controller.isGrounded;
        speed = isSprinting ? sprintSpeed : 5f;

    }

    private void Update()
    {
        inputMovement = playerInput.OnFoot.Movement.ReadValue<Vector2>();
        inputView = playerInput.OnFoot.Look.ReadValue<Vector2>();
        ProcessLook();
        ProcessMove();
        CalculateAimingIn();
    }

    
    private void ProcessLook()
    {
        
        cameraRotation.x += (-ySensivity * inputView.y * Time.deltaTime);
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -70, 80);
        characterRotation.y += (inputView.x * Time.deltaTime * xSensivity);
        transform.localRotation = Quaternion.Euler(characterRotation);
        
        cameraHolder.localRotation = Quaternion.Euler(cameraRotation);
    }

    #region -Weapon-

    [Header("Weapon")] 
    public WeaponController weapon;
    public float weaponAnimationSpeed;
    [HideInInspector] public bool isFalling;

    [Header("Aiming")] 
    public bool isAiming;

    private void AimingPressed()
    {
        isAiming = true;
    }

    private void AimingReleased()
    {
        isAiming = false;
    }

    private void CalculateAimingIn()
    {
        if (!weapon)
        {
            return;
        }

        weapon.isAiming = isAiming;
    }
    

    #endregion

    #region - Movement -
    public void SprintPressed()
    {
        isSprinting = true;
    }
    public void ProcessMove()
    {

        var vertical = speed * inputMovement.y * Time.deltaTime;
        var horizontal = speed * inputMovement.x * Time.deltaTime;

        var movement = new Vector3(horizontal, 0, vertical);
        movement = transform.TransformDirection(movement);
        controller.Move(movement);

        //Gravity of Player
        playerJump.y += gravity * Time.deltaTime;
        if(isGrounded && playerJump.y < 0) playerJump.y = -2f;
        controller.Move(playerJump * Time.deltaTime);

        //Debug.Log(playerVelocity.y);
    }

    public void SprintRelease()
    {
        isSprinting = false;
    }
    
    public void Jump() 
    {
        if(isGrounded)
        {
            playerJump.y  = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
    #endregion
    
    private void OnEnable()
    {
        playerInput.OnFoot.Enable();
    }

    private void OnDisable() 
    {
        playerInput.OnFoot.Disable();
    }

    public PlayerInput getPlayerInput()
    {
        return playerInput;
    }
}
