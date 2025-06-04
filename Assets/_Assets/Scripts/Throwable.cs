using System;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] float delay = 3f;
    [SerializeField] float damageRadious = 20f;
    [SerializeField] float explosiveForce = 1200f;

    private float countDowm;

    public bool hasExploded = false;
    public bool hasBeenThrow = false;

    public enum ThrowableType
    {
        None,
        Grenade
    }

    public ThrowableType throwableType;

    private void Start()
    {
        countDowm = delay; // Se inicializa el contador
    }

    private void Update()
    {
        if (hasBeenThrow && !hasExploded)
        {
            countDowm -= Time.deltaTime;
            if (countDowm <= 0f)
            {
                Explode();
                hasExploded = true;
            }
        }
    }


    private void Explode()
    {
        GetThrowableEffect();
        Destroy(gameObject);
    }

    private void GetThrowableEffect()
    {
        switch (throwableType)
        {
            case ThrowableType.Grenade:
                GrenadeEffect();
                break;
            
        }
    }

    private void GrenadeEffect()
    {
        GameObject explosionEffect = GlobalReferences.Instance.grenadeExplosionEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);

        //paysound
        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.grenadeSound);

        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadious);
        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosiveForce, transform.position, damageRadious);
            }

            if (objectInRange.gameObject.GetComponent<Enemy>())
            {
                objectInRange.gameObject.GetComponent<Enemy>().TakeDamage(100);
            }
        }
    }


}
