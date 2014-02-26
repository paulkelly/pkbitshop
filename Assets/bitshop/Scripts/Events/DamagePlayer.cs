using UnityEngine;
using System.Collections;

public class DamagePlayer : GameEvents.GameEvent {
	
	private int amount;
	GameObject damager;
	
	public DamagePlayer(GameObject damager, int amount)
	{
		this.damager = damager;
		this.amount = amount;
	}
	
	public int getDamageValue()
	{
		return amount;
	}

	public GameObject getDamager()
	{
		return damager;
	}
}
