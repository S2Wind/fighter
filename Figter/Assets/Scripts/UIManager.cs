using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Health
    [Header("Health")]

    [SerializeField] Text playerHealthText;

    [SerializeField] Text playerHealthAmountText;

    [SerializeField] Text playerMaxHealthText;
    
    //Link
    [Header("Link")]

    [SerializeField] PlayerHealth playerHealth;


    void Start()
    {
        playerHealthAmountText.text = playerHealth.GetHealth().ToString();
        playerMaxHealthText.text = playerHealth.GetMaxHealth().ToString();
    }

    public void SetPlayerHealthAmountText(float amount)
    {
        playerHealthAmountText.text = amount.ToString();
    }
}
