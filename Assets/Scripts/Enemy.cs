using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    GameObject target;
    Rigidbody2D m_Rigidbody;
    Animator m_Animator;
    Collider2D[] m_Colliders;


    [SerializeField] float speed = 0.6f;
    [SerializeField] float health = 3.0f;
    [SerializeField] float damage = 1.0f;
    [SerializeField] float hitForce = 1.0f;
    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }
    
    public float Hitforce
    {
        get { return hitForce; }
        set { hitForce = value; }
    }

    static bool grounded;
    bool ground;
    bool isFollowing;
    bool isDead;
    float knockTime = 0.2f;
    float knockTimer;
    Ray2D ray;
    RaycastHit2D hit;

    void Start()
    {
        target = GameObject.FindWithTag("Player");
        m_Rigidbody = this.GetComponent<Rigidbody2D>();
        m_Animator = this.GetComponent<Animator>();
        m_Colliders = this.GetComponents<Collider2D>();
        Physics2D.IgnoreCollision(m_Colliders[0], target.GetComponent<Collider2D>());
    }

    void Update()
    {
        FollowTarget();

        if (knockTimer > 0f)
        {
            knockTimer -= Time.deltaTime;
        }
        else
        {
            knockTimer = 0f;
            if (ground) isFollowing = true;
        }

        if (isDead)
        {
            if (m_Rigidbody.velocity.y == 0f)
            {
                m_Rigidbody.bodyType = RigidbodyType2D.Static;
                m_Colliders[0].enabled = false;
            }
        }
    }

    void FixedUpdate()
    {
        GroundDetection();
        ground = grounded;
    }

    void GroundDetection()
    {
        hit = Physics2D.Raycast(GameObject.Find("Scout_Feet").transform.position, Vector2.down);
        if (hit.distance < 0.03)
        {
            grounded = true;
        }
        if (hit.distance > 0.03)
        {
            grounded = false;
        }
    }

    private void FollowTarget()
    {
        if (!isFollowing || isDead) return;
        if (this.transform.position.x <= target.transform.position.x)
        {
            if (Math.Abs(transform.localScale.x) > 1)
            {
                transform.localScale = new Vector2(Math.Abs(transform.localScale.x) * Vector2.right.x, transform.localScale.y);
            }
            m_Rigidbody.velocity = new Vector2(speed, m_Rigidbody.velocity.y);
            m_Animator.SetFloat("Speed", speed);
        }
        else
        {
            if (Math.Abs(transform.localScale.x) > 1)
            {
                transform.localScale = new Vector2(Math.Abs(transform.localScale.x) * Vector2.left.x, transform.localScale.y);
            }
            m_Rigidbody.velocity = new Vector2(-speed, m_Rigidbody.velocity.y);
            m_Animator.SetFloat("Speed", speed);
        }
    }

    public void TakeDamage(float amount, float force)
    {
        StartCoroutine(Sleep(0.01f));
        health -= amount;
        m_Animator.SetTrigger("Hit");

        if (grounded)
        {
            isFollowing = false;
            knockTimer = knockTime;
            m_Rigidbody.AddForce(new Vector2(force, force), ForceMode2D.Impulse);
        }

        if (health <= 0f)
        {
            isFollowing = false;
            knockTimer = knockTime;
            m_Animator.SetTrigger("Faint");
            isDead = true;
            m_Colliders[1].enabled = false;
            m_Colliders[0].offset = new Vector2(m_Colliders[0].offset.x, -0.025f);
            m_Rigidbody.AddForce(new Vector2(0.8f, 0.7f), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(collision.collider, m_Colliders[0]);
        }
    }

    private IEnumerator Sleep(float duration)
    {

        float elapsed = 0f;

        while (elapsed < duration)
        {
            Time.timeScale = 0.1f;

            elapsed += Time.deltaTime * (1/ Time.timeScale);

            yield return null;
        }

        Time.timeScale = 1.0f;
    }
}
