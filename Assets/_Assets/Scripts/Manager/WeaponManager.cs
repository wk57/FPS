using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using static Weapon;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }

    public List<GameObject> weaponSlots;
    public GameObject activeWeaponSlot;

    [Header("Ammo")]
    public int totalRifleAmmo = 0;
    public int totalPistolAmmo = 0;

    [Header("Grenades")]   
    public float throwForce = 40f;
    public float forceMultiplier = 0f;
    public float forceLimit = 2f;
    public GameObject grenadePrefab;
    public GameObject throwableSpawn;

    public int lethalsCount = 0;
    public Throwable.ThrowableType equippedLethalType;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else { Instance = this; }
    }

    private void Start()
    {
        activeWeaponSlot = weaponSlots[0];

        equippedLethalType = Throwable.ThrowableType.None;
    }

    private void Update()
    {
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlots(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlots(1);
        }

        if (Input.GetKey(KeyCode.G))
        {
            forceMultiplier += Time.deltaTime;

            if (forceMultiplier > forceLimit)
            {
                forceMultiplier = forceLimit;
            }
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            if (lethalsCount>0)
            {
                ThrowLethal();
            }

            forceMultiplier = 0;
        }
        
        

        
    }
        

    public void PickUpWeapon(GameObject pickWeapon)
    {
        AddWeaponIntoActiveSlot(pickWeapon);
    }

    private void AddWeaponIntoActiveSlot(GameObject pickWeapon)
    {
        DropCurrentWeapon(pickWeapon);

        pickWeapon.transform.SetParent(activeWeaponSlot.transform, false);  

        Weapon weapon = pickWeapon.GetComponent<Weapon>();

        pickWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        pickWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);

        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    private void DropCurrentWeapon(GameObject pickWeapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            weaponToDrop.GetComponent <Weapon>().animator.enabled = false;

            weaponToDrop.transform.SetParent(pickWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickWeapon.transform.localPosition;  
            weaponToDrop.transform.localRotation = pickWeapon.transform.localRotation;
        }
    }

    public void SwitchActiveSlots(int slotNumber)
    {
        if (activeWeaponSlot.transform.childCount >0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }

        activeWeaponSlot = weaponSlots[slotNumber];

        if (activeWeaponSlot.transform.childCount > 0 )
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
        }
    }

    internal void PickUpAmmo(AmmoBox ammo)
    {
        switch (ammo.ammoType)
        {
            case AmmoBox.AmmoType.ArAmmo:
                totalRifleAmmo += ammo.ammoAmount;
                break;
            case AmmoBox.AmmoType.PistolAmmo:
                totalPistolAmmo += ammo.ammoAmount;
                break;            
        }
    }

    internal void DecreaseTotalAmmo(int bulletsToDecrease, Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.M1911:
                totalPistolAmmo -= bulletsToDecrease;
                break;
            case Weapon.WeaponModel.AK47:
                totalRifleAmmo -= bulletsToDecrease;
                break;            
        }
    }

    public int CheckAmmoLeftFor(Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.M1911:
                return totalPistolAmmo;

            case Weapon.WeaponModel.AK47:
                return totalRifleAmmo;
            default:
                return 0;
        }
    }

    #region || --------- granadas --------------||
    public void pickUpThrowable(Throwable throwable)
    {
        switch (throwable.throwableType)
        {
            case Throwable.ThrowableType.Grenade:
                PickUpThrowableAsLethal(Throwable.ThrowableType.Grenade);
                break;
        }
    }

    private void PickUpThrowableAsLethal(Throwable.ThrowableType lethal)
    {
        if (equippedLethalType == lethal || equippedLethalType == Throwable.ThrowableType.None)
        {
            equippedLethalType = lethal;

            if (lethalsCount < 2)
            {
                lethalsCount += 1;
                Destroy(InteractionManager.Instance.hoveredThrowable.gameObject);
                HUDManager.Instance.UpdateThrowables();
            }
            else
            {
                print("Lethals full");
            }
        }
        else 
        {

        }
    }

  
    #endregion

    private void ThrowLethal()
    {
        GameObject letalPrefab = GetThrowablePrefab();

        GameObject throwable = Instantiate(letalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);

        throwable.GetComponent<Throwable>().hasBeenThrow = true;

        lethalsCount -= 1;
        if (lethalsCount <= 0)
        {
            equippedLethalType = Throwable.ThrowableType.None;
        }

        HUDManager.Instance.UpdateThrowables();
    }

    private GameObject GetThrowablePrefab()
    {
        switch (equippedLethalType)
        {
            case Throwable.ThrowableType.Grenade:
                return grenadePrefab;               
        }

        return new();
    }
}
