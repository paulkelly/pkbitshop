using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float health = 30f;
	public float speed = 5f;

	public GameObject target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float targetX = target.transform.position.x;
		float targetY = target.transform.position.y;

		rigidbody2D.velocity = new Vector2(targetX - transform.position.x, targetY - transform.position.y);

		rigidbody2D.velocity = rigidbody2D.velocity.normalized * speed;
	}

	void takeDamage(float amount)
	{
		health -= amount;
		if(health < 0) Destroy(this.gameObject);
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		if(collider.gameObject.tag.Equals ("PlayerShip"))
		{
			DamagePlayer damagePlayerEvent = new DamagePlayer (transform.gameObject, 1);
			GameEvents.GameEventManager.post (damagePlayerEvent);
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
