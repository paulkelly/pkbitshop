using UnityEngine;

public class PlayerShip2D : MonoBehaviour, GameEvents.GameEventListener
{
	bool facingRight = true;		// For determining which way the player is currently facing.

	public GameObject bullet;

	[SerializeField] float maxSpeed = 10f;		// The fastest the player can travel in the x axis.
	
	[SerializeField] public static float[] fireRateProgress = {2f, 3f, 4f, 5f, 6f};	
	[SerializeField] public static float[] damageProgress = {6f, 8f, 10f, 12f, 14f};
	
	private static int fireRateLevel = 4;
	private static int damageLevel = 0;

	bool shieldEnabled = true;
	bool isShieldDisabling = false;
	bool isShieldEnabling = false;

	bool tookDamage = false;
	bool invunerable = false;

	private int shield = 3;
	private int shieldCharge = 0;
	private int shieldMaxCharge = 3;

	private float fireRate = fireRateProgress[fireRateLevel]; // The number of times the player can shoot in 1 second.
	private float cooldown = 0f;
	bool shooting = false;
	private float damage = damageProgress[damageLevel]; // The number of damage the player deals per shot.

	Animator anim;										// Reference to the player's animator component.
	BulletManager bulletManager;

    void Awake()
	{
		GameEvents.GameEventManager.registerListener(this);
		anim = GetComponent<Animator>();
	}

	void Start()
	{
		bulletManager = BulletManager.getInstance();
	}

	void Update()
	{

		if (anim.GetCurrentAnimatorStateInfo(0).IsName("shielddisable") && !isShieldDisabling)
		{
			isShieldDisabling = true;
		}
		if (!anim.GetCurrentAnimatorStateInfo(0).IsName("shielddisable") && isShieldDisabling)
		{
			isShieldDisabling = false;
			shieldEnabled = false;
			
			setShieldEnabled(false);
		}

		if (anim.GetCurrentAnimatorStateInfo(0).IsName("shieldenable") && !isShieldEnabling)
		{
			isShieldEnabling = true;
		}
		if (!anim.GetCurrentAnimatorStateInfo(0).IsName("shieldenable") && isShieldEnabling)
		{
			isShieldEnabling = false;
			shieldEnabled = true;

			setShieldEnabled(true);
		}

		if (anim.GetCurrentAnimatorStateInfo(0).IsName("invunerable") && tookDamage)
		{
			invunerable = true;
			tookDamage = false;
		}
		if (!anim.GetCurrentAnimatorStateInfo(0).IsName("invunerable") && invunerable)
		{
			invunerable = false;
		}
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

	void setShieldEnabled(bool enabled)
	{
		for(int i=0; i<transform.childCount; i++)
		{
			if(transform.GetChild(i).name.Equals("Shield"))
			{
				transform.GetChild(i).gameObject.SetActive(enabled);
				Debug.Log ("Shield enabled set to " + enabled);
			}
		}
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
			bulletManager.spawnBullet (transform.position, bulletDirection, true, damage);

			if(facingRight)
				Flip();
		}
		else if (direction.Equals (Direction.RIGHT))
		{
			bulletDirection = Vector3.right + playerDirectiony;
			bulletManager.spawnBullet (transform.position, bulletDirection, true, damage);

			if(!facingRight)
				Flip();
		}
	}

	void doDamage(int amount)
	{
		if (invunerable || tookDamage) return;
		Debug.Log ("Player took damage");
		anim.SetTrigger("Invunerable");
		tookDamage = true;
		if(shield > 0)
		{
			shield -= amount;
			if(shield <= 0)
			{
				shield = 0;
				anim.SetTrigger("DisableShield");
			}
		}
		else
		{
			Debug.Break();
		}
		UpdateHud updateHudEvent = new UpdateHud ();
		GameEvents.GameEventManager.post (updateHudEvent);
	}

	void gainShield()
	{
		if(shield == 0) anim.SetTrigger("EnableShield");

		shield++;
	}

	void gainShieldCharge()
	{
		shieldCharge++;
		if(shieldCharge >= shieldMaxCharge)
		{
			shieldCharge = shieldCharge % shieldMaxCharge;
			gainShield();
		}
		UpdateHud updateHudEvent = new UpdateHud ();
		GameEvents.GameEventManager.post (updateHudEvent);
	}

	public int getShield()
	{
		return shield;
	}

	public int getShieldCharge()
	{
		return shieldCharge;
	}

	public void receiveEvent(GameEvents.GameEvent e)
	{
		if(e.GetType().Name.Equals("DamagePlayer"))
		{
			int damage = ((DamagePlayer) e).getDamageValue();
			doDamage(damage);
		}
		else if(e.GetType().Name.Equals("EnterRoom"))
		{
			bool isFirstTime = ((EnterRoom) e).isFirstTime();
			if(isFirstTime)
			{
				Debug.Log ("Entered room for first time.");
				gainShieldCharge();
			}
		}
	}
}
