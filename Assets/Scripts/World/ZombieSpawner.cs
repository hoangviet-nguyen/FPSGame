using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace World
{
    public class ZombieSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject zombiePrefab;
        [SerializeField] private Transform player;

        private NavMeshSurface navMeshSurface; // Reference to your NavMeshSurface component

        void Start()
        {
            navMeshSurface = FindObjectOfType<NavMeshSurface>(); // Assuming you have a NavMeshSurface component in your scene
            
            // Initiate 10 zombies in a circle around the point of x60 y15 z60 with a distance of 40 from the point
            for (int i = 0; i < 10; i++)
            {
                float x = 60 + Mathf.Cos(i * Mathf.PI / 5) * 50;
                float y = 15;
                float z = 60 + Mathf.Sin(i * Mathf.PI / 5) * 50;
                
                Vector3 spawnPosition = new Vector3(x, y, z);
                
                // Sample the position to get the correct height on the NavMesh
                NavMeshHit hit;
                if (NavMesh.SamplePosition(spawnPosition, out hit, 10.0f, NavMesh.AllAreas))
                {
                    spawnPosition = hit.position;
                }
                else
                {
                    Debug.LogWarning("Failed to find a valid spawn position on the NavMesh.");
                    continue; // Skip this iteration if no valid position was found
                }
                
                GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
                zombie.transform.parent = transform;
                zombie.GetComponent<ZombieController>().target = player;
            }

            // After spawning the zombies, you may want to bake the NavMesh again to account for any changes.
            //navMeshSurface.BuildNavMesh();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}