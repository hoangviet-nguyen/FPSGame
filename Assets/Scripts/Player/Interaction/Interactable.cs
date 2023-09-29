using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private bool useEvents;
    private bool destroy;
    public string promptMessage;

    public void BasicInteract()
    {
        if (useEvents)
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        Interact();
    }

    protected virtual void Interact()
    {
        //This is a template function so here wont be any code
    }

    public bool getEvents()
    {
        return useEvents;
    }

    public void setEvent(bool setting)
    {
        useEvents = setting;
    }

    public bool Destroy()
    {
        return destroy;
    }
}
