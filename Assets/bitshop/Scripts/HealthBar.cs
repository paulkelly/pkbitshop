using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	float percentHealth = 1f;
	Vector2 pos;
	
	public bool alwaysShow = false;
	
	float lastValue = 1f;
	bool visible = false;
	float displayTime = 2f;
	float displayRemain = 0f;
	
	float min = 0.05f;

	Enemy enemyScript;

	void Start ()
	{
		enemyScript = transform.parent.GetComponent<Enemy> ();
		
		if(!alwaysShow)
		{
			transform.GetComponent<SpriteRenderer>().color = new Color(transform.GetComponent<SpriteRenderer>().color.r, 
																		transform.GetComponent<SpriteRenderer>().color.g, 
																		transform.GetComponent<SpriteRenderer>().color.b, 0);
			transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(transform.GetComponent<SpriteRenderer>().color.r, 
			                                                                       transform.GetComponent<SpriteRenderer>().color.g, 
		                                                                       transform.GetComponent<SpriteRenderer>().color.b, 0);
		 }
	}
	
	void ShowHealthbar()
	{
		transform.GetComponent<SpriteRenderer>().color = new Color(transform.GetComponent<SpriteRenderer>().color.r, 
		                                                           transform.GetComponent<SpriteRenderer>().color.g, 
		                                                           transform.GetComponent<SpriteRenderer>().color.b, 100);
		transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(transform.GetComponent<SpriteRenderer>().color.r, 
		                                                                       transform.GetComponent<SpriteRenderer>().color.g, 
		                                                                       transform.GetComponent<SpriteRenderer>().color.b, 100);
	}
	
	void HideHealthBar()
	{
		Color currentColor = transform.GetComponent<SpriteRenderer>().color;
		Color newColor = new Color(currentColor.r,
									currentColor.g, 
									currentColor.b, 0);
		transform.GetComponent<SpriteRenderer>().color = Color.Lerp(currentColor, newColor, 0.1f);
		
		
		currentColor = transform.GetChild(0).GetComponent<SpriteRenderer>().color;
		newColor = new Color(currentColor.r,
		                     currentColor.g, 
		                     currentColor.b, 0);
		transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.Lerp(currentColor, newColor, 0.1f);
	}
	

	void FixedUpdate()
	{
		//pos = new Vector2 (transform.position.x, transform.position.y - 1f);
		
		percentHealth = 0f;

		if(enemyScript.health > 0)
		{ 
			percentHealth = enemyScript.health/enemyScript.maxHealth;
			if(percentHealth != lastValue)
			{
				visible = true;
				displayRemain = displayTime;
				lastValue = percentHealth;
			}
		}
		else
		{
			visible = false;
		}
		
		if(percentHealth < min) percentHealth = min;
		Vector3 theScale = new Vector3 (percentHealth, 1, 1);
		transform.GetChild (0).transform.localScale = theScale;

		theScale = transform.localScale;
		if(transform.parent.localScale.x < 0 && !(theScale.x < 0) ||
		   transform.parent.localScale.x > 0 && !(theScale.x > 0)) theScale.x *= -1;
		transform.localScale = theScale;
		
		if(alwaysShow) return;
		if(visible)
		{
			ShowHealthbar();
			if(displayRemain > 0)
			{
				displayRemain -= Time.deltaTime;
			}
			else
			{
				visible = false;
			}
		}
		else
		{
			HideHealthBar();
		}

	}

}
