using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

public class ZombieStats : MonoBehaviour {
    [SerializeField] private float health = 100;
    [SerializeField] public float attackSpeed;
    [SerializeField] public float attackDamage;
    [SerializeField] AudioClip zombieDamage;
    [SerializeField] ParticleSystem zombieBlood;
    private AudioSource audioSource;
    private ZombieSpawner zombieSpawner;
    private float timeOfLastAttack = 0f;

    private bool isDead = false;

    private void Start()
    {
        zombieSpawner = GameObject.FindGameObjectWithTag("ZombieSpawner").GetComponent<ZombieSpawner>();
        audioSource = GetComponent<AudioSource>();
    }
namespace Zombie {
    public class ZombieStats : MonoBehaviour {
        [SerializeField] public float health;
        [SerializeField] public float attackSpeed;
        [SerializeField] public float attackDamage;

        public void Update() {
            if (Input.GetKeyDown(KeyCode.Q)) {
                TakeDamage(10);
                Debug.Log("Zombie hit");
            }
        }

        public void TakeDamage(float damage) {
            audioSource.PlayOneShot(zombieDamage);
            health -= damage;
        }
    }
}
