using UnityEngine;

[RequireComponent(typeof(PlayerShip2D))]
public class ShooterUserControl : MonoBehaviour 
{
	private PlayerShip2D character;

	private Direction shootDirection;

	void Awake()
	{
		character = GetComponent<PlayerShip2D>();
	}

    void Update ()
    {
        // Read the jump input in Update so button presses aren't missed.
#if CROSS_PLATFORM_INPUT
        //if (CrossPlatformInput.GetButtonDown("Jump")) jump = true;
#else
		//if (Input.GetButtonDown("Jump")) jump = true;
#endif

    }

	void FixedUpdate()
	{
		// Read the inputs.
		bool shooting = false;
		#if CROSS_PLATFORM_INPUT
		float h = CrossPlatformInput.GetAxis("Horizontal");
		float v = CrossPlatformInput.GetAxis("Vertical");

		if (CrossPlatformInput.GetButton("FireLeft"))
		{
			shooting = true;
			shootDirection = Direction.LEFT;
		}
		else if (CrossPlatformInput.GetButton("FireRight"))
		{
			shooting = true;
			shootDirection = Direction.RIGHT;
		} 

		#else
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		#endif

		// Pass all parameters to the character control script.
		character.Move( h, v );

		if (shooting) 
		{
			character.shoot (shootDirection);
		}
	}
}
