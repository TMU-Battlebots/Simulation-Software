using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    [SerializeField] CombatSystem enemyBot;
    CombatSystem playerBot;
    [SerializeField] BoxCollider2D col; //us WAIT THIS SINT YUSED AT ALL WHAT THE FUCK
    [SerializeField] CircleCollider2D enemyCol; //them

    [SerializeField] Rigidbody2D enemyrb;
    [SerializeField] Rigidbody2D playerrb;

    [SerializeField] float force;
    [SerializeField] float knockbackForce;
    Vector2 direction;

    int damage;

    void Start()
    {
        playerBot = GetComponentInParent<CombatSystem>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        damage = playerBot.GetDamage(); //gets damage from our bot and dishes it to enemys hp.
        SetKnockBacks(damage);
    }

    private void OnTriggerEnter2D(Collider2D collision) //for the
    {
        
        //enemyBot = collision.GetComponent<CombatSystem>(); 
        if (collision.gameObject.tag == "Player")
        {
            Explosion();
            
            enemyBot.DealDamage(damage);
            playerBot.DecrementDamage();
        }
    }

    void Explosion()
    {
            //Debug.Log("boom?");
            direction = enemyCol.transform.position - transform.position;
            enemyrb.AddForce(direction * force * Time.deltaTime, ForceMode2D.Impulse);
            playerrb.AddForce(-direction * knockbackForce * Time.deltaTime, ForceMode2D.Impulse);
        
    }

    public void SetKnockBacks(float attforce) //scales the knockback force to damage.
    {
        force = attforce * (25000.0f / 40.0f) + 25000.0f;
        // Ensure the transformed value is within the desired output range
        force = Mathf.Clamp(force, 25000.0f, 50000.0f);
        knockbackForce = force / 2 + 10000.0f;
    }
}
