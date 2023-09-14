using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Search;

public class PlayerInteract : MonoBehaviour
{

    private Camera camera;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask mask;
    
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<PlayerLook>().cam;
    }

    // Update is called once per frame
    void Update()
    {
        //Creating a ray at the center of the camera shooting outwards
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo; // stores information if something is hit
        bool hit = Physics.Raycast(ray, out hitInfo, mask);
        var interaction = ExtractInteractable(hitInfo);

        if (hit && interaction != null)
        {
            interaction.BasicInteract();
        }
    }

    private Interactable ExtractInteractable(RaycastHit hitInfo)
    {   
        return hitInfo.collider != null ? hitInfo.collider.GetComponent<Interactable>(): null;
    }
}
