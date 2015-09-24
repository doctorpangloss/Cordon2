using UnityEngine;
using System.Collections;

namespace Scratch
{
	public class Killable : MonoBehaviour
	{
		public string OnKillMessage = "OnKill";
		public string OnResurrectMessage = "OnResurrect";

		public void Kill ()
		{
			BroadcastMessage (OnKillMessage, SendMessageOptions.DontRequireReceiver);
		}

		public void Resurrect ()
		{
			BroadcastMessage (OnResurrectMessage, SendMessageOptions.DontRequireReceiver);
		}
	}

}