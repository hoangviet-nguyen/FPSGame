using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using static Models.WeaponModel;

public class WeaponController : MonoBehaviour
{
    private PlayerMotor _motor;

    [Header("References")] 
    public Animator weaponAnimator;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed;
    public LayerMask HitMask;
    public Models.WeaponModel settings;
    private bool initialize;
    
    
    private Vector3 _weaponRotation;
    private Vector3 _rotationVelocity;

    private Vector3 targetWeaponRotation;
    private Vector3 targetWeaponRotationVelocity;

    private Vector3 weaponMovementRotation;
    private Vector3 weaponMovementRotationVelocity;

    public bool isGroundedTrigger;

    public float fallingDelay;

    [Header("Weapon Sway")] 
    public Transform weaponSwayObject;
    public float swayAmountA = 1;
    public float swayAmountB = 2;
    public float swayScale = 600;
    public float swayLerpSpeed = 14;

    private float swayTime;
    private Vector3 swayPosition;
    private bool isAiming;

    [Header("Sights")] 
    public Transform sightTarget;
    public float sightOffset;
    public float aimingTime;
    private Vector3 weaponSwayPosition;
    
    [Header("Projectiles")]
    public float fireRate;
    private float currentFireRate;
    public List<Models.WeaponFireType> allowedFireType; //The user can toggle between weapons
    public Models.WeaponFireType currentFireType;
    public bool isShooting;
    
    
    
    
    public void Initialize(PlayerMotor motor)
    {
        _motor = motor;
        initialize = true;
    }

    private void Awake()
    {
        //Weapon Init;
        _motor.playerInput.Player.FirePressed.performed += e => IsShooting();
        _motor.playerInput.Player.FireReleased.performed += e => ShootingReleased();
        _motor.playerInput.Player.AimingPressed.performed += e => AimingPressed();
        _motor.playerInput.Player.AimingReleased.performed += e => AimingReleased();
    }

    private void Start() 
    {
        _weaponRotation = transform.localRotation.eulerAngles;
        currentFireType = allowedFireType.First();
        isShooting = false;
    }

    private void Update()
    {
        if (!initialize)
        {
            return;
        }
        SetWeaponAnimations();
        CalculateWeaponSway();
        CalculateAiming();
        CalculateBullet();
    }

    #region Aiming
    private void CalculateAiming()
    {
        var targetPosition = transform.position;

        if (isAiming)
        {
            targetPosition = _motor.cameraHolder.transform.position + (weaponSwayObject.transform.position - sightTarget.transform.position);
        }

        weaponSwayPosition = weaponSwayObject.transform.position;
        weaponSwayPosition = Vector3.SmoothDamp(weaponSwayPosition, targetPosition, ref weaponSwayPosition, aimingTime);
        weaponSwayObject.transform.position = weaponSwayPosition;
    }
    private void AimingPressed()
    {
        isAiming = true;
    }

    private void AimingReleased()
    {
        isAiming = false;
    }
    
    #endregion
    
    private void SetWeaponAnimations(){}

    private void CalculateWeaponSway()
    {
        targetWeaponRotation.y += settings.SwayAmount * _motor.inputView.x * Time.deltaTime;
        targetWeaponRotation.x += settings.SwayAmount * _motor.inputView.y * Time.deltaTime;
        
        
        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, new Vector3(0,270,0) , ref targetWeaponRotationVelocity, 
                                                    settings.SwayResetSmooting);
        
        _weaponRotation = Vector3.SmoothDamp(_weaponRotation, targetWeaponRotation, ref _rotationVelocity,
                                            settings.SwaySmoothing);
        transform.localRotation = Quaternion.Euler(_weaponRotation);

    }

    #region Shooting
    private void CalculateBullet()
    {
        
        Ray ray = new Ray(bulletSpawn.position, bulletSpawn.right * 5);
        Debug.DrawRay(ray.origin, ray.direction);
        RaycastHit hitInfo; // stores information if something is hit
        bool hit = Physics.Raycast(ray, out hitInfo);
        
        if (hitInfo.collider != null && hitInfo.collider.CompareTag("Zombie"))
        {
            var currentZombie = hitInfo.collider.GetComponent<ZombieStats>();
            if (isShooting)
            {
                TapShot(currentZombie);
            }
        }
        
    }
    public void TapShot(ZombieStats stats)
    {
        var bullet = Instantiate(bulletPrefab,bulletSpawn);
        stats.TakeDamage(5);
        isShooting = false;

    }
    
    public void IsShooting()
    {
       isShooting = true;
    }

    public void ShootingReleased()
    {
      isShooting = false;
    }
    #endregion
    
    
    private Vector3 LissajousCurve(float Time, float A, float B)
    {
        return new Vector3(Mathf.Sin(Time), A * Mathf.Sin(B * Time + Mathf.PI));
    }
    
}
