using UnityEngine;
using System.Collections;

public class DamagePlayer : GameEvents.GameEvent {
	
	private int amount;
	private bool bounce = false;
	GameObject damager;
	
	public DamagePlayer(GameObject damager, int amount)
	{
		this.damager = damager;
		this.amount = amount;
	}

	public DamagePlayer(GameObject damager, int amount, bool bounce)
	{
		this.damager = damager;
		this.amount = amount;
		this.bounce = bounce;
	}

	public bool bounceAfterTakingDamage()
	{
		return bounce;
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
