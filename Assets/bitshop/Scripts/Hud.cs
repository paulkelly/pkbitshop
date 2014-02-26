using UnityEngine;
using System.Collections;

public class Hud : MonoBehaviour, GameEvents.GameEventListener {

	public GameObject playerShip;
	private PlayerShip2D playerShipComponent;
	public GameObject shieldValue;
	public Sprite[] numbers;

	public Sprite noCharge;
	public Sprite charge;

	public GameObject[] shieldCharges;

	// Use this for initialization
	void Start () {
		GameEvents.GameEventManager.registerListener(this);
		playerShipComponent = playerShip.GetComponent<PlayerShip2D>();
	}

	void updateHud()
	{
		int shield = playerShipComponent.getShield();
		SpriteRenderer spriteRenderer = shieldValue.GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = numbers[shield];

		int charges = playerShipComponent.getShieldCharge();

		for(int i=0; i<shieldCharges.Length; i++)
		{
			if(i < charges)
			{
				shieldCharges[i].GetComponent<SpriteRenderer>().sprite = charge;
			}
			else
			{
				shieldCharges[i].GetComponent<SpriteRenderer>().sprite = noCharge;
			}
		}
	}

	public void receiveEvent(GameEvents.GameEvent e)
	{
		if(e.GetType().Name.Equals("UpdateHud"))
		{
			updateHud();
		}
		
	}
}
