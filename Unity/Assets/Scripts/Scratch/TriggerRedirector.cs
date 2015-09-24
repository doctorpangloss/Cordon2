using UnityEngine;
using System.Collections;

namespace Scratch
{
	public class TriggerRedirector : MonoBehaviour
	{
		public GameObject target;
		public string onTriggerEnterMessage = "OnTriggerEnter";
		public string onTriggerExitMessage = "OnTriggerExit";

		void OnTriggerEnter (Collider other)
		{
			target.SendMessage (onTriggerEnterMessage, other, SendMessageOptions.RequireReceiver);
		}

		void OnTriggerExit (Collider other)
		{
			target.SendMessage (onTriggerExitMessage, other, SendMessageOptions.RequireReceiver);
		}
	}
}