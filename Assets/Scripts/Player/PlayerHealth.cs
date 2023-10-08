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
    
    void Start()
    {
        health = maxHealth;
        endscreen = GameObject.Find("EndscreenCanvas").GetComponent<Endscreen>();
    }
    
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        if (health <= 0)
        {
            endscreen.Endgame();
        }
    }

    public void UpdateHealthUI()
    {
        var fillFront = frontHealthBar.fillAmount;
        var fillBack = backHealthBar.fillAmount;
        var percentComplete = lerpTimer / chipSpeed;
        var healthFraction = health / maxHealth;
        var takenDamage = fillBack > healthFraction;
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
    }

    public void HealDamage(float heal)
    {
        health += heal;
        lerpTimer = 0f;
    }

    public float getHealth()
    {
        return health;
    }
}
