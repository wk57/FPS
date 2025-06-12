using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit " + collision.gameObject.name + " !");
            Destroy(gameObject);
            CreateBulletImpactEffect(collision);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            print("hit wall");
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<Enemy>().isDead == false) 
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
            }

            collision.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);

            CreateBloodEffect(collision);

            Destroy(gameObject);
        }
    }


    private void CreateBloodEffect(Collision objectHit)
    {
        ContactPoint contact = objectHit.contacts[0];

        GameObject bloodSprayPrefab = Instantiate(GlobalReferences.Instance.bloodEffect, contact.point, Quaternion.LookRotation(contact.normal)

            );

        bloodSprayPrefab.transform.SetParent(objectHit.gameObject.transform);
    }


    void CreateBulletImpactEffect(Collision objectHit)
    {
        ContactPoint contact = objectHit.contacts[0];

        GameObject hole = Instantiate(GlobalReferences.Instance.bulletImpactEffectPrefab, contact.point, Quaternion.LookRotation(contact.normal)
            
            );

        hole.transform.SetParent(objectHit.gameObject.transform);
    }




}
