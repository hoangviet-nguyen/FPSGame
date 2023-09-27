using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World
{
    public class ZombieSpawner : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private GameObject zombiePrefab;
        [SerializeField] private float maxDistance = 20f;
        [SerializeField] private float spawnDelay = 3f;
        [SerializeField] private int zombiesPerWave = 50;
        [SerializeField] private int maxZombiesAlive = 20;

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
        }

        void AddSpawnPoints()
        {
            /*Verschiedene vordefinierte SpawnPoints auf der Map werden zur Liste hinzugefügt
            spawnPoints.Add(new Vector3(68.8519974f, 5.20200014f, 106.237999f));
            spawnPoints.Add(new Vector3(68.8519974f, 5.20200014f, 101.059998f));
            spawnPoints.Add(new Vector3(68.4599991f, 5.57200003f, 92.0899963f));
            spawnPoints.Add(new Vector3(83.9800034f, 4.57999992f, 106.040001f));
            spawnPoints.Add(new Vector3(86.1800003f, 4.89099979f, 98.1999969f));
            spawnPoints.Add(new Vector3(86.1800003f, 5.09000015f, 93.4899979f));
            */
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
                    currentWave++;
                    zombiesPerWave = currentWave + 5;
                    zombiesRemainingInWave = zombiesPerWave;
                }

                if (spawnedZombies.Count < maxZombiesAlive)
                {
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

        void DespawnZombie(GameObject zombie)
        {
            //Sobald implementiert soll beim Tod eines Zombies diese Methode aufgerufen werden
            spawnedZombies.Remove(zombie);
            Destroy(zombie);
        }
    }
}