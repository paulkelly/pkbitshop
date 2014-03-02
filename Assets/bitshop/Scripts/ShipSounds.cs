using UnityEngine;
using System.Collections;

public class ShipSounds : MonoBehaviour {

	public float minPitch = 0.5f;
	public float maxPitch = 1.5f;

	public AudioClip[] gunshot;
	public float minGunshotVolume = 0.25f;
	public float maxGunshotVolume = 1f;

	public AudioClip[] takeDamage;
	public float minDmgVolume = 0.25f;
	public float maxDmgVolume = 1f;

	public AudioClip[] takeShieldDamage;
	public float minShieldVolume = 0.25f;
	public float maxShieldVolume = 1f;

	public AudioClip[] death;
	public float minDeathVolume = 0.25f;
	public float maxDeathVolume = 1f;
	
	public AudioClip[] shieldCharge;
	public float minshieldChargeVolume = 0.45f;
	public float maxshieldChargeVolume = 0.6f;
	
	public AudioClip[] powerup;
	public float minPowerupVolume = 0.8f;
	public float maxPowerupVolume = 1f;

	private void PlayRandomSound(AudioClip clip, float pitch, float volume)
	{
		audio.pitch = pitch;
		audio.volume = volume;
		audio.PlayOneShot(clip);
	}

	public void PlayGunshot()
	{
		PlayRandomSound (gunshot[Random.Range (0, gunshot.Length)], 
		                 Random.Range (minPitch, maxPitch), 
		                 Random.Range (minGunshotVolume, maxGunshotVolume));
	}

	public void PlayTakeDamage()
	{
		PlayRandomSound (takeDamage[Random.Range (0, takeDamage.Length)], 
		                 Random.Range (minPitch, maxPitch), 
		                 Random.Range (minDmgVolume, maxDmgVolume));
	}

	public void playTakeShieldDamage()
	{
		PlayRandomSound (takeShieldDamage[Random.Range (0, takeShieldDamage.Length)], 
		                 Random.Range (minPitch, maxPitch), 
		                 Random.Range (minShieldVolume, maxShieldVolume));
	}

	public void playDeath()
	{
		PlayRandomSound (death[Random.Range (0, death.Length)], 
		                 Random.Range (minPitch, maxPitch), 
		                 Random.Range (minDeathVolume, maxDeathVolume));
	}
	
	public void playShieldUp(int num)
	{
		PlayRandomSound (shieldCharge[num], 
		                 Random.Range (minPitch, maxPitch), 
		                 Random.Range (minshieldChargeVolume, maxshieldChargeVolume));
	}
	
	public void playPowerup()
	{
		PlayRandomSound (powerup[Random.Range (0, death.Length)], 
		                 Random.Range (minPitch, maxPitch), 
		                 Random.Range (minPowerupVolume, maxPowerupVolume));
	}

}
