using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableObject : MonoBehaviour
{
    private int health;
    private int defaultHealth;

    public void GetDamaged(int damage_value) {
        health -= damage_value;
        if (health <= 0)
            Die();
    }

    void Die() {

    }

    public void ResetHealth() {
        health = defaultHealth;
    }

    void Start()
    {
        ResetHealth();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
