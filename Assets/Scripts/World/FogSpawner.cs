using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fogPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        //initiate 10 objects of type fog in a circle around the point of x60 y15 z60 with a distance of 40 from the  point
        for (int i = 0; i < 40; i++)
        {
            float x = 60 + Mathf.Cos(i * Mathf.PI / 5) * 50;
            float y = 15;
            float z = 60 + Mathf.Sin(i * Mathf.PI / 5) * 50;
            GameObject fog = Instantiate(fogPrefab, new Vector3(x, y, z), Quaternion.identity);
            fog.transform.parent = transform;
        }

        
  



    }


}
