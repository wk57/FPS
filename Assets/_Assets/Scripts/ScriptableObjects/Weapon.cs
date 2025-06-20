using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;
    public int weaponDamage;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawn;
    bool allowReset = true;
    [SerializeField] bool isShooting, readyToShoot;
    [SerializeField] float bulletVelocity = 30;
    [SerializeField] float bulletPrefabLifetime = 3f;

    public enum ShootingMode
    {
        Single,
        Auto,
        Busrt
    }

    [SerializeField] ShootingMode currentShootingMode;

    [SerializeField] float shootingDelay = 2f;

    public int bulletPerBurst = 1;
    [SerializeField] int currentBurst;

    [SerializeField] float spreadIntensity;
    [SerializeField] float hipFireSpread;
    [SerializeField] float adsFireSpread;


    [SerializeField] GameObject muzzleEffect;
    internal Animator animator;

    //REloading ammo
    [SerializeField] float reloadTime;
    public int magazineSize, bulletsLeft;
    [SerializeField] bool isReloading;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    private bool isADS;
    
    //SABER QUE ARMA ES
    public enum WeaponModel
    {
        M1911,
        AK47
    }

    public WeaponModel thisWeaponModel;

    private void Awake()
    {
        readyToShoot = true;
        currentBurst = bulletPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = magazineSize;

        spreadIntensity = hipFireSpread;
    }


    // -----------------------------------------------UPDATE----------------------------------------
    void Update()
    {


        if (isActiveWeapon)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("WeaponRender");
            }

            if (Input.GetMouseButtonDown(1))
            {
                EnterADS();
            }

            if (Input.GetMouseButtonUp(1))
            {
                ExitADS();
            }


            GetComponent<Outline>().enabled = false;

            if (bulletsLeft == 0 && isShooting)
            {
                SoundManager.Instance.emptyM1911.Play();
            }

            if (currentShootingMode == ShootingMode.Auto)
            {
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Busrt)
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }


            //recarga
            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReloading == false && WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
            {
                Reload();
            }

            //recarga automatica

            /*if (readyToShoot && !isShooting && isReloading == false && bulletsLeft <= 0)
            {
                Reload();
            }
            */

            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                currentBurst = bulletPerBurst;
                FireWeapon();
            }

        }
        else {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("WeaponRender");
            }
        }


    }
   

    //-----------------------------------FUERA DE UPDATE--------------------------------------------------\\
    private void EnterADS()
    {
        animator.SetTrigger("ENTERADS");
        isADS = true;
        HUDManager.Instance.crossHair.SetActive(false);
        spreadIntensity = adsFireSpread;
    }

    private void ExitADS()
    {
        animator.SetTrigger("EXITADS");
        isADS = false;
        HUDManager.Instance.crossHair.SetActive(true);
        spreadIntensity = hipFireSpread;
    }

    private void FireWeapon()
    {
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();

        if (isADS)
        {
            animator.SetTrigger("ADSRECOIL");
        }
        else 
        
        {
            animator.SetTrigger("RECOIL");
        }

        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionaAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        Bullet bul =bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;

        bullet.transform.forward = shootingDirection;

        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifetime));

        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        if (currentShootingMode == ShootingMode.Busrt && currentBurst > 1)
        {
            currentBurst--;
            Invoke("FireWeapon", shootingDelay);
        }

    }

    private void Reload()
    {
       

        animator.SetTrigger("RELOAD");
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);
        isReloading = true;
        readyToShoot = false;
              

        Invoke("ReloadCompleted", reloadTime);
        
    }

    private void ReloadCompleted()
    {
        //Metodo para perder balas en provedor
        //if (WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > magazineSize)
        //{
        //    bulletsLeft = magazineSize;
        //    WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        //}
        //else
        //{
        //    bulletsLeft = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel);
        //    WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        //}

        //Mantener balas en proveedor y sumar las de reserva
        int bulletsNeeded = magazineSize - bulletsLeft;
        int availableAmmo = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel);
        int bulletsToReload = Math.Min(bulletsNeeded, availableAmmo);

        bulletsLeft += bulletsToReload;

        WeaponManager.Instance.DecreaseTotalAmmo(bulletsToReload, thisWeaponModel);

        isReloading = false;
        readyToShoot = true;
    }


    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionaAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
