using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using World;

namespace Zombie {
    public class ZombieController : MonoBehaviour {
        private NavMeshAgent _agent;
        private Animator animator;
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] internal Transform target;
        [SerializeField] AudioClip zombieAttack;
        [SerializeField] AudioClip zombieIdle;
        [SerializeField] AudioClip zombieDamage;
        private AudioSource audioSource;
        private float _timeOfLastAttack = 0f;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private ZombieSpawner _zombieSpawner;
        private static readonly int Die1 = Animator.StringToHash("Die1");
        
        [SerializeField] public float health;
        [SerializeField] public float attackSpeed;
        [SerializeField] public float attackDamage;
        [SerializeField] ParticleSystem zombieBlood;
        private float timeOfLastAttack = 0f;

        private bool isDead = false;
        
        private void Start()
        {
            health = 100;
            GetReferences();
            _zombieSpawner = GameObject.FindGameObjectWithTag("ZombieSpawner").GetComponent<ZombieSpawner>();
        }

        private void Update() {
            _agent.SetDestination(target.position);
            animator.SetFloat(Speed, 1, 0.3f, Time.deltaTime);
            float distance = Vector3.Distance(transform.position, target.position);

            if (health <= 0) {
                _agent.isStopped = true;
                animator.SetTrigger(Die1);
                return;
            }

            RotateToTarget();
            if (audioSource.isPlaying == false) {
                audioSource.PlayOneShot(zombieIdle);
            }

            if (distance <= _agent.stoppingDistance * 1.2) {
                animator.SetFloat(Speed, 0);
                if (Time.time >= _timeOfLastAttack + attackSpeed) {
                    audioSource.PlayOneShot(zombieAttack);
                    _timeOfLastAttack = Time.time;
                    animator.SetTrigger(Attack);
                    playerHealth.TakeDamage(attackDamage);
                  
                }
            }
        }

        private void DespawnAfterAnim(GameObject zombie) {
            if (zombie != null) {
                _zombieSpawner.DespawnZombie(this.gameObject);
            }
        }

        private void RotateToTarget() {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        private void GetReferences() {
            _agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
            audioSource = GetComponent<AudioSource>();
        }

        public void TakeDamage(float damage) {
            Debug.Log(health);
            audioSource.PlayOneShot(zombieDamage);
            health -= damage;
            //move navmesh agent away from player
            _agent.Move(transform.forward * -1);
        }
    }
}
