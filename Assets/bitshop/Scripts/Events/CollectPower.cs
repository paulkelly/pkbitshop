using UnityEngine;
using System.Collections;

public class CollectPower : GameEvents.GameEvent {

	private int type;

	public CollectPower(int type)
	{
		this.type = type;
	}

	public int getPowerupType()
	{
		return type;
	}
}
	
