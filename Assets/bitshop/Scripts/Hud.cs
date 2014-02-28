using UnityEngine;
using System.Collections;

public class Hud : MonoBehaviour, GameEvents.GameEventListener {

	public GameObject playerShip;
	private PlayerShip2D playerShipComponent;
	public GameObject shieldValue;
	public GameObject dmgValue;
	public GameObject rofValue;
	public Sprite[] numbers;

	public Sprite noCharge;
	public Sprite charge;

	public GameObject[] shieldCharges;

	// Use this for initialization
	void Start () {
		GameEvents.GameEventManager.registerListener(this);
		playerShipComponent = playerShip.GetComponent<PlayerShip2D>();

		Invoke("updateHud", 0.1f);
	}

	void updateHud()
	{
		int shield = playerShipComponent.getShield();
		int damage = playerShipComponent.getDamage()+1;
		int rateOfFire = playerShipComponent.getRateOfFire ()+1;

		SpriteRenderer spriteRenderer = shieldValue.GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = numbers[shield];

		spriteRenderer = dmgValue.GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = numbers[damage];

		spriteRenderer = rofValue.GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = numbers[rateOfFire];

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
