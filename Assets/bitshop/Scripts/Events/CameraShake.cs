using UnityEngine;
using System.Collections;

public class CameraShake : GameEvents.GameEvent {

	private float amount = 0.15f;
	private float duration = 0.2f;

	public CameraShake(float amount, float duration)
	{
		this.amount = amount;
		this.duration = duration;
	}
	public CameraShake ()
	{
	}

	public float getAmount()
	{
		return amount;
	}

	public float getDuration()
	{
		return duration;
	}
}
