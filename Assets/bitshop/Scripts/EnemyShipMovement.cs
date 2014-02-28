using UnityEngine;
using System.Collections;

public class EnemyShipMovement : MonoBehaviour {

	Animator anim;

	public GameObject player;
	public GameObject missile;

	bool facingRight = true;

	public float maxSpeed = 50f;
	public float accelerationForce = 600f;

	public float minXEvade = 0.3f;
	public float maxXEvade = 1f;

	public float minYEvade = 0.3f;
	public float maxYEvade = 1f;

	public float minShootCooldown = 2f;
	public float maxShootCooldown = 3.5f;
	float shootCooldown = 3f;

	Vector2 evadeTarget;
	bool needNewTarget = true;

	Enemy enemyScript;

	// Use this for initialization
	void Start () {
		player = FindPlayer.Instance.getPlayerObject ();
		shootCooldown = Random.Range(minShootCooldown, maxShootCooldown);
		anim = GetComponent<Animator>();
		enemyScript = GetComponent<Enemy> ();
	}

	// Update is called once per frame
	void FixedUpdate () {

		if (anim.GetCurrentAnimatorStateInfo(0).IsName("enemyShipIdle"))
		{
			needNewTarget = true;

			if(shootCooldown > 0f)
			{
				shootCooldown -= Time.deltaTime;
			}
			else
			{
				Shoot();
				shootCooldown = Random.Range(minShootCooldown, maxShootCooldown);
			}
		}
		else if (anim.GetCurrentAnimatorStateInfo(0).IsName("enemyShipEvade"))
		{
			if(needNewTarget)
			{
				float whichDirection = Random.Range(0, 500);
				if(whichDirection < 125)
				{

					evadeTarget = new Vector2(transform.position.x + Random.Range(minXEvade, maxXEvade),
				                        	  transform.position.y + Random.Range(minYEvade, maxYEvade));
				}
				else if(whichDirection < 250)
				{
					evadeTarget = new Vector2(transform.position.x + Random.Range(minXEvade, maxXEvade),
					                          transform.position.y - Random.Range(minYEvade, maxYEvade));
				}
				else if(whichDirection < 375)
				{
					evadeTarget = new Vector2(transform.position.x - Random.Range(minXEvade, maxXEvade),
					                          transform.position.y + Random.Range(minYEvade, maxYEvade));
				}
				else
				{
					evadeTarget = new Vector2(transform.position.x - Random.Range(minXEvade, maxXEvade),
					                          transform.position.y - Random.Range(minYEvade, maxYEvade));
				}

				Vector2 direction = new Vector2(evadeTarget.x - transform.position.x,
				                                evadeTarget.y - transform.position.y).normalized;
				
				rigidbody2D.AddForce(direction * accelerationForce);
				needNewTarget = false;
			}
		}

		if(facingRight && player.transform.position.x < transform.position.x)
		{
			Flip ();
		}
		else if(!facingRight && player.transform.position.x > transform.position.x)
		{
			Flip ();
		}
	}

	void Shoot()
	{
		enemyScript.shipSounds.PlayGunshot ();
		Vector3 rotation;
		if (facingRight)
		{
			rotation = new Vector3(0, 0, 0);
		}
		else
		{
			rotation = new Vector3(0, 0, 180);
		}
		GameObject missileSpawn = (GameObject) Instantiate(missile, transform.position, Quaternion.Euler(rotation));
	}

	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
