using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour {
    private NavMeshAgent agent = null;
    private Animator animator = null;
    [SerializeField] private Transform target;

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
        if (distance <= agent.stoppingDistance +0.1f) {
            animator.SetFloat("Speed", 0);
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
    }
}
