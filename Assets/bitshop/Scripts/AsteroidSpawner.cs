using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour {

	public GameObject asteroid;

	public GameObject target;

	public float speed;

	public float minRespawnTime = 1f;
	public float maxRespawnTime = 1.2f;

	public float minScale = 0.7f;
	public float maxScale = 1.2f;

	public float minTorque = -5f;
	public float maxTorque = 5f;

	private Vector2 velocity;

	float cooldown = 0f;
	
	// Use this for initialization
	void Start () {
		velocity = new Vector2 (target.transform.position.x - transform.position.x,
		                        target.transform.position.y - transform.position.y).normalized * speed;
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
			GameObject asteroidInstance = (GameObject) Instantiate(asteroid, position, Quaternion.identity);
			asteroidInstance.GetComponent<Asteroid>().rigidbody2D.velocity = velocity;
			asteroidInstance.GetComponent<Asteroid>().rigidbody2D.AddTorque(Random.Range(minTorque, maxTorque));
			float scale = Random.Range(minScale, maxScale);
			asteroidInstance.GetComponent<Asteroid>().transform.localScale = new Vector3(scale, scale, 1);
			Destroy(asteroidInstance, 3f);
		}
	}
}
