using UnityEngine;
using static Weapon;


public class SoundManager : MonoBehaviour
{
    //singleton

    public static SoundManager Instance { get; set; }

    public AudioSource ShootingChannel;
  

    public AudioClip ak47Shot;
    public AudioClip m1911Shot;

    public AudioSource reloadingSoundM1911;
    public AudioSource emptyM1911;

    public AudioSource reloadingSoundAk47;
    public AudioSource emptyAk47;

    public AudioSource throwablesChannel;
    public AudioClip grenadeSound;

    public AudioClip zombieWalk;
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieHurt;
    public AudioClip zombieDeath;

    public AudioSource ZombieChannel;
    public AudioSource ZombieChannel2;

    public AudioSource PlayerChannel;
    public AudioClip playerHurt;
    public AudioClip playerDeath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else { Instance = this; }
    }


    public void PlayShootingSound(WeaponModel weapon)
    {       

        switch (weapon)
        {
            case WeaponModel.M1911:
                ShootingChannel.PlayOneShot(m1911Shot);
                break;
            case WeaponModel.AK47:
                ShootingChannel.PlayOneShot(ak47Shot);
                break;

        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.M1911:
                reloadingSoundM1911.Play();
                break;
            case WeaponModel.AK47:
                reloadingSoundAk47.Play();
                break;

        }
    }


    public void PlayEmptySound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.M1911:
                emptyM1911.Play();
                break;
            case WeaponModel.AK47:
                emptyAk47.Play();
                break;

        }
    }
}
