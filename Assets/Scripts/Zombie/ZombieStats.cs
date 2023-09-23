using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

public class ZombieStats : MonoBehaviour {
    [SerializeField] private float health;
    [SerializeField] public float attackSpeed;
    [SerializeField] public float attackDamage;
    private float timeOfLastAttack = 0f;

    private bool isDead = false;
    
    public void Update() {
        if (health <= 0) {
            Die();
        }
    }

    public void TakeDamage(float damage) {
        health -= damage;
    }

    private void Die() {
        Destroy(gameObject);
    }
}
