using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    private float health;
    private float lerpTimer;
    private BoxCollider zombie;
    public float maxHealth = 100;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;
    public Endscreen endscreen;
    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        endscreen = GameObject.Find("EndscreenCanvas").GetComponent<Endscreen>();
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(10);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            HealDamage(20);
        }

        if (health <= 0)
        {
            Debug.Log("Player dead");

            endscreen.Endgame();
        }
    }

    public void UpdateHealthUI()
    {
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float percentComplete = lerpTimer / chipSpeed;
        float healthFraction = health / maxHealth;
        bool takenDamage = fillBack > healthFraction;
        lerpTimer += Time.deltaTime;
        
        if (takenDamage)
        {
            frontHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.red;
            percentComplete *= percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
        }
        else
        {
            backHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.green;
            percentComplete *= percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, backHealthBar.fillAmount, percentComplete);
        }
    }
    
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        //Debug.Log("damage");
    }

    public void HealDamage(float heal)
    {
        health += heal;
        lerpTimer = 0f;
    }
}
