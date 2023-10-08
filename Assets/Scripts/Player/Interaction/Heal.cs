using System.Collections;
using UnityEngine;

public class Heal : Interactable
{
    private void Awake()
    {
        setPromptMessage("Heal");
    }

    protected override void Interact(PlayerInteract player)
    {
        StartCoroutine(HealPlayer(player.GetComponent<PlayerHealth>()));
    }

    private IEnumerator HealPlayer(PlayerHealth health)
    {
        if (health.getHealth() < health.maxHealth)
        {
            health.HealDamage(20);
            DestroyImmediate(gameObject); 
        }
        else
        {
           setPromptMessage("Already max health");
           yield return new WaitForSecondsRealtime(1);
           setPromptMessage("Heal");
        }
    }
}
