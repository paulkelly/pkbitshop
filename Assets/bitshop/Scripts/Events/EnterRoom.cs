using UnityEngine;
using System.Collections;

public class EnterRoom : GameEvents.GameEvent {

	private Transform newRoom;

	public EnterRoom(Transform newRoom)
	{
		this.newRoom = newRoom;
	}

	public Transform getNewRoom()
	{
		return newRoom;
	}
}
