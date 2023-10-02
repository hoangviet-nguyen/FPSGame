using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace World
{
    public class ZombieSpawner : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private GameObject zombiePrefab;
        [SerializeField] private float maxDistance = 20f;
        [SerializeField] private float spawnDelay = 3f;
        [SerializeField] private int zombiesPerWave = 5;
        [SerializeField] private int maxZombiesAlive = 3;
        [SerializeField] private TMP_Text waveText;

        private List<Vector3> spawnPoints = new List<Vector3>();
        private List<GameObject> spawnedZombies = new List<GameObject>();
        private int currentWave = 1;
        private int zombiesRemainingInWave;

        void Start()
        {
            zombiePrefab.GetComponent<ZombieController>().target = player;
            AddSpawnPoints();
            zombiesRemainingInWave = zombiesPerWave;
            StartCoroutine(SpawnZombiesWithDelay());
            waveText.text = "Wave " + currentWave;
        }

        void AddSpawnPoints()
        {

            GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");

            foreach (GameObject spawnPointObject in spawnPointObjects)
            {
                spawnPoints.Add(spawnPointObject.transform.position);
            }
        }

        IEnumerator SpawnZombiesWithDelay()
        {
            while (true)
            {
                if (zombiesRemainingInWave <= 0)
                {
                    //wait 5 seconds before starting the next wave
                    yield return new WaitForSeconds(5f);
                    currentWave++;
                    zombiesPerWave = currentWave + 5;
                    waveText.text = "Wave " + currentWave + "\n (" + spawnedZombies.Count + " alive)";
                    zombiesRemainingInWave = zombiesPerWave;
                    Debug.Log("Wave " + currentWave + " started!");
                }

                if (spawnedZombies.Count < maxZombiesAlive)
                {   
                    Debug.Log("Spawning a zombie...");
                    Vector3 closestSpawnPoint = Vector3.zero;
                    float closestDistance = maxDistance;

                    foreach (Vector3 spawnPoint in spawnPoints)
                    {
                        float distanceToPlayer = Vector3.Distance(player.position, spawnPoint);
                        if (distanceToPlayer <= maxDistance && distanceToPlayer < closestDistance)
                        {
                            closestSpawnPoint = spawnPoint;
                            closestDistance = distanceToPlayer;
                        }
                    }

                    if (closestSpawnPoint != Vector3.zero && zombiesRemainingInWave > 0)
                    {
                        GameObject newZombie = SpawnZombie(closestSpawnPoint);
                        zombiesRemainingInWave--;
                        waveText.text = "Wave " + currentWave + "\n (" + spawnedZombies.Count + " alive)";
                    }
                }

                if (currentWave > 5)
                {
                    Debug.Log("All waves complete.");
                    yield break;
                }

                yield return new WaitForSeconds(spawnDelay);
            }
        }

        GameObject SpawnZombie(Vector3 spawnPosition)
        {
            GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
            spawnedZombies.Add(zombie);
            Debug.Log("Spawned a zombie at " + spawnPosition + "!");
            return zombie;
        }

        public void DespawnZombie(GameObject zombie)
        {
            spawnedZombies.Remove(zombie);
            Destroy(zombie);
            waveText.text = "Wave " + currentWave + "\n (" + spawnedZombies.Count + " alive)";

        }

    }
}