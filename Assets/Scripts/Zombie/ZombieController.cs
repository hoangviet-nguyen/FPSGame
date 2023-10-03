using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour {
    private NavMeshAgent agent = null;
    public Animator animator = null;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] internal Transform target;
    [SerializeField] AudioClip zombieAttack;
    [SerializeField] AudioClip zombieIdle;
    private ZombieStats stats;
    private float _timeOfLastAttack = 0f;
    private AudioSource audioSource;
    private GameObject zombie;

    private void Start() {
        GetReferences();
    }

    private void Update() {
        MoveToTarget();
    }   
    
    private void MoveToTarget() {
        agent.SetDestination(target.position);
        animator.SetFloat("Speed", 1, 0.3f, Time.deltaTime);
        RotateToTarget();
        if (audioSource.isPlaying == false) {
            audioSource.PlayOneShot(zombieIdle);
        }
        
        float distance = Vector3.Distance(transform.position, target.position);
            if (distance <= agent.stoppingDistance*1.2) {
                animator.SetFloat("Speed", 0);
                if (Time.time >= _timeOfLastAttack + stats.attackSpeed) {
                    audioSource.PlayOneShot(zombieAttack);
                    _timeOfLastAttack = Time.time;
                    animator.SetTrigger("Attack");
                    Debug.Log("Player hit");
                    playerHealth.TakeDamage(stats.attackDamage);
                }
            }
        
    }
    
    private void RotateToTarget() {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

    }
    
    private void GetReferences() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        stats = GetComponent<ZombieStats>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        zombie = this.gameObject;
        audioSource = GetComponent<AudioSource>();
    }
}
