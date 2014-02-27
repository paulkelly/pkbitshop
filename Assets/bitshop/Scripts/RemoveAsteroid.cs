using UnityEngine;
using System.Collections;

public class RemoveAsteroid : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D collider)
	{
		if (collider.tag == ("Asteroid"))
		{
			Destroy(collider.gameObject);
		}
	}
}
