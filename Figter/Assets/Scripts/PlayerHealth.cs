using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Configs Static")]

    [SerializeField] float health;

    [SerializeField] float maxHealth;

    [SerializeField] float timeToUntouchable;

    [Header("System")]

    [SerializeField] UIManager UIManager;

    bool touchable;

    public bool Touchable { get => touchable; set => touchable = value; }

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
        Touchable = true;
        timeToUntouchable = 1f;
    }

    public void HealthRegain(float amount)
    {
        health = Mathf.Clamp(health + amount, 0, health);
        UIManager.SetPlayerHealthAmountText(health);
    }    

    public void DamagePlayer(float amount)
    {
        if (Touchable)
        {
            if (health > 0f)
            {
                health -= amount;
                UIManager.SetPlayerHealthAmountText(health);
                if (health <= 0)
                {
                    gameObject.GetComponentInParent<Animator>().SetTrigger("isDead");
                    gameObject.GetComponent<PlayerControl>().Attitudes = 32;
                }
                StartCoroutine(UntouchableTime());
            }
        }
        
    }

    IEnumerator UntouchableTime()
    {
        yield return new WaitForSeconds(timeToUntouchable);
        Touchable = true;
    }


    public void SaveHealth()
    {
        PlayerConfigs.Health = health;
        PlayerConfigs.MaxHealth = maxHealth;
    }
}

