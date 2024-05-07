using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] GameObject deathSkull;
    //[SerializeField] TextMeshProUGUI healthValue;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        //healthValue.SetText("PLAYER 1: ", health);
        
    }

    public void DeathIcon()
    {
        Debug.Log("DeathSkull!!");
        Instantiate(deathSkull, this.transform);
    }
}
