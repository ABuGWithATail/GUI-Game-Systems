using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialStats : MonoBehaviour
{

    private PlayerStats playerStats;
    public Image radialHealthIcon;
    public float curHealth;
    public float maxHealth;

    public Image radialStaminaIcon;
    public float curStamina;
    public float maxStamina;

    public Image radialManaIcon;
    public float curMana;
    public float maxMana;

    private void Start()
    {        
        maxHealth = playerStats.maxHealth;
        maxStamina = playerStats.maxStamina;
        maxMana = playerStats.maxMana;
    }
    // Update is called once per frame
    void Update()
    {
        curHealth = playerStats.currentHealth;
        curStamina = playerStats.currentStamina;
        curMana = playerStats.currentMana;
        HealthChange();
        StaminaChange();
        ManaChange();
    }
    void HealthChange()
    {
        float amount = Mathf.Clamp01(curHealth/maxHealth);
        radialHealthIcon.fillAmount = amount;
    }
    void StaminaChange()
    {
        float amount = Mathf.Clamp01(curStamina/maxStamina);
        radialStaminaIcon.fillAmount = amount;
    }
    void ManaChange()
    {
        float amount = Mathf.Clamp01(curMana/maxMana);
        radialManaIcon.fillAmount = amount;
    }
}
