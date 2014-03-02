using UnityEngine;
using System.Collections;

public class FailScreen : MonoBehaviour {

	public float moveSpeed = 0f;
	
	void FixedUpdate ()
	{
		transform.position = new Vector3(transform.position.x - moveSpeed, transform.position.y, transform.position.z);
	
		BackgroundScrollEvent scroll = new BackgroundScrollEvent (Time.deltaTime*10, 0);
		GameEvents.GameEventManager.post (scroll);
	}
}
