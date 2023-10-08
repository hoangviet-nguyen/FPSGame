using UnityEngine;
using UnityEngine.AI;
using World;
using Random = UnityEngine.Random;

namespace Zombie {
    public class ZombieController : MonoBehaviour {
        #region Variables
        
        private NavMeshAgent _agent;
        private Animator _animator;
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] internal Transform target;
        [SerializeField] AudioClip zombieAttack;
        [SerializeField] AudioClip zombieIdle;
        [SerializeField] AudioClip zombieDamage;
        private AudioSource _audioSource;
        private float _timeOfLastAttack;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private ZombieSpawner _zombieSpawner;
        private static readonly int Die1 = Animator.StringToHash("Die1");
        
        [SerializeField] public float health;
        [SerializeField] public float attackSpeed;
        [SerializeField] public float attackDamage;
        private float _timeOfLastIdle;
        
        #endregion
        
        private void Start()
        {
            GetReferences();
            _zombieSpawner = GameObject.FindGameObjectWithTag("ZombieSpawner").GetComponent<ZombieSpawner>();
            switch (GameValues.Difficulty)
            {
                case 1:
                    health *= 1f;
                    attackDamage *= 1f;
                    break;
                case 2:
                    health *= 1.5f;
                    attackDamage *= 1.5f;
                    break;
                case 3:
                    health *= 2f;
                    attackDamage *= 2f;
                    break;
                default:
                    health *= 1f;
                    attackDamage *= 1f;
                    break;
            }
        }

        private void Update() {
            _agent.SetDestination(target.position);
            _animator.SetFloat(Speed, 1, 0.3f, Time.deltaTime);
            var distance = Vector3.Distance(transform.position, target.position);

            if (health <= 0) {
                _agent.isStopped = true;
                _animator.SetTrigger(Die1);
                return;
            }

            RotateToTarget();
            if (Time.time >= _timeOfLastIdle + Random.Range(6, 16)) {
                _audioSource.PlayOneShot(zombieIdle);
                _timeOfLastIdle = Time.time;
            }

            if (!(distance <= _agent.stoppingDistance * 1.2)) return;
            _animator.SetFloat(Speed, 0);
            if (!(Time.time >= _timeOfLastAttack + attackSpeed)) return;
            _audioSource.PlayOneShot(zombieAttack);
            _timeOfLastAttack = Time.time;
            _animator.SetTrigger(Attack);
            playerHealth.TakeDamage(attackDamage);
        }
        
        private void DespawnAfterAnim(GameObject zombie) {
            if (zombie != null) {
                _zombieSpawner.DespawnZombie(this.gameObject);
            }
        }

        private void RotateToTarget() {
            var direction = (target.position - transform.position).normalized;
            var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        private void GetReferences() {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
            _audioSource = GetComponent<AudioSource>();
        }

        public void TakeDamage(float damage) {
            //dont play sound if audio not initialized
            if (_audioSource != null)
            {
                _audioSource.PlayOneShot(zombieDamage);
            }
            
            health -= damage;
            //move navmesh agent away from player
            _agent.Move(transform.forward * -1);
        }
    }
}
