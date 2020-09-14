using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float damage = 2.0f;
    [SerializeField] float knockForceMin = 0.8f;
    [SerializeField] float knockForceMax = 1.2f;

    [SerializeField] GameObject smokeObject;
    Animator m_Animator;
    AudioManager m_AudioManager;

    void Start()
    {
        m_Animator = this.GetComponent<Animator>();
        Destroy(gameObject, m_Animator.GetCurrentAnimatorStateInfo(0).length);
        m_AudioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        m_AudioManager.PlayExplose();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        float knockForce = Random.Range(knockForceMin, knockForceMax);

        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().TakeDamage(damage, transform.position.x < other.transform.position.x ? knockForce : -knockForce, true);
        }
    }

    void OnDestroy()
    {
        GameObject smoke = Instantiate(smokeObject, transform.position, transform.rotation) as GameObject;
    }
}
