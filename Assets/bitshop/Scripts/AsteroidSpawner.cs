using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour {

	public GameObject asteroid;

	public GameObject target;

	public float minSpeed;
	public float maxSpeed;

	public float firstMinSpawnTime = 0f;
	public float minRespawnTime = 1f;
	public float maxRespawnTime = 1.2f;

	public float minScale = 0.7f;
	public float maxScale = 1.2f;

	public float minTorque = -5f;
	public float maxTorque = 5f;
	
	public float lifetime = 3f;

	private Vector2 velocity;
	
	public bool cameraShake = false;

	float cooldown = 0f;
	
	// Use this for initialization
	void Start () {
		cooldown = Random.Range (firstMinSpawnTime, minRespawnTime);
		velocity = new Vector2 (target.transform.position.x - transform.position.x,
		                        target.transform.position.y - transform.position.y).normalized * Random.Range(minSpeed, maxSpeed);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(cooldown > 0)
		{
			cooldown -= Time.deltaTime;
		}
		else
		{
			cooldown = Random.Range (minRespawnTime, maxRespawnTime);
			Vector3 position = new Vector3(transform.position.x, transform.position.y, asteroid.transform.position.z);
			Vector3 axis = new Vector3(0, 0 ,1);
			GameObject asteroidInstance = (GameObject) Instantiate(asteroid, position, Quaternion.Euler(Random.rotation * axis));
			asteroidInstance.GetComponent<Asteroid>().rigidbody2D.velocity = velocity;
			float scale = Random.Range(minScale, maxScale);
			asteroidInstance.GetComponent<Asteroid>().transform.localScale = new Vector3(scale, scale, 1);
			Destroy(asteroidInstance, lifetime);
			
			if(cameraShake)
			{
				CameraShake cameraShakeEvent = new CameraShake (0.1f, 0.15f);
				GameEvents.GameEventManager.post (cameraShakeEvent);
			}
		}
	}
}
