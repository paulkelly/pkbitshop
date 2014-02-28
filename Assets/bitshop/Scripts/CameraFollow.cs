using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour, GameEvents.GameEventListener
{
	public float xSmooth = 20f;		// How smoothly the camera catches up with it's target movement in the x axis.
	public float ySmooth = 20f;		// How smoothly the camera catches up with it's target movement in the y axis.

	public Vector2 offset = new Vector2(0, -1f);

	public Transform target;

	const float NORMAL_ASPECT = 4/3f;
	const float COMPUTER_WIDE_ASPECT = 16/10f;
	const float EPSILON = 0.01f;

	float jiggleAmt = 0f;
	bool shake = false;
	
	void Start ()
	{
		GameEvents.GameEventManager.registerListener(this);

		float aspectRatio = Screen.width / ((float)Screen.height);
		
		if (Mathf.Abs(aspectRatio - NORMAL_ASPECT) < EPSILON)
		{
			camera.rect = new Rect(0f, 0.125f, 1f, 0.75f); // 16:9 viewport in a 4:3 screen res
		}
		else if (Mathf.Abs(aspectRatio - COMPUTER_WIDE_ASPECT) < EPSILON)
		{
			camera.rect = new Rect(0f, 0.05f, 1f, 0.9f); // 16:9 viewport in a 16:10 screen res
		}
	}

	void Update ()
	{
		if(jiggleAmt>0)
		{
			float quakeAmt = Random.value*jiggleAmt*2 - jiggleAmt;
			Vector3 pp = transform.position;
			pp.y+= quakeAmt; // can also add to x and/or z

			quakeAmt = Random.value*jiggleAmt*2 - jiggleAmt;
			pp.x+= quakeAmt;
			transform.position = pp;
		}
		else
		{
			TrackTarget();
		}
	}

	void TrackTarget ()
	{
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = target.transform.position.x + offset.x;
		float targetY = target.transform.position.y + offset.y;

		// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
		targetX = Mathf.Lerp(transform.position.x, targetX, xSmooth * Time.deltaTime);

		// ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
		targetY = Mathf.Lerp(transform.position.y, targetY, ySmooth * Time.deltaTime);

		// Set the camera's position to the target position with the same z component.
		transform.position = new Vector3(targetX, targetY, transform.position.z);

	}

	public void receiveEvent(GameEvents.GameEvent e)
	{
		if(e.GetType().Name.Equals("EnterRoom"))
		{
			target = ((EnterRoom) e).getNewRoom();
		}
		if(e.GetType ().Name.Equals("CameraShake"))
		{
			jiggleAmt = ((CameraShake) e).getAmount();
			StartCoroutine(jiggleCam2(((CameraShake) e).getDuration()));
		}
		
	}

	IEnumerator jiggleCam2(float duration) {
		yield return new WaitForSeconds(duration);
		jiggleAmt=0;
	}

}
