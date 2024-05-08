using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DamageUI : MonoBehaviour
{
    [SerializeField] Slider slider;

    public void SetDamage(int damage)
    {
        slider.value = damage;
    }

}
