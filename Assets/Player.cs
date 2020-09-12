using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public Transform sprite;
	// Use this for initialization
	Animator anim;
	//Speed and jump vary between characters
	[SerializeField] bool spy_spawn;
	[SerializeField] float Speed = 1.3f;
	[SerializeField] float Jump = 3.0f;
	[SerializeField] float bulletDamage = 1.0f;
	[SerializeField] float shootCD = 0.2f;
	float shootTimer = 0f;

	static bool grounded;
	bool ground;
	[SerializeField] bool Scout = true;
	[SerializeField] Rigidbody2D rigid;
	Movement Move = new Movement ();

	Ray2D ray;
	RaycastHit2D hit;

	[SerializeField] GameObject gunPoint;
	[SerializeField] GameObject bulletObject;

	void Start () {
		anim = GetComponent<Animator>();
	}
	void FixedUpdate(){
		GroundDetection ();
		ground = grounded;
		if (shootTimer > 0f)
		{
			shootTimer -= Time.deltaTime;
		}
	}

	// Update is called once per frame
	void Update () {
		anim.SetFloat ("Speed", Mathf.Abs(rigid.velocity.x));
		anim.SetBool ("touchingGround", grounded);

		Move.Motion(Speed, Jump, rigid, grounded, Scout,sprite);

		if(Input.GetButton("Fire1")){
			Shooting ();
		}

	}

	public void GroundDetection(){
		hit = Physics2D.Raycast (GameObject.Find("Spy_Feet").transform.position, Vector2.down);
		if (hit.distance < 0.03){
			grounded = true;
		}
		if(hit.distance > 0.03){
			grounded = false;
		}
	}
	public void Shooting(){
		if (shootTimer <= 0)
		{
			GameObject bullet = Instantiate(bulletObject, gunPoint.transform.position, gunPoint.transform.rotation) as GameObject;
			bullet.GetComponent<Bullet>().Damage = bulletDamage;
			Destroy(bullet, 0.8f);
			if (Movement.facingRight)
			{
				bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 200);
			}
			if (!Movement.facingRight)
			{
				bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 200);
			}
			shootTimer = shootCD;
		}
	}
}

