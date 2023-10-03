using UnityEngine;
using UnityEngine.AI;
using World;

namespace Zombie {
    public class ZombieController : MonoBehaviour {
        private NavMeshAgent _agent;
        private Animator animator;
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] internal Transform target;
        private ZombieStats _stats;
        private float _timeOfLastAttack = 0f;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private ZombieSpawner _zombieSpawner;
        private static readonly int Die1 = Animator.StringToHash("Die1");

        private void Start() {
            GetReferences();
            _zombieSpawner = GameObject.FindGameObjectWithTag("ZombieSpawner").GetComponent<ZombieSpawner>();
        }

        private void Update() {
            _agent.SetDestination(target.position);
            animator.SetFloat(Speed, 1, 0.3f, Time.deltaTime);
            float distance = Vector3.Distance(transform.position, target.position);

            if (_stats.health <= 0){
                _agent.isStopped = true;
                animator.SetTrigger(Die1);
                return;
            }
            
            RotateToTarget();
        
            if (distance <= _agent.stoppingDistance*1.2) {
                animator.SetFloat(Speed, 0);
                if (Time.time >= _timeOfLastAttack + _stats.attackSpeed) {
                    _timeOfLastAttack = Time.time;
                    animator.SetTrigger(Attack);
                    playerHealth.TakeDamage(_stats.attackDamage);
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
            _stats = GetComponent<ZombieStats>();
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        }
    }
}
