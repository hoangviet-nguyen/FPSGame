using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string promptMessage;

    public void BasicInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    {
        //This is a template function so here wont be any code
    }
    
   
}
