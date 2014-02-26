using UnityEngine;

public class PlayerShip2D : MonoBehaviour 
{
	bool facingRight = true;		// For determining which way the player is currently facing.

	public GameObject bullet;

	[SerializeField] float maxSpeed = 10f;		// The fastest the player can travel in the x axis.
	
	[SerializeField] public static float[] fireRateProgress = {2f, 3f, 4f, 5f, 6f};	
	[SerializeField] public static float[] damageProgress = {6f, 8f, 10f, 12f, 14f};
	
	private static int fireRateLevel = 4;
	private static int damageLevel = 0;

	private float fireRate = fireRateProgress[fireRateLevel]; // The number of times the player can shoot in 1 second.
	private float cooldown = 0f;
	bool shooting = false;
	private float damage = damageProgress[damageLevel]; // The number of damage the player deals per shot.

	Animator anim;										// Reference to the player's animator component.
	BulletManager bulletManager;

    void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void Start()
	{
		bulletManager = BulletManager.getInstance();
	}


	void FixedUpdate()
	{
		if (cooldown > 0f)
		{
			cooldown -= Time.deltaTime;
			shooting = true;
		}

		ParticleSystem particleSystem = getParticleSystem();
		if(particleSystem != null)
		{
			if (rigidbody2D.velocity.magnitude > 0)
			{
				particleSystem.enableEmission = true;
			}
			else
			{
				particleSystem.enableEmission = false;
			}
		}
		else
		{
			Debug.LogError("Could not find particle system for player ship.");
		}

	}

	ParticleSystem getParticleSystem()
	{
		for(int i=0; i<transform.childCount; i++)
		{
			if(transform.GetChild(i).particleSystem != null) return transform.GetChild(i).particleSystem;
		}
		return null;
	}

	public void Move(float moveH, float moveV)
	{
		// Move the character
		rigidbody2D.velocity = new Vector2(moveH * maxSpeed, moveV * maxSpeed);

		if (rigidbody2D.velocity.magnitude > maxSpeed)
						rigidbody2D.velocity = rigidbody2D.velocity.normalized * maxSpeed;

		// If the input is moving the player right and the player is facing left...
		if(moveH > 0 && !facingRight && !shooting)
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(moveH < 0 && facingRight && !shooting)
			// ... flip the player.
			Flip();

		shooting = false;
	}
	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;

		ParticleSystem particleSystem = getParticleSystem();
		if(particleSystem != null)
		{
			particleSystem.transform.Rotate(new Vector3(180, 0, 0));
		}
		
		// Set horizontal velocity to 0 when turning
		rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
	}

	public void shoot (Direction direction)
	{
		if (cooldown > 0f) return;
		cooldown = 1f / fireRate;
		anim.SetTrigger("Shoot");
		Vector2 playerDirection = rigidbody2D.velocity;
		float curveDapening = (maxSpeed * 2);
		Vector3 playerDirectionx = new Vector3 (playerDirection.x, 0, 0) / curveDapening;
		Vector3 playerDirectiony = new Vector3 (0, playerDirection.y, 0) / curveDapening;

		Vector3 bulletDirection = Vector3.down + playerDirectionx;
		if (direction.Equals (Direction.LEFT))
		{
			bulletDirection = Vector3.left + playerDirectiony;
			bulletManager.spawnBullet (transform.position, bulletDirection, true);

			if(facingRight)
				Flip();
		}
		else if (direction.Equals (Direction.RIGHT))
		{
			bulletDirection = Vector3.right + playerDirectiony;
			bulletManager.spawnBullet (transform.position, bulletDirection, true);

			if(!facingRight)
				Flip();
		}
	}
}
