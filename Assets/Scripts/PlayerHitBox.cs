using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    [SerializeField] BoxCollider2D col;
    [SerializeField] CircleCollider2D enemyCol;

    [SerializeField] Rigidbody2D enemyrb;
    [SerializeField] Rigidbody2D playerrb;

    [SerializeField] float force;
    [SerializeField] float knockbackForce;
    [SerializeField] Vector2 direction;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (col.IsTouching(enemyCol))
        {
            //Debug.Log("EnemyHit");
            Explosion();
        }
    }

    void Explosion()
    {
        Debug.Log("boom?");
            direction = enemyCol.transform.position - transform.position;
            enemyrb.AddForce(direction * force, ForceMode2D.Impulse);
            playerrb.AddForce(-direction * knockbackForce, ForceMode2D.Impulse);
        
    }
}
