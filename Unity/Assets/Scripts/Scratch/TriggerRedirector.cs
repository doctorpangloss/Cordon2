using UnityEngine;
using System.Collections;

namespace Scratch
{
	public class TriggerRedirector : MonoBehaviour
	{
		public GameObject target;

		void OnTriggerEnter (Collider other)
		{
			target.SendMessage ("OnTriggerEnter", other, SendMessageOptions.RequireReceiver);
		}

		void OnTriggerExit (Collider other)
		{
			target.SendMessage ("OnTriggerExit", other, SendMessageOptions.RequireReceiver);
		}
	}
}