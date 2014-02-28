using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {

	public int type;

	void OnTriggerEnter2D (Collider2D collider)
	{
		if (collider.tag.Equals ("PlayerShip"))
		{
			CollectPower collectPowerEvent = new CollectPower (type);
			GameEvents.GameEventManager.post (collectPowerEvent);

			Destroy(this.gameObject);
		}
	}
}
