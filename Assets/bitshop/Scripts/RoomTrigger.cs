using UnityEngine;
using System.Collections;

public class RoomTrigger : MonoBehaviour {

	public bool isFirstTime = true;

	public GameObject blockerObj;

	bool blocked = false;
	ArrayList blockers = new ArrayList();

	ArrayList enemies = new ArrayList();

	void Start()
	{
		for(int i=0; i<transform.childCount; i++)
		{
			if(transform.GetChild(i).tag.Equals("Enemy") ||
			   transform.GetChild(i).tag.Equals("RockSpawner") ||
			   transform.GetChild(i).tag.Equals("Boss"))
			{
				transform.GetChild(i).gameObject.SetActive(false);
			}
		}
	}

	void Update()
	{
		if(blocked)
		{
			int enemyCount = 0;
			foreach(GameObject obj in enemies)
			{
				if(obj != null) enemyCount ++;
			}
			for(int i=0; i<transform.childCount; i++)
			{
				if(transform.GetChild(i).tag.Equals("Boss")) enemyCount++;
			}
			if(enemyCount <= 0)
			{
				unblockRoom();
			}
		}
	}

	void blockRoom()
	{
		int enemyCount = 0;
		for(int i=0; i<transform.childCount; i++)
		{
			if(transform.GetChild(i).tag.Equals("Spawner"))
			{
				EnemySpawner spawner = transform.GetChild(i).GetComponent<EnemySpawner>();
				SpawnEnemy(spawner.GetEnemy(), transform.GetChild (i).position);
				enemyCount++;
			}
		}

		for(int i=0; i<transform.childCount; i++)
		{
			if(transform.GetChild(i).tag.Equals("Boss")) enemyCount++;
		}

		if (enemyCount > 0)
		{
			blocked = true;
			Vector3 position = new Vector3 (transform.position.x - 20f, transform.position.y, transform.position.z + 1);
			GameObject blocker = (GameObject) Instantiate(blockerObj, position, Quaternion.identity);
			blockers.Add (blocker);

			position = new Vector3 (transform.position.x + 20f, transform.position.y, transform.position.z + 1);
			blocker = (GameObject) Instantiate(blockerObj, position, Quaternion.identity);
			blockers.Add (blocker);

			position = new Vector3 (transform.position.x, transform.position.y + 10.5f, transform.position.z + 1);
			blocker = (GameObject) Instantiate(blockerObj, position, Quaternion.Euler(new Vector3(0, 0, 90)));
			blockers.Add (blocker);

			position = new Vector3 (transform.position.x, transform.position.y - 10.5f, transform.position.z + 1);
			blocker = (GameObject) Instantiate(blockerObj, position, Quaternion.Euler(new Vector3(0, 0, 90)));
			blockers.Add (blocker);
		}
	}

	void SpawnEnemy(GameObject type, Vector3 position)
	{
		GameObject newEnemy = (GameObject) Instantiate(type, position, Quaternion.identity);
		enemies.Add (newEnemy);
	}

	void unblockRoom()
	{
		blocked = false;

		foreach(GameObject obj in blockers)
		{
			Destroy(obj);
		}
	}

	void OnTriggerEnter2D (Collider2D collider)
	{
		if (collider.tag.Equals ("PlayerShip"))
		{
			EnterRoom enterRoomEvent = new EnterRoom (transform, isFirstTime);
			GameEvents.GameEventManager.post (enterRoomEvent);

			if(isFirstTime)
			{
				blockRoom();
			}

			isFirstTime = false;

			for(int i=0; i<transform.childCount; i++)
			{
				transform.GetChild(i).gameObject.SetActive(true);
			}
		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.tag.Equals ("PlayerShip"))
		{
			for(int i=0; i<transform.childCount; i++)
			{
				if(transform.GetChild(i).tag.Equals("RockSpawner"))
				{
					transform.GetChild(i).gameObject.SetActive(false);
				}
			}
		}
	}
	
	void OnCollisionEnter2D(Collision2D collider)
	{
		if(collider.gameObject.tag.Equals ("PlayerShip"))
		{
			DamagePlayer damagePlayerEvent = new DamagePlayer (transform.gameObject, 1, true);
			GameEvents.GameEventManager.post (damagePlayerEvent);

			CameraShake cameraShakeEvent = new CameraShake ();
			GameEvents.GameEventManager.post (cameraShakeEvent);
		}
	}
}
