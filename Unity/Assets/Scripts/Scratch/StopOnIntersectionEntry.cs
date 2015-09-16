using UnityEngine;
using System.Collections;

namespace Scratch
{
	public class StopOnIntersectionEntry : MonoBehaviour
	{
		[Header("Options")]
		public MoveController
			moveController;
		public LayerMask triggerLayers;
		[Tooltip("How much distance should the object move until it stops?")]
		public float
			brakingDistance = 4f;
		[Header("Runtime")]
		public bool
			shouldStop = false;
		public float distanceTraveledSinceStop = 0f;
		public Vector3 lastPosition;
		// Use this for initialization
		void Start ()
		{
			if (moveController == null) {
				enabled = false;
				return;
			}
		}
	
		void OnTriggerEnter (Collider other)
		{
			// Did we enter any of the other layers?
			if (((1 << other.gameObject.layer) & triggerLayers.value) == 0) {
				return;
			}

			shouldStop = true;
			distanceTraveledSinceStop = 0;
		}

		// Update is called once per frame
		void FixedUpdate ()
		{
			distanceTraveledSinceStop += (lastPosition - transform.position).magnitude;
			lastPosition = transform.position;

			if (shouldStop
				&& distanceTraveledSinceStop >= brakingDistance) {
				moveController.moving = false;
				shouldStop = false;
			}
		}

		// If you swipe in any way, cancel your should stop
		void SwipeNorth ()
		{
			shouldStop = false;
		}

		void SwipeEast ()
		{
			shouldStop = false;
		}

		void SwipeSouth ()
		{
			shouldStop = false;
		}

		void SwipeWest ()
		{
			shouldStop = false;
		}
	}
}
