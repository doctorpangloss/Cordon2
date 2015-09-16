using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace Scratch
{
	public class MoveController : NetworkBehaviour
	{
		CharacterController controller;
		public float speed = 0.85f;
		public bool moving = true;

		// Use this for initialization
		void Start ()
		{
			// Move controller is disabled for non-local players.
			if (!isLocalPlayer) {
				enabled = false;
				return;
			}

			controller = GetComponent<CharacterController> ();
		}
	
		// Update is called once per frame
		void FixedUpdate ()
		{
			if (moving) {
				controller.Move (transform.forward * Time.fixedDeltaTime * speed);
			} else {
				controller.Move (Vector3.zero);
			}
		}

		void SwipeNorth ()
		{
			moving = true;
			transform.rotation = Quaternion.Euler (0f, 0f, 0f);
		}

		void SwipeWest ()
		{
			moving = true;
			transform.rotation = Quaternion.Euler (0f, -90f, 0f);
		}

		void SwipeEast ()
		{
			moving = true;
			transform.rotation = Quaternion.Euler (0f, 90f, 0f);
		}

		void SwipeSouth ()
		{
			moving = true;
			transform.rotation = Quaternion.Euler (0f, 180f, 0f);
		}
	}
}