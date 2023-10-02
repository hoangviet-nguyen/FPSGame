using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private PlayerMotor _motor;

    [Header("References")] public Animator weaponAnimator;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public Models.WeaponModel settings;
    private bool initialize;


    private Vector3 _weaponRotation;
    private Vector3 _rotationVelocity;

    private Vector3 targetWeaponRotation;
    private Vector3 targetWeaponRotationVelocity;

    private Vector3 weaponMovementRotation;
    private Vector3 weaponMovementRotationVelocity;
    

    [Header("Weapon Sway")] 
    public Transform weaponSwayObject;

    private float swayTime;
    private Vector3 swayPosition;

    [HideInInspector] public bool isAiming;

    [Header("Sights")] 
    public Transform sightTarget;
    public float aimingTime;
    private Vector3 weaponSwayPosition;

    [Header("Projectiles")] public float fireRate;
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
        isShooting = false;
        isAiming = false;
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
            targetPosition = _motor.cameraHolder.transform.position +
                             (weaponSwayObject.transform.position - sightTarget.transform.position);
        }

        weaponSwayPosition = weaponSwayObject.transform.position;
        weaponSwayPosition = Vector3.SmoothDamp(weaponSwayPosition, targetPosition, ref weaponSwayPosition, aimingTime);
        weaponSwayObject.transform.position = weaponSwayPosition;
    }
    
    public void AimingPressed()
    {
        isAiming = true;
    }

    public void AimingReleased()
    {
        isAiming = false;
    }

    #endregion
    

    private void SetWeaponAnimations()
    {
    }

    private void CalculateWeaponSway()
    {
        targetWeaponRotation.y += settings.SwayAmount * _motor.inputView.x * Time.deltaTime;
        targetWeaponRotation.x += settings.SwayAmount * _motor.inputView.y * Time.deltaTime;


        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, new Vector3(0, 270, 0),
            ref targetWeaponRotationVelocity,
            settings.SwayResetSmooting);

        _weaponRotation = Vector3.SmoothDamp(_weaponRotation, targetWeaponRotation, ref _rotationVelocity,
            settings.SwaySmoothing);
        transform.localRotation = Quaternion.Euler(_weaponRotation);

    }

    #region Shooting

    private void CalculateBullet()
    {
        Ray ray = new Ray(bulletSpawn.position, bulletSpawn.right);
        Debug.DrawRay(ray.origin, ray.direction * 5);
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
        else if (isShooting)
        {
            TapShot(new ZombieStats());
        }
    }

    private void TapShot(ZombieStats zombie)
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawn);
        zombie.TakeDamage(100);
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
}