using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Search;

public class PlayerInteract : MonoBehaviour
{

    public Camera camera;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask mask;
    private PlayerUI playerUI;
    private PlayerMotor _motor;
    
    // Start is called before the first frame update
    void Start()
    {
        playerUI = GetComponent<PlayerUI>();
        _motor = GetComponent<PlayerMotor>();

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

            if (_motor.getPlayerInput().OnFoot.Interaction.triggered)
            {
                interaction.BasicInteract();   
            }
        }
    }
}
