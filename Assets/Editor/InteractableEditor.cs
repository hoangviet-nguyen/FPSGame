using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Interactable), true)]
public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Interactable interactable = (Interactable)target;
        base.OnInspectorGUI();
        
        if (target.GetType() == typeof(EventOnly))
        {
            
            EditorGUILayout.HelpBox("Event Only Interactable, can ONLY use UnityEvents",MessageType.Info);

            if (interactable.GetComponent<InteractionEvent>() == null)
            {
                interactable.setEvent(true);
                interactable.gameObject.AddComponent<InteractionEvent>();
            }
        }
        
        if (interactable.getEvents() && interactable.GetComponent<InteractionEvent>() == null)
        { 
            Debug.Log("Created");
            interactable.gameObject.AddComponent<InteractionEvent>();
        }
        else if (!interactable.getEvents() && interactable.Destroy() && interactable.GetComponent<InteractionEvent>() != null)
        {
            DestroyImmediate(interactable.GetComponent<InteractionEvent>());
        }
    }
}
