using UnityEngine;
using System.Collections;


public class EnemySpawner : MonoBehaviour {

	public GameObject[] enemyList;

	public GameObject GetEnemy()
	{
		return enemyList[Random.Range(0, enemyList.Length)];
	}
}
