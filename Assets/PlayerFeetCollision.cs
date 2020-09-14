using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeetCollision : MonoBehaviour
{
    [SerializeField] float jumpOnHeadSpeed = 5.0f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyHead")
        {
            if (transform.position.y > collision.transform.position.y)
            {
                transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.parent.GetComponent<Rigidbody2D>().velocity.x, Vector2.up.y * jumpOnHeadSpeed);
                //transform.parent.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpOnHeadForce, ForceMode2D.Impulse);
                collision.transform.parent.GetComponent<Enemy>().TakeDamage(1.0f, 0f);
            }
        }
    }
}
