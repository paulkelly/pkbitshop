using UnityEngine;
using System.Collections;

public class BossEye : MonoBehaviour {

	Enemy bossBody;
	Animator anim;

	ArrayList enemies = new ArrayList();
	public GameObject minionType;

	bool isLaserActive = false;

	public GameObject player;
	float movementLerpTime = 0.02f;

	float spawnEyeCooldown =0f;
	public float minEyeCooldown = 0.2f;
	public float maxEyeCooldown = 2f;

	// Use this for initialization
	void Start () {
		player = FindPlayer.Instance.getPlayerObject ();
		bossBody = transform.parent.GetComponent<Enemy> ();
		anim = transform.GetComponent<Animator> ();

		spawnEyeCooldown = Random.Range (minEyeCooldown, maxEyeCooldown);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(spawnEyeCooldown < 0f)
		{
			SpawnMiniEye();
			spawnEyeCooldown = Random.Range (minEyeCooldown, maxEyeCooldown);
		}
		else
		{
			spawnEyeCooldown -= Time.deltaTime;
		}

		if (anim.GetCurrentAnimatorStateInfo(0).IsName("bossAiming"))
		{
			isLaserActive = false;

			Vector3 target = Vector3.Lerp(transform.position, player.transform.position, movementLerpTime);
			transform.position = new Vector3(transform.position.x, target.y, transform.position.z);
		}
		else if (anim.GetCurrentAnimatorStateInfo(0).IsName("bossFireing"))
		{
			isLaserActive = true;
		}
		else
		{
			isLaserActive = false;
		}
	}

	void SpawnMiniEyes()
	{
		int number = Random.Range (4, 7);

		for(int i=0;i<number; i++)
		{
			SpawnMiniEye();
		}
	}

	void SpawnMiniEye()
	{
		float randomX = Random.Range (-0.5f, 0.5f);
		float randomY = Random.Range (-10.5f, 10.5f);
		Vector3 position = new Vector3 (bossBody.transform.position.x + randomX,
		                                bossBody.transform.position.y + randomY,
		                                bossBody.transform.position.z + 1);

		GameObject newEnemy = (GameObject) Instantiate(minionType, position, Quaternion.identity);
		enemies.Add (newEnemy);
	}

	void OnDestroy()
	{
		foreach(GameObject obj in enemies)
		{
			Destroy (obj);
		}

		Win winEvent = new Win ();
		GameEvents.GameEventManager.post (winEvent);
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		if(collider.gameObject.tag.Equals ("PlayerShip"))
		{
			DamagePlayer damagePlayerEvent = new DamagePlayer (transform.gameObject, 1, false);
			GameEvents.GameEventManager.post (damagePlayerEvent);
			
			CameraShake cameraShakeEvent = new CameraShake (0.1f, 0.15f);
			GameEvents.GameEventManager.post (cameraShakeEvent);
			
		}
		
	}
	
	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.gameObject.tag.Equals ("PlayerShip"))
		{
			DamagePlayer damagePlayerEvent = new DamagePlayer (transform.gameObject, 1, false);
			GameEvents.GameEventManager.post (damagePlayerEvent);
			
			CameraShake cameraShakeEvent = new CameraShake (0.1f, 0.15f);
			GameEvents.GameEventManager.post (cameraShakeEvent);
			
		}

		if(collider.gameObject.tag.Equals ("Bullet"))
		{
			if(collider.gameObject.GetComponent<Bullet>().isPlayerBullet())
			{
				bossBody.takeDamage(collider.gameObject.GetComponent<Bullet>().getDamage());
			}
		}
	}
}
