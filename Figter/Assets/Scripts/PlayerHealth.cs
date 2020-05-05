using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Configs Static")]

    [SerializeField] float health;

    [SerializeField] float maxHealth;

    [Header("System")]

    [SerializeField] UIManager UIManager;

    public float GetHealth() { return health; }

    public float GetMaxHealth() { return maxHealth; }

    void Start()
    {
        if(!PlayerConfigs.GameStart)
        {
            PlayerConfigs.MaxHealth = 3f;
            PlayerConfigs.Health = 3f;
            PlayerConfigs.GameStart = true;
        }
        else
        {
            health = PlayerConfigs.Health;
            maxHealth = PlayerConfigs.MaxHealth;
        }

    }

    public void HealthRegain(float amount)
    {
        health = Mathf.Clamp(health + amount, 0, health);
        UIManager.SetPlayerHealthAmountText(health);
    }    

    public void DamagePlayer(float amount)
    {
        if( health > 0f)
        {
            health -= amount;
            UIManager.SetPlayerHealthAmountText(health);
            if (health <= 0)
            {
                gameObject.GetComponentInParent<Animator>().SetTrigger("isDead");
                gameObject.GetComponent<PlayerControl>().Attitudes = 32;
            }
        }
    }


    public void SaveHealth()
    {
        PlayerConfigs.Health = health;
        PlayerConfigs.MaxHealth = maxHealth;
    }
}

