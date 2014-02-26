﻿using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour{

	private static BulletManager bulletManager = null;

	public GameObject bullet;

	void Awake()
	{
		bulletManager = this;
	}

	public static BulletManager getInstance()
	{
		return bulletManager;
	}

	public void spawnBullet(Vector3 position, Vector3 direction, bool firedByPlayer)
	{
		GameObject spawnedBullet = (GameObject) Instantiate(bullet, position, Quaternion.identity);
		
		Bullet bulletComponent = spawnedBullet.GetComponent<Bullet>();
		bulletComponent.setDirection (direction);
		if(firedByPlayer) bulletComponent.setPlayerBullet();
	}

}