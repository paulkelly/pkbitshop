using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour, GameEvents.GameEventListener
{
	public float xSmooth = 20f;		// How smoothly the camera catches up with it's target movement in the x axis.
	public float ySmooth = 20f;		// How smoothly the camera catches up with it's target movement in the y axis.

	public Transform target;

	void Start ()
	{
		GameEvents.GameEventManager.registerListener(this);
	}

	void Update ()
	{
		TrackTarget();
	}

	void TrackTarget ()
	{
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = target.transform.position.x;
		float targetY = target.transform.position.y;

		// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
		targetX = Mathf.Lerp(transform.position.x, target.position.x, xSmooth * Time.deltaTime);

		// ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
		targetY = Mathf.Lerp(transform.position.y, target.position.y, ySmooth * Time.deltaTime);

		// Set the camera's position to the target position with the same z component.
		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}

	public void receiveEvent(GameEvents.GameEvent e)
	{
		if(e.GetType().Name.Equals("EnterRoom"))
		{
			target = ((EnterRoom) e).getNewRoom();
		}
		
	}
}
