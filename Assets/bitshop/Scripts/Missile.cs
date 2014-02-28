using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {

	public GameObject target;
	public LayerMask background;

	public float accelerationForce = 300f;
	public float maxVelocity = 60f;

	public float maxTurnSpeed = 0.00001f;
	
	Enemy enemyScript;

	// Use this for initialization
	void Start () {
		enemyScript = GetComponent<Enemy> ();
		target = FindPlayer.Instance.getPlayerObject ();

		Vector2 direction = new Vector2(target.transform.position.x - transform.position.x,
		                                target.transform.position.y - transform.position.y).normalized;
		//rotateTowardsPlayer (1f);
	}

	
	// Update is called once per frame
	void FixedUpdate ()
	{
		Vector2 direction = new Vector2(target.transform.position.x - transform.position.x,
		                                target.transform.position.y - transform.position.y).normalized;

		rotateTowardsPlayer(maxTurnSpeed);

		rigidbody2D.AddForce ((this.transform.rotation * Vector3.right) * accelerationForce);

		if (rigidbody2D.velocity.magnitude > maxVelocity)
						rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity, maxVelocity);
	}

	void rotateTowardsPlayer(float time)
	{
		Vector2 direction = new Vector2(target.transform.position.x - transform.position.x,
		                                target.transform.position.y - transform.position.y).normalized;
		float angle = (Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg) - 90;
		
		//transform.rotation = Quaternion.AngleAxis (angle, Vector3.back);
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis (angle, Vector3.back), time);

	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		if(!collider.gameObject.tag.Equals ("Enemy"))
		{
			enemyScript.kill ();
		}
		
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.gameObject.tag.Equals ("Bullet"))
		{
			Destroy(collider.gameObject);

			enemyScript.kill ();
		}
	}
}
