using UnityEngine;
using System.Collections;

namespace Scratch
{
	public class Killable : MonoBehaviour
	{
		public string OnKillMessage = "OnKill";

		public void Kill ()
		{
			BroadcastMessage (OnKillMessage);
		}
	}

}