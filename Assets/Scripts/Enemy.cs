using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    AudioManager m_AudioManager;

    GameObject target;
    Rigidbody2D m_Rigidbody;
    Animator m_Animator;
    Collider2D[] m_Colliders;

    [Header("Variables")]
    [SerializeField] float speed = 0.6f;
    [SerializeField] float health = 3.0f;
    [SerializeField] float damage = 1.0f;
    [SerializeField] float hitForce = 1.0f;
    [SerializeField] float explosionChance = 1.0f;

    [Header("Prefabs")]
    [SerializeField] GameObject explosionObject;
    [SerializeField] Sprite faintedSprite;

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
    bool isMoving;
    bool faceRight;
    bool isDead;
    float knockTime = 0.2f;
    float knockTimer;
    float turnCD = 0.3f;
    float turnTimer;
    Ray2D ray;
    RaycastHit2D hit;

    void Start()
    {
        target = GameObject.FindWithTag("Player");
        m_Rigidbody = this.GetComponent<Rigidbody2D>();
        m_Animator = this.GetComponent<Animator>();
        m_Colliders = this.GetComponents<Collider2D>();
        Physics2D.IgnoreCollision(m_Colliders[0], target.GetComponent<Collider2D>());
        //Physics2D.IgnoreCollision(m_Colliders[2], target.GetComponent<Collider2D>());
        m_AudioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        faceRight = false;
    }

    void Update()
    {
        MoveForward();

        if (knockTimer > 0f)
        {
            knockTimer -= Time.deltaTime;
        }
        else
        {
            knockTimer = 0f;
            if (ground) isMoving = true;
        }

        if (turnTimer > 0f)
        {
            turnTimer -= Time.deltaTime;
        }
        else
        {
            turnTimer = 0f;
        }

        if (isDead)
        {
            if (m_Rigidbody.velocity.y == 0f)
            {
                m_Rigidbody.bodyType = RigidbodyType2D.Static;
                m_Colliders[0].enabled = false;
                m_Colliders[2].enabled = false;
            }
        }

        //if (isDead && !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Scout_Faint"))
        //{
        //    m_Animator.SetTrigger("Faint");
        //}
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
        if (!isMoving || isDead) return;
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
        m_Animator.SetFloat("Speed", speed);
    }

    private void MoveForward()
    {
        if (!isMoving || isDead) return;
        if (faceRight)
        {
            transform.localScale = new Vector2(Math.Abs(transform.localScale.x) * Vector2.right.x, transform.localScale.y);
            m_Rigidbody.velocity = new Vector2(speed, m_Rigidbody.velocity.y);
        }
        else
        {
            transform.localScale = new Vector2(Math.Abs(transform.localScale.x) * Vector2.left.x, transform.localScale.y);
            m_Rigidbody.velocity = new Vector2(-speed, m_Rigidbody.velocity.y);
        }
        m_Animator.SetFloat("Speed", speed);
    }

    public void TakeDamage(float amount, float force, bool isBullet)
    {
        StartCoroutine(Sleep(0.01f));
        health -= amount;
        m_Animator.SetTrigger("Hit");

        if (grounded)
        {
            isMoving = false;
            knockTimer = knockTime;
            m_Rigidbody.AddForce(new Vector2(force, Math.Abs(force)), ForceMode2D.Impulse);
        }

        if (health <= 0f && !isDead)
        {
            if (UnityEngine.Random.Range(0f, 1.0f) <= explosionChance && isBullet)
            {
                GameObject explosion = Instantiate(explosionObject, transform.position, transform.rotation) as GameObject;
                foreach (Collider2D coll in m_Colliders)
                {
                    Physics2D.IgnoreCollision(coll, explosion.GetComponent<Collider2D>());
                }
            }
            isDead = true;
            isMoving = false;
            knockTimer = knockTime;
            //m_Animator.SetTrigger("Faint");
            m_Animator.enabled = false;
            this.GetComponent<SpriteRenderer>().sprite = faintedSprite;
            this.GetComponent<SpriteRenderer>().color = Color.white;
            m_Colliders[0].enabled = false;
            m_Colliders[1].enabled = false;
            m_Colliders[2].enabled = true;
            m_Colliders[2].offset = new Vector2(m_Colliders[2].offset.x, -0.025f);
            Physics2D.IgnoreCollision(m_Colliders[2], target.GetComponent<Collider2D>());
            float faintForce = UnityEngine.Random.Range(0.3f, 0.6f);
            m_Rigidbody.AddForce(new Vector2(faintForce * force >= 0 ? 1.0f : -1.0f, faintForce * 0.7f), ForceMode2D.Impulse);
            transform.Find("Head").gameObject.SetActive(false);
            transform.Find("Sword").gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(collision.collider, m_Colliders[0]);
            Physics2D.IgnoreCollision(collision.collider, m_Colliders[2]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyWall" && knockTimer <= 0f)
        {
            if (turnTimer <= 0f)
            {
                turnTimer = turnCD;
                faceRight = !faceRight;
            }
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
