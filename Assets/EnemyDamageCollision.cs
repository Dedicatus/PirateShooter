using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageCollision : MonoBehaviour
{
    float damage;
    float hitForce;

    private void Start()
    {
        damage = transform.parent.GetComponent<Enemy>().Damage;
        hitForce = transform.parent.GetComponent<Enemy>().Hitforce;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            bool fromRight = (transform.position.x >= collision.transform.position.x) ? true : false;

            collision.GetComponent<Player>().TakeDamage(damage, hitForce, transform.position.x >= collision.transform.position.x);
        }
    }
}
