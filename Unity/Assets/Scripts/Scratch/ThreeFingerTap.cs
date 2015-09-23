using UnityEngine;
using System.Collections;
using System.Linq;

namespace Scratch
{
	public class ThreeFingerTap : MonoBehaviour
	{
		public int fingers = 3;
		public string tapMessage = "OnFingerTap";
	
		// Update is called once per frame
		void Update ()
		{
			if (Input.touchCount == fingers
				&& Input.touches.Any (t => t.phase == TouchPhase.Began)) {
				BroadcastMessage (tapMessage, fingers, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}