using UnityEngine;
using System.Collections;

public class BackgroundScroll : MonoBehaviour, GameEvents.GameEventListener {

	public float speed = 0f;
	
	float amountToMoveX = 0f;
	float amountToMoveY = 0f;
	
	
	void Awake()
	{
		GameEvents.GameEventManager.registerListener(this);
	}
	
	// Update is called once per frame
	void Update () 
	{
		renderer.material.mainTextureOffset = new Vector2((amountToMoveX *speed)%1, (amountToMoveY *speed)%1);
	}
	
	public void receiveEvent(GameEvents.GameEvent e)
	{
		if(e.GetType().Name.Equals("BackgroundScrollEvent"))
		{
			amountToMoveX += ((BackgroundScrollEvent) e).MoveX;
			amountToMoveY += ((BackgroundScrollEvent) e).MoveY;
		}
	}
}
