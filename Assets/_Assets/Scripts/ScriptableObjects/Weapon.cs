using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;

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

    [SerializeField] float spreadIntesnsty;

    [SerializeField] GameObject muzzleEffect;
    internal Animator animator;

    //loading ammo
    [SerializeField] float reloadTime;
    public int magazineSize, bulletsLeft;
    [SerializeField] bool isReloading;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;
    
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
    }


    // Update is called once per frame
    void Update()
    {


        if (isActiveWeapon)
        {

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
            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReloading == false)
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


    }

    private void FireWeapon()
    {
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOIL");

        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionaAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

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
        bulletsLeft = magazineSize;
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

        float x = UnityEngine.Random.Range(-spreadIntesnsty, spreadIntesnsty);
        float y = UnityEngine.Random.Range(-spreadIntesnsty, spreadIntesnsty);

        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
