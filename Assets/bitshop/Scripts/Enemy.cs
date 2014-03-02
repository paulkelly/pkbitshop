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

	public bool colliderTakesDamage = true;
	public GameObject bossEye;

	Animator anim;	
	public ShipSounds shipSounds { get; set; }

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		shipSounds = GetComponent<ShipSounds>();
		health = maxHealth;
	}

	public void takeDamage(float amount)
	{
		anim.SetTrigger("Damage");
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
		if(dead) return;
		shipSounds.playDeath();
		anim.SetTrigger("Death");
		if(!colliderTakesDamage)
		{
			Invoke("Death", 0f);
		}
		else
		{
			Invoke("Death", 0.5f);
		}
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

		if(colliderTakesDamage)
		{
		var expl = Instantiate(explosion, transform.position, Quaternion.identity);
		Destroy(expl, 1); // delete the explosion after 1 second
		}
		else
		{
			var expl = Instantiate(explosion, bossEye.transform.position, Quaternion.identity);
			Destroy(expl, 3); // delete the explosion after 1 second
		}
	}

	void Death()
	{
		if(colliderTakesDamage)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Destroy (bossEye);
		}
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

			CameraShake cameraShakeEvent = new CameraShake (0.1f, 0.15f);
			GameEvents.GameEventManager.post (cameraShakeEvent);

			if(colliderTakesDamage) takeDamage(1);
		}

	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(colliderTakesDamage && collider.gameObject.tag.Equals ("Bullet"))
		{
			if(collider.gameObject.GetComponent<Bullet>().isPlayerBullet())
			{
				takeDamage(collider.gameObject.GetComponent<Bullet>().getDamage());
			}
		}
	}
}
