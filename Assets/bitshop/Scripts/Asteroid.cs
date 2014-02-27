using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	Animator anim;	

	void Start()
	{
		anim = GetComponent<Animator>();
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "PlayerShip") 
		{
			DamagePlayer damagePlayerEvent = new DamagePlayer (transform.gameObject, 1, false);
			GameEvents.GameEventManager.post (damagePlayerEvent);
			anim.SetTrigger("Death");
			Invoke("Death", 0.5f);
		}
	}

	void Death()
	{
		Destroy(this.gameObject);
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		if(collider.gameObject.tag.Equals ("PlayerShip"))
		{
			DamagePlayer damagePlayerEvent = new DamagePlayer (transform.gameObject, 1, false);
			GameEvents.GameEventManager.post (damagePlayerEvent);
		}
		
	}
}
