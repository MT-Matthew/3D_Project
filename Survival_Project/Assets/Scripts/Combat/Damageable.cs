using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Damageable : MonoBehaviour
{
    public float health = 100f;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void TakeDamage(float amount, Vector3 knockback)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }

        ApplyKnockback(knockback);
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected void ApplyKnockback(Vector3 knockback)
    {
        if (rb != null)
        {
            rb.AddForce(knockback, ForceMode.Impulse);
        }
    }
}
