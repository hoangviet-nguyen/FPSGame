using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStats : MonoBehaviour {
    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;

    [SerializeField] private bool canAttack;

    private bool isDead = false;

    public void DealDamage() {
        //Damaging functionality
    }

    public void Die() {
        isDead = true;
        Destroy(gameObject);
    }
    
    
}
