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
    [Header("Settings")] public Models.WeaponModel settings;
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

    [HideInInspector] 
    public bool isAiming;

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

    private void Start() 
    {
        _weaponRotation = transform.localRotation.eulerAngles;
        currentFireType = allowedFireType.First();
    }

    private void Update()
    {
        if (!initialize)
        {
            return;
        }
        
        CalculateWeaponRotation();
        SetWeaponAnimations();
        CalculateWeaponSway();
        CalculateAiming();
        CalculateBullet();
    }

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
    
    
    public void TriggerJump(){}

    private void CalculateWeaponRotation()
    {
    }
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

    private void CalculateBullet()
    {
        if (isShooting)
        {
            TapShot();
            
            if (currentFireType == Models.WeaponFireType.Pistols)
            {
                isShooting = false;
            }
        }
    }

    public void TapShot()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawn);
        
        //Load bullet settings
        
    }
    
    private Vector3 LissajousCurve(float Time, float A, float B)
    {
        return new Vector3(Mathf.Sin(Time), A * Mathf.Sin(B * Time + Mathf.PI));
    }
    
}
