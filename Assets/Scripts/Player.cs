using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	AudioManager m_AudioManager;

	public Transform sprite;
	// Use this for initialization
	Animator anim;
	CameraManager m_CameraManager;
	Rigidbody2D m_Rigidbody;

	//Speed and jump vary between characters
	[Header("Variables")]
	[SerializeField] bool spy_spawn;
	[SerializeField] float speed = 1.3f;
	[SerializeField] float jumpSpeed = 3.0f;
	[SerializeField] float health = 100.0f;
	[SerializeField] float bulletDamage = 1.0f;
	[SerializeField] float bulletForce = 300.0f;
	[SerializeField] float bulletKnockForce = 300.0f;
	[SerializeField] float bulletYShiftRange = 2.0f;
	[SerializeField] float bulletsYSpreadRange = 2.0f;
	[SerializeField] float shellForce = 30.0f;
	[SerializeField] float shellYShift = 0.2f;
	[SerializeField] float shootCD = 0.2f;

	[SerializeField] float hurtCD = 0.2f;
	float hurtTimer;

	float shootTimer = 0f;

	static bool grounded;
	bool ground;
	bool isShooting;
	[SerializeField] bool Scout = true;
	[SerializeField] Rigidbody2D rigid;
	Movement Move = new Movement ();

	Ray2D ray;
	RaycastHit2D hit;

	[Header("Prefabs")]
	[SerializeField] GameObject gunPoint;
	[SerializeField] GameObject bulletObject;
	[SerializeField] GameObject muzzleFlashObject;
	[SerializeField] GameObject shellObject;

	void Start () {
		anim = GetComponent<Animator>();
		m_CameraManager = GameObject.FindWithTag("MainCamera").GetComponent<CameraManager>();
		m_Rigidbody = this.GetComponent<Rigidbody2D>();
		m_AudioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
	}
	void FixedUpdate(){
		GroundDetection ();
		ground = grounded;
	}

	// Update is called once per frame
	void Update () {
		anim.SetFloat ("Speed", Mathf.Abs(rigid.velocity.x));
		anim.SetBool ("touchingGround", grounded);

		if (shootTimer > 0f)
		{
			shootTimer -= Time.deltaTime;
		}

		if (hurtTimer > 0f)
		{
			hurtTimer -= Time.deltaTime;
		}
		else
		{
			hurtTimer = 0f;
		}

		if (Input.GetButton("Fire1"))
		{
			Shooting();
			isShooting = true;
		}
		else
		{
			isShooting = false;
		}

		Move.Motion(speed, jumpSpeed, rigid, grounded, Scout, isShooting, sprite);
	}

	void GroundDetection(){
		hit = Physics2D.Raycast (GameObject.Find("Spy_Feet").transform.position, Vector2.down);
		if (hit.distance < 0.03){
			grounded = true;
		}
		if(hit.distance > 0.03){
			grounded = false;
		}
	}

	void Shooting(){
		if (shootTimer <= 0)
        {
            Shoot();
            shootTimer = shootCD;
        }

		if (Movement.facingRight)
		{
			m_Rigidbody.velocity = new Vector2(-0.28f + Movement.curVelocityX, m_Rigidbody.velocity.y);
		}
		else
		{
			m_Rigidbody.velocity = new Vector2(0.28f + Movement.curVelocityX, m_Rigidbody.velocity.y);
		}
    }

    void Shoot()
    {
        GameObject bulletUp = Instantiate(bulletObject, gunPoint.transform.position, gunPoint.transform.rotation, GameObject.Find("Bullets").transform) as GameObject;
		GameObject bulletMid = Instantiate(bulletObject, gunPoint.transform.position, gunPoint.transform.rotation, GameObject.Find("Bullets").transform) as GameObject;
		GameObject bulletDown = Instantiate(bulletObject, gunPoint.transform.position, gunPoint.transform.rotation, GameObject.Find("Bullets").transform) as GameObject;

		bulletUp.GetComponent<Bullet>().Damage = bulletDamage;
		bulletUp.GetComponent<Bullet>().KnockForce = bulletKnockForce;

		bulletMid.GetComponent<Bullet>().Damage = bulletDamage;
		bulletMid.GetComponent<Bullet>().KnockForce = bulletKnockForce;

		bulletDown.GetComponent<Bullet>().Damage = bulletDamage;
		bulletDown.GetComponent<Bullet>().KnockForce = bulletKnockForce;

		m_AudioManager.PlayGunshot();
		Destroy(bulletUp, 0.8f);
		Destroy(bulletMid, 0.8f);
		Destroy(bulletDown, 0.8f);

		GameObject shell = Instantiate(shellObject, gunPoint.transform.position, gunPoint.transform.rotation, GameObject.Find("Bullets").transform) as GameObject;

		GameObject muzzleFlash = Instantiate(muzzleFlashObject, gunPoint.transform.position, gunPoint.transform.rotation, transform) as GameObject;
		Destroy(muzzleFlash, 0.1f);

		StartCoroutine(m_CameraManager.Shake(0.15f, 0.05f, Movement.facingRight));

		if (Movement.facingRight)
        {
            bulletUp.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletForce);
			bulletMid.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletForce);
			bulletDown.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletForce);
			shell.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1.0f + Random.Range(-shellYShift, shellYShift), 1.0f + Random.Range(-shellYShift, shellYShift)) * shellForce);
		}
        else
        {
            bulletUp.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletForce);
			bulletMid.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletForce);
			bulletDown.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletForce);
			bulletUp.transform.localScale = new Vector2(bulletUp.transform.localScale.x * -1, bulletUp.transform.localScale.y);
			bulletMid.transform.localScale = new Vector2(bulletMid.transform.localScale.x * -1, bulletMid.transform.localScale.y);
			bulletDown.transform.localScale = new Vector2(bulletDown.transform.localScale.x * -1, bulletDown.transform.localScale.y);
			shell.GetComponent<Rigidbody2D>().AddForce(new Vector2(1.0f + Random.Range(-shellYShift, shellYShift), 1.0f + Random.Range(-shellYShift, shellYShift)) * shellForce);
		}

		//float bulletYShift = Random.Range(-bulletYShiftRange, bulletYShiftRange);
		bulletUp.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletUp.GetComponent<Rigidbody2D>().velocity.x, Random.Range(-bulletYShiftRange, bulletYShiftRange) + bulletsYSpreadRange);
		bulletMid.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletMid.GetComponent<Rigidbody2D>().velocity.x, Random.Range(-bulletYShiftRange, bulletYShiftRange));
		bulletDown.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletDown.GetComponent<Rigidbody2D>().velocity.x, Random.Range(-bulletYShiftRange, bulletYShiftRange) - bulletsYSpreadRange);
	}

	public void TakeDamage(float amount, float force, bool fromRight)
	{
		

		if (hurtTimer <= 0f)
		{
			health -= amount;
			hurtTimer = hurtCD;
			Vector2 dir = fromRight ? Vector2.left : Vector2.right;
			m_Rigidbody.AddForce(new Vector2(dir.x * force, force), ForceMode2D.Impulse);
		}

		if (health <= 0f)
		{

		}
	}
}

