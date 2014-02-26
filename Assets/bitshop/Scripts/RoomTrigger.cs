using UnityEngine;
using System.Collections;

public class RoomTrigger : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D collider)
	{
		if (collider.gameObject.tag.Equals ("PlayerShip"))
		{
			EnterRoom enterRoomEvent = new EnterRoom (transform);
			GameEvents.GameEventManager.post (enterRoomEvent);
		}
	}
}
