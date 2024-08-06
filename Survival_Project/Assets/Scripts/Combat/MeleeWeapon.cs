using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public float damage;
    public bool isAttacking;
    public float knockbackForce;

    Collider weaponCollider;

    void Start()
    {
        weaponCollider = GetComponent<BoxCollider>();
        weaponCollider.enabled = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartAttack();
        }
    }

    void StartAttack()
    {
        isAttacking = true;
        weaponCollider.enabled = true;
    }

    void EndAttack()
    {
        isAttacking = false;
        weaponCollider.enabled = false;
    }


    void OnTriggerEnter(Collider other)
    {
        if (isAttacking && other.CompareTag("Monster"))
        {
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable != null)
            {
                Vector3 knockbackDirection = other.transform.position - transform.position;
                knockbackDirection.y = 0;
                damageable.TakeDamage(damage, knockbackDirection.normalized * knockbackForce);
                
            }
        }
        
    }
}
