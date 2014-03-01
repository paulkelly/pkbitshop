using UnityEngine;

public class PlayerShip2D : MonoBehaviour, GameEvents.GameEventListener
{
	bool facingRight = true;		// For determining which way the player is currently facing.

	public GameObject bullet;

	[SerializeField] float maxSpeed = 10f;		// The fastest the player can travel in the x axis.
	
	[SerializeField] public static float[] fireRateProgress = {2f, 2.4f, 3.6f, 3f, 3.5f, 4f, 4.5f, 5f, 5.5f, 6f};	
	[SerializeField] public static float[] damageProgress = {3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f, 11f, 12f};

	public GameObject GunshotParticleEffect;

	public GameObject explosion;

	public int fireRateLevel = 0;
	public int damageLevel = 0;
	
	bool isShieldDisabling = false;
	bool isShieldEnabling = false;

	bool tookDamage = false;
	bool invunerable = false;

	private int shield = 3;
	private int shieldCharge = 0;
	private int shieldMaxCharge = 3;

	private float fireRate; // The number of times the player can shoot in 1 second.
	private float cooldown = 0f;
	bool shooting = false;
	private float damage; // The number of damage the player deals per shot.

	Animator anim;										// Reference to the player's animator component.
	ShipSounds shipSounds;
	BulletManager bulletManager;

	public float bounceForce = 10000f;
	
	private bool has5Shot = false;

    void Awake()
	{
		GameEvents.GameEventManager.registerListener(this);
		anim = GetComponent<Animator>();
		shipSounds = GetComponent<ShipSounds>();

		fireRate = fireRateProgress[fireRateLevel]; // The number of times the player can shoot in 1 second.
		damage = damageProgress[damageLevel]; // The number of damage the player deals per shot.
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
			
			setShieldEnabled(false);
		}

		if (anim.GetCurrentAnimatorStateInfo(0).IsName("shieldenable") && !isShieldEnabling)
		{
			isShieldEnabling = true;
		}
		if (!anim.GetCurrentAnimatorStateInfo(0).IsName("shieldenable") && isShieldEnabling)
		{
			isShieldEnabling = false;

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

	Transform getBulletSpawnPoint()
	{
		for(int i=0; i<transform.childCount; i++)
		{
			for(int j=0; j<transform.GetChild(i).childCount; j++)
			{
				if(transform.GetChild(i).GetChild(j).tag.Equals("BulletSpawn"))
				{
					return transform.GetChild(i).GetChild(j);
				}
			}
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
	}

	public void shoot (Direction direction)
	{
		if (cooldown > 0f) return;
		cooldown = 1f / fireRate;
		anim.SetTrigger("Shoot");
		shipSounds.PlayGunshot ();
		Vector2 playerDirection = rigidbody2D.velocity;
		float curveDapening = (maxSpeed * 3);
		Vector3 playerDirectionx = new Vector3 (playerDirection.x, 0, 0) / curveDapening;
		Vector3 playerDirectiony = new Vector3 (0, playerDirection.y, 0) / curveDapening;
		
		Vector3 shot2 = new Vector3(0, 0.04f, 0);
		Vector3 shot3 = new Vector3(0, -0.04f, 0);

		Vector3 bulletDirection = Vector3.down + playerDirectionx;
		if (direction.Equals (Direction.LEFT))
		{
			bulletDirection = Vector3.left + playerDirectiony;
			bulletManager.spawnBullet (getBulletSpawnPoint().position, bulletDirection, true, damage);
			bulletManager.spawnBullet (getBulletSpawnPoint().position, bulletDirection + shot2, true, damage);
			bulletManager.spawnBullet (getBulletSpawnPoint().position, bulletDirection + shot3, true, damage);
			if(has5Shot)
			{
				bulletManager.spawnBullet (getBulletSpawnPoint().position, bulletDirection + 2*shot2, true, damage);
				bulletManager.spawnBullet (getBulletSpawnPoint().position, bulletDirection + 2*shot3, true, damage);
			}
			if(facingRight)
				Flip();
		}
		else if (direction.Equals (Direction.RIGHT))
		{
			bulletDirection = Vector3.right + playerDirectiony;
			bulletManager.spawnBullet (getBulletSpawnPoint().position, bulletDirection, true, damage);
			bulletManager.spawnBullet (getBulletSpawnPoint().position, bulletDirection + shot2, true, damage);
			bulletManager.spawnBullet (getBulletSpawnPoint().position, bulletDirection + shot3, true, damage);
			if(has5Shot)
			{
				bulletManager.spawnBullet (getBulletSpawnPoint().position, bulletDirection + 2*shot2, true, damage);
				bulletManager.spawnBullet (getBulletSpawnPoint().position, bulletDirection + 2*shot3, true, damage);
			}
			if(!facingRight)
				Flip();
		}
		spawnGunshotPaticleEffect ();
	}

	void spawnGunshotPaticleEffect()
	{
		var expl = Instantiate(GunshotParticleEffect, getBulletSpawnPoint().position, Quaternion.identity);
		Destroy(expl, 1); // delete the explosion after 1 second
	}

	void doDamage(int amount)
	{
		if(shield > 0)
		{
			shipSounds.playTakeShieldDamage();
		}
		if (invunerable || tookDamage) return;
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
			anim.SetTrigger("Death");
			var expl = Instantiate(explosion, transform.position, Quaternion.identity);
			Destroy(gameObject); // destroy the grenade
			Destroy(expl, 1); // delete the explosion after 1 second
			shipSounds.playDeath();
			//Invoke ("EndGame", 0.5f);
			EndGame();
		}
		UpdateHud updateHudEvent = new UpdateHud ();
		GameEvents.GameEventManager.post (updateHudEvent);
	}

	void EndGame()
	{
		Debug.Log ("invoked end game");
		Restart restart = new Restart ();
		GameEvents.GameEventManager.post (restart);
	}

	void gainShield()
	{
		if(shield == 0) anim.SetTrigger("EnableShield");
		shield++;

		UpdateHud updateHudEvent = new UpdateHud ();
		GameEvents.GameEventManager.post (updateHudEvent);
	}

	void gainRateofFire()
	{
		if(fireRateLevel < fireRateProgress.Length-1)
		{
			fireRateLevel++;
			fireRate = fireRateProgress[fireRateLevel];
		}

		UpdateHud updateHudEvent = new UpdateHud ();
		GameEvents.GameEventManager.post (updateHudEvent);
	}

	void gainDamage()
	{
		if(damageLevel < damageProgress.Length-1)
		{
			damageLevel++;
			damage = damageProgress[damageLevel];
		}

		UpdateHud updateHudEvent = new UpdateHud ();
		GameEvents.GameEventManager.post (updateHudEvent);
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

	public int getDamage()
	{
		return damageLevel;
	}

	public int getRateOfFire()
	{
		return fireRateLevel;
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
			bool doKnockback = !(tookDamage || invunerable) && ((DamagePlayer) e).bounceAfterTakingDamage();
			doDamage(damage);

			if(doKnockback)
			{
				GameObject damager = ((DamagePlayer) e).getDamager();

				if(damager.tag.Equals("Enemy"))
				{
					Vector2 direction = new Vector2(transform.position.x - damager.transform.position.x, 
					                                transform.position.y - damager.transform.position.y);
					rigidbody2D.AddForce(direction * bounceForce);
				}
				else
				{
					Vector2 direction = rigidbody2D.velocity.normalized * -1;;
					rigidbody2D.AddForce(direction * bounceForce);
				}
			}
		}
		else if(e.GetType().Name.Equals("EnterRoom"))
		{
			bool isFirstTime = ((EnterRoom) e).isFirstTime();
			if(isFirstTime)
			{
				gainShieldCharge();
			}
		}
		else if(e.GetType().Name.Equals("CollectPower"))
		{
			int type = ((CollectPower) e).getPowerupType();
			if(type == 1)
			{
				gainShield();
			}
			else if(type == 2)
			{
				gainDamage();
			}
			else if(type == 3)
			{
				gainRateofFire();
			}
			else if(type == 4)
			{
				has5Shot = true;
			}
	
		}
	}
}
