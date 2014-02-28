using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float bounceForce = 8000f;

	public float maxHealth = 30f;
	public float health;
	private bool dead = false;

	public GameObject explosion;

	public GameObject[] powerups;
	public float powerDropChange = 33f;

	Animator anim;	
	public ShipSounds shipSounds { get; set; }

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		shipSounds = GetComponent<ShipSounds>();
		health = maxHealth;
	}

	void takeDamage(float amount)
	{
		health -= amount;
		if(health < 0)
		{
			kill ();
		}
		else
		{
			shipSounds.PlayTakeDamage();
		}
	}

	public void kill()
	{
		shipSounds.playDeath();
		anim.SetTrigger("Death");
		Invoke("Death", 0.5f);
		rigidbody2D.velocity = Vector2.zero;
		dead = true;

		if(powerups.Length > 0)
		{
			int dropPowerup = Random.Range (1, 100);
			if(dropPowerup < powerDropChange)
			{
				Instantiate(powerups[Random.Range(0, powerups.Length)], transform.position, Quaternion.identity);
			}
		}
		var expl = Instantiate(explosion, transform.position, Quaternion.identity);
		Destroy(gameObject); // destroy the grenade
		Destroy(expl, 1); // delete the explosion after 1 second
	}

	void Death()
	{
		Destroy(this.gameObject);
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		if(collider.gameObject.tag.Equals ("PlayerShip") && !dead)
		{
			DamagePlayer damagePlayerEvent = new DamagePlayer (transform.gameObject, 1, false);
			GameEvents.GameEventManager.post (damagePlayerEvent);

			Vector2 direction = new Vector2(transform.position.x - collider.transform.position.x, 
			                                transform.position.y - collider.transform.position.y);
			rigidbody2D.velocity = new Vector2(0, 0);
			rigidbody2D.AddForce(direction * bounceForce);
		}

	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.gameObject.tag.Equals ("Bullet"))
		{
			if(collider.gameObject.GetComponent<Bullet>().isPlayerBullet())
			{
				takeDamage(collider.gameObject.GetComponent<Bullet>().getDamage());
			}
		}
	}
}
