using UnityEngine;
using System.Collections;

public class RoomTrigger : MonoBehaviour {

	public bool isFirstTime = true;

	void OnTriggerEnter2D (Collider2D collider)
	{
		if (collider.gameObject.tag.Equals ("PlayerShip"))
		{
			EnterRoom enterRoomEvent = new EnterRoom (transform, isFirstTime);
			isFirstTime = false;
			GameEvents.GameEventManager.post (enterRoomEvent);
		}
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		if(collider.gameObject.tag.Equals ("PlayerShip"))
		{
			DamagePlayer damagePlayerEvent = new DamagePlayer (transform.gameObject, 1);
			GameEvents.GameEventManager.post (damagePlayerEvent);
		}
	}
}
