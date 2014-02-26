using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float speed = 0.1f;
	bool playerBullet = false;
	private Vector3 direction;

	public Bullet(Vector3 direction)
	{
		this.direction = direction;
	}

	public void setDirection(Vector3 direction)
	{
		this.direction = direction;
	}

	public void setPlayerBullet()
	{
		playerBullet = true;
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
		Destroy(this.gameObject);
	}
}
