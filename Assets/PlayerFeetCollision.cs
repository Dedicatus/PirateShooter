using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeetCollision : MonoBehaviour
{
    AudioManager m_AudioManager;

    [SerializeField] float jumpOnHeadSpeed = 5.0f;

    private void Start()
    {
        m_AudioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyHead")
        {
            m_AudioManager.PlayJumpOnEnemy();
            if (transform.position.y > collision.transform.position.y)
            {
                transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.parent.GetComponent<Rigidbody2D>().velocity.x, Vector2.up.y * jumpOnHeadSpeed);
                collision.transform.parent.GetComponent<Enemy>().TakeDamage(1.0f, 0f, false);
            }
        }
    }
}
