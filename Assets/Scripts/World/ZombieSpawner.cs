using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zombie;

namespace World
{
    public class ZombieSpawner : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private GameObject zombiePrefab;
        [SerializeField] private GameObject zombiePrefab2;
        [SerializeField] private GameObject zombiePrefab3;
        [SerializeField] private float maxDistance = 20f;
        [SerializeField] private float spawnDelay = 3f;
        private int _zombiesPerWave = 5;
        private int _maxZombiesAlive = 3;
        [SerializeField] private TMP_Text waveText;

        private  List<Vector3> _spawnPoints = new List<Vector3>();
        private List<GameObject> spawnedZombies = new List<GameObject>();
        private int _currentWave = 1;
        private int _zombiesRemainingInWave;
        private int _waveLength = 5;

        void Start()
        {
            zombiePrefab.GetComponent<ZombieController>().target = player;
            zombiePrefab2.GetComponent<ZombieController>().target = player;
            zombiePrefab3.GetComponent<ZombieController>().target = player;
            AddSpawnPoints();
            _zombiesRemainingInWave = _zombiesPerWave;
            StartCoroutine(SpawnZombiesWithDelay());
            waveText.text = "Wave " + _currentWave;

            switch (GameValues.Difficulty)
            {
                case 1:
                    _maxZombiesAlive = 7;
                    _zombiesPerWave = 15;
                    break;
                case 2:
                    _maxZombiesAlive = 15;
                    _zombiesPerWave = 35;
                    break;
                case 3:
                    _maxZombiesAlive = 25;
                    _zombiesPerWave = 50;
                    break;
                default:
                    _maxZombiesAlive = 7;
                    _zombiesPerWave = 15;
                    break;
                    
            }

            switch (GameValues.WaveLength)
            {
                case 1:
                    _waveLength = 4;
                    break;
                case 2:
                    _waveLength = 7;
                    break;
                case 3:
                    _waveLength = 10;
                    break;
                default:
                    _waveLength = 5;
                    break;
            }
            
            Debug.Log("Difficulty: " + GameValues.Difficulty);
            Debug.Log("Wave length: " + GameValues.WaveLength);
            
        }

        void AddSpawnPoints()
        {

            GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");

            foreach (GameObject spawnPointObject in spawnPointObjects)
            {
                _spawnPoints.Add(spawnPointObject.transform.position);
            }
        }

        IEnumerator SpawnZombiesWithDelay()
        {
            while (true)
            {
                if (_zombiesRemainingInWave <= 0)
                {
                    //wait 5 seconds before starting the next wave
                    yield return new WaitForSeconds(5f);
                    _currentWave++;
                    _zombiesPerWave = _currentWave + 5;
                    waveText.text = "Wave " + _currentWave + "\n (" + spawnedZombies.Count + " alive)";
                    _zombiesRemainingInWave = _zombiesPerWave;
                    Debug.Log("Wave " + _currentWave + " started!");
                }

                if (spawnedZombies.Count < _maxZombiesAlive)
                {   
                    Debug.Log("Spawning a zombie...");
                    Vector3 closestSpawnPoint = Vector3.zero;
                    float closestDistance = maxDistance;

                    foreach (Vector3 spawnPoint in _spawnPoints)
                    {
                        float distanceToPlayer = Vector3.Distance(player.position, spawnPoint);
                        if (distanceToPlayer <= maxDistance && distanceToPlayer < closestDistance)
                        {
                            closestSpawnPoint = spawnPoint;
                            closestDistance = distanceToPlayer;
                        }
                    }

                    if (closestSpawnPoint != Vector3.zero && _zombiesRemainingInWave > 0)
                    {
                        GameObject newZombie = SpawnZombie(closestSpawnPoint);
                        _zombiesRemainingInWave--;
                        waveText.text = "Wave " + _currentWave + "\n (" + spawnedZombies.Count + " alive)";
                    }
                }

                if (_currentWave >= _waveLength)
                {
                    Debug.Log("All waves complete.");
                    yield break;
                }

                yield return new WaitForSeconds(spawnDelay);
            }
        }

        GameObject SpawnZombie(Vector3 spawnPosition)
        {
            //randomly choose a zombie prefab
            GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
            int randomZombie = Random.Range(0, 3);
            switch (randomZombie)
            {
                case 0:
                    zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
                    break;
                case 1:
                    zombie = Instantiate(zombiePrefab2, spawnPosition, Quaternion.identity);
                    break;
                case 2:
                    zombie = Instantiate(zombiePrefab3, spawnPosition, Quaternion.identity);
                    break;
            }
            
            spawnedZombies.Add(zombie);
            Debug.Log("Spawned a zombie at " + spawnPosition + "!");
            return zombie;
        }

        public void DespawnZombie(GameObject zombie)
        {
            spawnedZombies.Remove(zombie);
            Destroy(zombie);
            waveText.text = "Wave " + _currentWave + "\n (" + spawnedZombies.Count + " alive)";

        }

    }
}