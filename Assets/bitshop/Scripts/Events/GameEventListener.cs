using UnityEngine;
using System.Collections;

namespace GameEvents
{
	public interface GameEventListener
	{
		void receiveEvent(GameEvent e);
	}
}