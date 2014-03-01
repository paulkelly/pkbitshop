using UnityEngine;
using System.Collections;

public class DroneMovement : MonoBehaviour {

	public bool facingRight = true;

	public float speedInactive = 1f;
	float speedActive = 7f;

	public float minSpeedActive = 6f;
	public float maxSpeedActive = 10f;
		
	public float minActiveDelay = 3f;
	public float maxActivateDelay = 5f;
	
	private float activateDelay = 0f;
	
	public float maxLockCooldown = 0.5f;
	public float minLockCooldown = 0.3f;
	private float lockCooldown = 0f;
	private bool needLockOn = false;

	private GameObject target;

	float targetX;
	float targetY;

	Animator anim;

	void Start () {
		target = FindPlayer.Instance.getPlayerObject ();
		
		activateDelay = Random.Range (minActiveDelay-3f, maxActivateDelay-3f);
		anim = GetComponent<Animator>();

		speedActive = Random.Range (minSpeedActive, maxSpeedActive);
	}

	// Update is called once per frame
	void FixedUpdate () {
		float distance = transform.position.x - targetX;
		if(distance > 0f && distance < 1f || distance < 0f && distance > -1f)
		{
			needLockOn = true;
		}
		distance = transform.position.y - targetY;
		if(distance > 0f && distance < 1f || distance < 0f && distance > -1f)
		{
			needLockOn = true;
		}

		if(anim.GetCurrentAnimatorStateInfo(0).IsName("droneDeath"))
		{
			rigidbody2D.velocity = Vector2.zero;
			return;
		}
		float speed = speedInactive;
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("activate")) 
		{		
			speed = speedActive;

			if(lockCooldown <= 0)
			{
				targetX = target.transform.position.x;
				targetY = target.transform.position.y + Random.Range(-1f, 1f);
				
				lockCooldown = Random.Range(minLockCooldown, maxLockCooldown);

			}
			else
			{
				lockCooldown-= Time.deltaTime;
			}
		}
		else
		{
			if(activateDelay <= 0)
			{
				anim.SetTrigger("Activate");
				activateDelay = Random.Range (minActiveDelay, maxActivateDelay);
			}
			else
			{
				activateDelay -= Time.deltaTime;
			}
			lockCooldown = 0f;
		}
		
		
		rigidbody2D.velocity = new Vector2 (targetX - transform.position.x, targetY - transform.position.y);
		
		rigidbody2D.velocity = rigidbody2D.velocity.normalized * speed;
		
		
		// If the input is moving the player right and the player is facing left...
		if(rigidbody2D.velocity.x > 0f && !facingRight)
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(rigidbody2D.velocity.x < 0f && facingRight)
			// ... flip the player.
			Flip();
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
