using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Search;

public class PlayerInteract : MonoBehaviour
{

    private Camera camera;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask mask;
    private PlayerUI playerUI;
    private InputManager inputManager;
    
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Creating a ray at the center of the camera shooting outwards
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo; // stores information if something is hit
        bool hit = Physics.Raycast(ray, out hitInfo, mask);
        playerUI.UpdateText(string.Empty);

        if (hit && hitInfo.collider.GetComponent<Interactable>() != null)
        {
            var interaction = hitInfo.collider.GetComponent<Interactable>();
            playerUI.UpdateText(hitInfo.collider.GetComponent<Interactable>().promptMessage);

            if (inputManager.onFoot.Interaction.triggered)
            {
                interaction.BasicInteract();   
            }
        }
    }
}
