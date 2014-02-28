using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float speed = 0.1f;
	bool playerBullet = false;
	float amount;
	private Vector3 direction;

	public GameObject explosion;

	public Bullet(Vector3 direction)
	{
		this.direction = direction;
	}

	public void setDirection(Vector3 direction)
	{
		this.direction = direction;
	}

	public void setDamage(float amount)
	{
		this.amount = amount;
	}

	public float getDamage()
	{
		return amount;
	}

	public void setPlayerBullet()
	{
		playerBullet = true;
	}

	public bool isPlayerBullet()
	{
		return playerBullet;
	}

	// Update is called once per frame
	void FixedUpdate () {
		transform.Translate (direction.normalized * speed, Space.World);
	}

	void OnTriggerEnter2D (Collider2D collider)
	{
		if (playerBullet && collider.gameObject.tag.Equals ("PlayerShip") || collider.isTrigger)
		{
			return;
		}
		else if(collider.gameObject.tag.Equals ("PlayerShip"))
		{
			DamagePlayer damagePlayerEvent = new DamagePlayer (transform.gameObject, 1);
			GameEvents.GameEventManager.post (damagePlayerEvent);
		}
		Destroy(this.gameObject);

		var expl = Instantiate(explosion, transform.position, Quaternion.identity);
		Destroy(gameObject); // destroy the grenade
		Destroy(expl, 1); // delete the explosion after 1 second
	}
}
