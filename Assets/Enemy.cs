using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    GameObject target;
    Rigidbody2D m_Rigidbody;

    [SerializeField] float speed = 0.6f;
    [SerializeField] float health = 3.0f;

    void Start()
    {
        target = GameObject.FindWithTag("Player");
        m_Rigidbody = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        if (this.transform.position.x <= target.transform.position.x)
        {
            if (Math.Abs(transform.localScale.x) > 1)
            {
                transform.localScale = new Vector2(Math.Abs(transform.localScale.x) * Vector2.right.x, transform.localScale.y);
            }
            m_Rigidbody.velocity = new Vector2(speed, m_Rigidbody.velocity.y);
        }
        else
        {
            if (Math.Abs(transform.localScale.x) > 1)
            {
                transform.localScale = new Vector2(Math.Abs(transform.localScale.x) * Vector2.left.x, transform.localScale.y);
            }
            m_Rigidbody.velocity = new Vector2(-speed, m_Rigidbody.velocity.y);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //hurt player
        }
    }
}
