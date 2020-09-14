using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject impactFlashObject;

    float damage;
    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    float knockForce;
    public float KnockForce
    {
        get { return knockForce; }
        set { knockForce = value; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().TakeDamage(damage, this.GetComponent<Rigidbody2D>().velocity.x > 0 ? knockForce : -knockForce, true);
            GameObject impactFlash = Instantiate(impactFlashObject, this.transform.position, this.transform.rotation, other.transform) as GameObject;


            if (transform.localScale.x < 0 && other.transform.localScale.x < 0)
            {
                impactFlash.transform.localScale = new Vector2(impactFlash.transform.localScale.x * -1, impactFlash.transform.localScale.y);
            }

            Destroy(impactFlash, 0.1f);
            Destroy(gameObject);
        }
        else if (other.tag == "Wall")
        {
            GameObject impactFlash = Instantiate(impactFlashObject, this.transform.position, this.transform.rotation, other.transform) as GameObject;

            if (transform.position.x > other.transform.position.x)
            {
                impactFlash.transform.localScale = new Vector2(impactFlash.transform.localScale.x * -1, impactFlash.transform.localScale.y);
            }

            Destroy(impactFlash, 0.1f);
            Destroy(gameObject);
        }
    }

}
