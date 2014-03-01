using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	float percentHealth = 1f;
	Vector2 pos;

	Enemy enemyScript;

	void Start ()
	{
		enemyScript = transform.parent.GetComponent<Enemy> ();
	}
	

	void Update()
	{
		//pos = new Vector2 (transform.position.x, transform.position.y - 1f);
		
		percentHealth = 0f;

		if(enemyScript.health > 0)
		{ 
			percentHealth = enemyScript.health/enemyScript.maxHealth;
		}
		Vector3 theScale = new Vector3 (percentHealth, 1, 1);
		transform.GetChild (0).transform.localScale = theScale;

		theScale = transform.localScale;
		if(transform.parent.localScale.x < 0 && !(theScale.x < 0) ||
		   transform.parent.localScale.x > 0 && !(theScale.x > 0)) theScale.x *= -1;
		transform.localScale = theScale;
		

	}

}
