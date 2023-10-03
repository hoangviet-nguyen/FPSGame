using UnityEngine;
using World;

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
            health -= damage;
            Debug.Log(health);
        }
    }
}
