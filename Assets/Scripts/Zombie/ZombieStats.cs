using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

public class ZombieStats : MonoBehaviour {
    [SerializeField] private float health = 100;
    [SerializeField] public float attackSpeed;
    [SerializeField] public float attackDamage;
    private ZombieSpawner zombieSpawner;
    private float timeOfLastAttack = 0f;

    private bool isDead = false;

    private void Start()
    {
        zombieSpawner = GameObject.FindGameObjectWithTag("ZombieSpawner").GetComponent<ZombieSpawner>();
    }

    public void Update() {
        if (health <= 0) {
            Die();
        }
    }

    public void TakeDamage(float damage) {
        health -= damage;
        Debug.Log(health);
    }

    private void Die() {
        zombieSpawner.DespawnZombie(this.gameObject);
        Debug.Log("Zombie died");
    }
}
