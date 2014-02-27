using UnityEngine;
using System.Collections;

public class FindPlayer : MonoBehaviour {

	public GameObject player;

	public static FindPlayer Instance { get; private set; }

	void Awake()
	{
		Instance = this;
	}

	public GameObject getPlayerObject()
	{
		return player;
	}
}
