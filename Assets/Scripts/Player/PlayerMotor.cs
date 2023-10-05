

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{

    private CharacterController controller;
    [HideInInspector] public PlayerInput playerInput;
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
    
    [Header("Weapon")] 
    public List<WeaponController> weapons;
    public float weaponAnimationSpeed;
    private int _currentWeapon;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = new PlayerInput();
        
        //Camera Init
        cameraRotation = cameraHolder.localRotation.eulerAngles;
        characterRotation = transform.localRotation.eulerAngles;
        
        playerInput.Player.Jump.performed += ctx => Jump();
        playerInput.Player.SprintStart.performed += ctx => SprintPressed();
        playerInput.Player.SprintRelease.performed += ctx => SprintRelease();
        
        //Weapon Init;
        playerInput.Player.AimingPressed.performed += e => weapons.ForEach(weapon => weapon.AimingPressed());
        playerInput.Player.AimingReleased.performed += e => weapons.ForEach(weapon => weapon.AimingReleased());
        playerInput.Player.FirePressed.performed += e => weapons.ForEach(weapon => weapon.IsShooting());
        playerInput.Player.FireReleased.performed += e => weapons. ForEach(weapon => weapon.ShootingReleased());
        playerInput.Player.WeaponSwitch.performed += e => SwitchWeapon();
        weapons.ForEach(weapon => weapon.Initialize(this));
        weapons[0].IsFullAuto(false); weapons[1].IsFullAuto(true);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        isGrounded = controller.isGrounded;
        speed = isSprinting ? sprintSpeed : 5f;
        xSensivity = GameValues.mousesensitivity;
        ySensivity = GameValues.mousesensitivity;

    }

    private void Update()
    { 
        inputMovement = playerInput.Player.Movement.ReadValue<Vector2>();
        inputView = playerInput.Player.Look.ReadValue<Vector2>();
        ProcessLook();
        ProcessMove();
    }

    
    private void ProcessLook()
    {
        
        cameraRotation.x += (-ySensivity * inputView.y * Time.deltaTime);
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -70, 80);
        characterRotation.y += (inputView.x * Time.deltaTime * xSensivity);
        transform.localRotation = Quaternion.Euler(characterRotation);
        
        cameraHolder.localRotation = Quaternion.Euler(cameraRotation);
    }

    private void SwitchWeapon()
    {
        _currentWeapon++;
        _currentWeapon %= 2;
        for(var i = 0; i < weapons.Count(); i++ )
        {
            weapons[i].gameObject.SetActive(false);
        }
        weapons[_currentWeapon].gameObject.SetActive(true);
    }
    
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
        playerInput.Player.Enable();
    }

    private void OnDisable() 
    {
        playerInput.Player.Disable();
    }
}
