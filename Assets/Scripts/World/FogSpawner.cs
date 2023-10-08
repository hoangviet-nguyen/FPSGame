using UnityEngine;

public class FogSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fogPrefab;
    
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            float x = 60 + Mathf.Cos(i * Mathf.PI / 5) * 50;
            float y = 15;
            float z = 60 + Mathf.Sin(i * Mathf.PI / 5) * 50;
            GameObject fog = Instantiate(fogPrefab, new Vector3(x, y, z), Quaternion.identity);
            fog.transform.parent = transform;
        }
    }

}
