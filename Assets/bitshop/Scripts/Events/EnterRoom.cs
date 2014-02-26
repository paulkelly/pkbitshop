using UnityEngine;
using System.Collections;

public class EnterRoom : GameEvents.GameEvent {

	private Transform newRoom;
	private bool firstTime;

	public EnterRoom(Transform newRoom, bool firstTime)
	{
		this.newRoom = newRoom;
		this.firstTime = firstTime;
	}

	public Transform getNewRoom()
	{
		return newRoom;
	}

	public bool isFirstTime()
	{
		return firstTime;
	}
}
