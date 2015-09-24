using UnityEngine;
using System.Collections;

namespace Scratch
{
	public class ThreeFingerTap : MonoBehaviour
	{
		public int fingers = 3;
		public string tapMessage = "OnFingerTap";
	
		// Update is called once per frame
		void Update ()
		{

			if (Input.touchCount != fingers) {
				return;
			}
			var touches = Input.touches;
			var any = false;
			for (var i = 0; i < touches.Length; i++) {
				any = touches [i].phase == TouchPhase.Began;
			}

			if (!any) {
				return;
			}

			BroadcastMessage (tapMessage, fingers, SendMessageOptions.DontRequireReceiver);
		}
	}
}