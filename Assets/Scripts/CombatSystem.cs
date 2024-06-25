using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class CombatSystem : MonoBehaviour
{
    /*this class will dictate how the health and damage system will work and interact with each other.
     * Separate from Player or AI so can be used on both - the collision detection will return this type of class.
     */

    //HP & Damage
    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth = 0;

    [SerializeField] HealthUI healthUI;
    [SerializeField] DamageUI damageUI;

    //calculating damage
    double attackPower; // y = f(x)
    [SerializeField] int damageNumber;
    double amplitude;
    [SerializeField] double x; //this actually functions as our timer for damage calculations.  y = f(x)
    float minusx = 4; //when 
    double xUpperBound = 8.343; //this specific number is just when the 2 equations intersect

    //calculating boost
    bool isBoosting = false;
    double energy;
    double b;
    //death explosion?
    
    bool isDead = false;


    public void Start()
    {
        currentHealth = maxHealth;

    }

    public void Update()
    {
        Attack();
        UpdateUI();
        
    }
    /*Inputs attackpoint of enemy bot, then subtracts it from our hp.
     *Is called when someone deals damage to this bot
     */
    public void DealDamage(int hp)
    {  
        
        //Debug.Log("damage taken");
        currentHealth -= hp;

        if (currentHealth < 0)
        {
            healthUI.SetHealth(0);
            healthUI.DeathIcon();
            //Debug.Log("i am dead!!!!");
            isDead = true;           
            enabled = false;
        }
    }

    private void Attack() //attack power increases over time as the spinners spins up.
    {
            x += Time.deltaTime; //increment x
            if (x <= xUpperBound)
            {
                amplitude = Pow(2.5, 0.31 * x);
                attackPower = -amplitude * Cos(3 * PI * x) + Exp(0.4 * x);
                /*composite equation of the exponentially increasing dmg     
                 * Sinusoidal is a fun way to give some randomness (that can be mastered if timing attacks correctly)
                 * Sinusoidal also emulates a spinner's bite colliding with the enemy bot at different intervals
                 * 
                 */
            }
            else
            {
                attackPower = -5.5 * Cos(3 * PI * x) + 31;
                //switches to a straight sine wave after some time so the damage number doesnt increase forever
                //still goes up and down lol
            }
            damageNumber = (int)Round(attackPower);
    }

    //I usesd to have an idea to have a shared energy/stamina system for both the attack power and boost, but that got cut at the end.
    //Do this is currently unused.
    public void SetBoost(bool boostState)
    {
        isBoosting = boostState;
    }
    
    public void Energy()
    {
        if (isBoosting == false)
        {
            b += Time.deltaTime;
            energy = b;

        }
    }

    public int GetDamage() //used by Playerhitbox to take damage number - does a quick conversion too
    {   
        return damageNumber;
    }

    //whenever a collision happens, call this to decrement x, emulating the spinner stopping upon contact.
    public void DecrementDamage() 
    {
        if (x < xUpperBound)
        {
            x = Max(0, x - minusx); //subtracts x timer thing (dictates attackPower) by a specific amount, but never goes to 0;
        }
        else
        {
            x = 2.0f; //once x reaches upper bound, just drop it down severely
        }
       
    }

    public void UpdateUI()
    {
        healthUI.SetHealth(currentHealth);
        damageUI.SetDamage(damageNumber);
    }

    public bool DeathCheck()
    {
        return isDead;
    }

}
