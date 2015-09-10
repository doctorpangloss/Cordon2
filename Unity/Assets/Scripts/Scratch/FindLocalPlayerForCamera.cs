using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace Scratch
{
	public class FindLocalPlayerForCamera : NetworkBehaviour
	{
		public SmoothDampXZCamera smoothDampingCamera;
	
		// Update is called once per frame
		void Update ()
		{
			if (smoothDampingCamera == null) {
				return;
			}

			// TODO: Convert to player entity
			var moveControllers = FindObjectsOfType<MoveController> ();

			if (moveControllers == null
				|| moveControllers.Length == 0) {
				return;
			}

			for (var i = 0; i < moveControllers.Length; i++) {
				if (moveControllers [i].isLocalPlayer) {
					smoothDampingCamera.target = moveControllers [i].transform;
					enabled = false;
					break;
				}
			}

		}
	}

}