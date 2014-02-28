using UnityEngine;
using System.Collections;

public class Restarter : MonoBehaviour, GameEvents.GameEventListener {

	void Awake()
	{
		GameEvents.GameEventManager.registerListener(this);
	}

	public void receiveEvent(GameEvents.GameEvent e)
	{
		if(e.GetType().Name.Equals("Restart"))
		{
			Application.LoadLevel(1);
		}
		if(e.GetType().Name.Equals("Win"))
		{
			Invoke ("TriggerWin", 3f);
		}
	}

	void TriggerWin()
	{
		Application.LoadLevel(2);
	}

	void onDestroy()
	{
		GameEvents.GameEventManager.unregisterListener (this);
	}

	void Update()
	{
		if(CrossPlatformInput.GetButton("Enter")) Debug.Log ("Enter pressed");
		if(CrossPlatformInput.GetButton("Enter") && Application.loadedLevel != 0)
		{
			Application.LoadLevel(0);
		}
	}
}
