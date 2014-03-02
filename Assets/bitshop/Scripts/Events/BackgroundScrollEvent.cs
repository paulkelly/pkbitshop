using UnityEngine;
using System.Collections;

public class BackgroundScrollEvent : GameEvents.GameEvent {

	public float MoveX {get; set;}
	public float MoveY {get; set;}
	
	public BackgroundScrollEvent (float x, float y)
	{
		MoveX = x;
		MoveY = y;
	}
}
