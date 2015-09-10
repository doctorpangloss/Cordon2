using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace Scratch
{
	public class MoveController : NetworkBehaviour
	{
		CharacterController controller;

		public float speed = 0.85f;


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
			controller.Move(transform.forward * Time.fixedDeltaTime * speed);
		}

		void SwipeNorth ()
		{
			transform.rotation = Quaternion.Euler (0f, 0f, 0f);
		}

		void SwipeWest ()
		{
			transform.rotation = Quaternion.Euler (0f, -90f, 0f);
		}

		void SwipeEast ()
		{
			transform.rotation = Quaternion.Euler (0f, 90f, 0f);
		}

		void SwipeSouth ()
		{
			transform.rotation = Quaternion.Euler (0f, 180f, 0f);
		}
	}
}