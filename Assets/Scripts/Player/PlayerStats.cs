using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{


    //all of these are public for now but I should make them private before submission.
    [Header("Character Base Stats.")]
    public float strength = 10;
    public float dexterity = 10;
    public float consitution = 10;
    public float intelligence = 10;
    public float wisdom = 10;
    public float charisma = 10;

    //find a better word then consumable, that's not accurate
    [Header("Character consumable Stats.")]
    public float maxHealth = 10f;
    public float currentHealth = 10f;
    public float maxStamina = 20f;
    public float currentStamina = 20f;
    public float maxMana = 20f;
    public float currentMana = 20f;
    float damageCooldown;
    private bool isDead = false;

    [Header("UI Elements")]
    public Image radialHealthIcon;
    public Image radialStaminaIcon;
    public Image radialManaIcon;
    public Text statText;
    public GameObject hudObject;
    public GameObject deathObject;
    public GameObject damageIndicator;
    public PlayerController controller;

    private void Start()
    {
        StatTextWriting();
    }
    // Update is called once per frame
    void Update()
    {
        // make sure all of the consumable stats are clamped to 0 at minimum and whatever the max is at maximum.
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);

        damageCooldown -= Time.deltaTime;

        HealthChange();
        StaminaChange();
        ManaChange();
        
        if (currentHealth <= 0)
        {
            controller.IsDead();
        }

        //testing for damage. and also allows the overlay for damage.
        if (Input.GetButtonDown("Self Harm"))
        {
            currentHealth--;
            Debug.Log("health is " + currentHealth);
            DamageIndicator();
            damageCooldown = 0.2f;
        }

        // remove the overlay for damage (I'm sure there is a better way to do this, but this is functional. I don't think it adds a lot to each frames update.
        if (damageCooldown <= 0.0f)
        {
            RemoveDamageOverlay();
        }


    }

    public void DealDamageOverTime(float damage)
    {
        //dealing damage for standing on a platform, dosn't workcurrently because of collider issues.
        currentHealth -= damage * Time.deltaTime;
        DamageIndicator();
    }

    void HealthChange()
    {
        float amount = Mathf.Clamp01(currentHealth / maxHealth);
        radialHealthIcon.fillAmount = amount;
    }
    void StaminaChange()
    {
        float amount = Mathf.Clamp01(currentStamina / maxStamina);
        radialStaminaIcon.fillAmount = amount;
    }
    void ManaChange()
    {
        float amount = Mathf.Clamp01(currentMana / maxMana);
        radialManaIcon.fillAmount = amount;
    }

    void StatTextWriting()
    {
        statText.text = "Strength = " + strength + "\nDexterity = " + dexterity + "\nContitution = " + consitution + "\nIntelligence = " + intelligence + "\nWisdom = " + wisdom + "\nCharisma = " + charisma ;
    }

    void DamageIndicator()
    {
        damageIndicator.SetActive(true);
    }

    void RemoveDamageOverlay()
    {
        damageIndicator.SetActive(false);
    }

    public void Respawn()
    {
        controller.RemoveLossUI();        
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentMana = maxMana;        
    }
}
