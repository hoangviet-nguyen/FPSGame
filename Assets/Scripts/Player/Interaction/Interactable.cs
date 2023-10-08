using System;
using UnityEngine;
public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private bool useEvents;
    private bool destroy;
    private string promptMessage;
    

    public void BasicInteract(PlayerInteract player)
    {
        if (useEvents)
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        Interact(player);
    }

    protected virtual void Interact(PlayerInteract player)
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

    public void setPromptMessage(string message)
    {
        promptMessage = message;
    }

    public string getPromptmessage()
    {
        return promptMessage;
    }
}
