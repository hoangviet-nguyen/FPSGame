using TMPro;
using UnityEngine;
using Zombie;

public class WeaponController : MonoBehaviour
{
    private PlayerMotor _motor;

    [Header("References")]
    public AudioClip bulletSound;
    public Transform bulletSpawn;
    public Models.WeaponModel settings;
    private bool initialize;


    private Vector3 _weaponRotation;
    private Vector3 _rotationVelocity;

    private Vector3 targetWeaponRotation;
    private Vector3 targetWeaponRotationVelocity;

    
    [Header("Weapon Sway")] 
    public Transform weaponSwayObject;

    [HideInInspector] public bool isAiming;
    [Header("Sights")] 
    public Transform sightTarget;
    private Vector3 weaponSwayPosition;

    [Header("Projectiles")] public float fireRate;
    private float currentFireRate;
    public bool isShooting;
    public bool hitmarkerShowing;
    public  GameObject Hitmarker;
    public float damage;
    private bool isFullAuto;
    private ZombieController mockZombie;
    public void Initialize(PlayerMotor motor)
    {
        _motor = motor;
        initialize = true;
        isFullAuto = false;
        mockZombie = new ZombieController();
    }

    private void Start()
    {
        _weaponRotation = transform.localRotation.eulerAngles;
        isShooting = false;
        isAiming = false;
    }

    private void Update()
    {
        if (!initialize)
        {
            return;
        }
        
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
        weaponSwayPosition = Vector3.SmoothDamp(weaponSwayPosition, targetPosition, ref weaponSwayPosition, 0);
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
    

    private void CalculateWeaponSway()
    {
        targetWeaponRotation.y += settings.SwayAmount * _motor.inputView.x * Time.deltaTime;
        targetWeaponRotation.x += settings.SwayAmount * _motor.inputView.y * Time.deltaTime;


        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, new Vector3(0, 0, 0),
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
                var currentZombie = hitInfo.collider.GetComponent<ZombieController>();
                if (isShooting)
                {
                    ShowHitmarker();
                    Shoot(currentZombie, isFullAuto);
                }
        }
        else if (isShooting)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().pitch = fireRate;
                GetComponent<AudioSource>().PlayOneShot(bulletSound);
                isShooting = isFullAuto;
            }
            
        }
    }

    private void Shoot(ZombieController zombie, bool isFullAuto)
    {
        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().pitch = fireRate;
            GetComponent<AudioSource>().PlayOneShot(bulletSound);
        }
        zombie.TakeDamage(damage);
        isShooting = isFullAuto;
    }
    

    public void IsFullAuto(bool fullAuto)
    {
        isFullAuto = fullAuto;
    }


    public void IsShooting()
    {
        isShooting = true;
    }

    public void ShootingReleased()
    {
       isShooting = false;
    }
    private void ShowHitmarker()
    {
        if (hitmarkerShowing)
        {
            return;
        }
        
        
        Hitmarker.SetActive(true);
        Debug.Log("Hitmarker");
        hitmarkerShowing = true;
        Invoke("HideHitmarker", 0.1f);
        
    }
    private void HideHitmarker()
    {
        Hitmarker.SetActive(false);
        hitmarkerShowing = false;
    }
    

    #endregion
}