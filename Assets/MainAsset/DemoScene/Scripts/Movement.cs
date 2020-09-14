using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour{
	AudioManager m_AudioManager;
	public bool canDoubleJump = false;
	public static bool facingRight = true;

	public static float curVelocityX;

	public KeyCode jumpKey = KeyCode.Space;
	public KeyCode leftKey = KeyCode.A;
	public KeyCode rightKey = KeyCode.D;
	public static float timer = 5.0f;

    void Start()
    {
		m_AudioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
	}
    public void Motion (float speed, float jump, Rigidbody2D rdbdy, bool grounded, bool isScout,bool isShooting, Transform sprite){
		if (Input.GetKeyDown (jumpKey)) {
			if(grounded){
			rdbdy.velocity = new Vector2 ( rdbdy.velocity.x, jump);
				m_AudioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
				m_AudioManager.PlayJump();
			}

			if (grounded && isScout) {
				rdbdy.velocity = new Vector2 (rdbdy.velocity.x, jump);
				m_AudioManager.PlayJump();
				canDoubleJump = true;
			} else {
				if(canDoubleJump){
					canDoubleJump = false;
					m_AudioManager.PlayJump();
					rdbdy.velocity = new Vector2 (rdbdy.velocity.x, (jump - 0.6f));
				}
			
			}
		}

		if (Input.GetKey(leftKey))
		{
			if (!isShooting)
			{
				if (sprite.localScale.x > 1)
				{
					sprite.localScale = new Vector3(sprite.localScale.x * -1, sprite.localScale.y, sprite.localScale.z);
				}

				facingRight = false;
			}
			rdbdy.velocity = new Vector2(-speed, rdbdy.velocity.y);
			curVelocityX = -speed;
		}
		else if (Input.GetKey(rightKey))
		{
			if (!isShooting)
			{
				if (sprite.localScale.x < 1)
				{
					sprite.localScale = new Vector3(sprite.localScale.x * -1, sprite.localScale.y, sprite.localScale.z);
				}
				facingRight = true;
			}
			rdbdy.velocity = new Vector2(speed, rdbdy.velocity.y);
			curVelocityX = speed;
		}
		else
		{
			curVelocityX = 0f;
		}
	}


}