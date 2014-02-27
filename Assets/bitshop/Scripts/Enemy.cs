using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float bounceForce = 8000f;

	public float health = 30f;
	private bool dead = false;

	public GameObject explosion;

	Animator anim;	
	ShipSounds shipSounds;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		shipSounds = GetComponent<ShipSounds>();
	}

	void takeDamage(float amount)
	{
		health -= amount;
		if(health < 0)
		{
			shipSounds.playDeath();
			anim.SetTrigger("Death");
			Invoke("Death", 0.5f);
			rigidbody2D.velocity = Vector2.zero;
			dead = true;

			var expl = Instantiate(explosion, transform.position, Quaternion.identity);
			Destroy(gameObject); // destroy the grenade
			Destroy(expl, 3); // delete the explosion after 3 seconds
		}
		else
		{
			shipSounds.PlayTakeDamage();
		}
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
