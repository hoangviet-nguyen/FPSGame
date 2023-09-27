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
    private ZombieStats stats;
    private float _timeOfLastAttack = 0f;

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
        
        float distance = Vector3.Distance(transform.position, target.position);
            if (distance <= agent.stoppingDistance) {
                animator.SetFloat("Speed", 0);
                if (Time.time >= _timeOfLastAttack + stats.attackSpeed) {
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
        //playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        //find playerhealth from tagged player
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }
}
