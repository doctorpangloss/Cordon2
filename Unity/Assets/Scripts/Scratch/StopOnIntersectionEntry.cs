using UnityEngine;
using System.Collections;

namespace Scratch
{
	public class StopOnIntersectionEntry : MonoBehaviour
	{
		public enum Direction
		{
			North,
			South,
			East,
			West
		}

		[Header("Options")]
		public MoveController
			moveController;
		public LayerMask triggerLayers;
		public float resetDistance = 10f;
		[Tooltip("How much distance should the object move until it stops?")]
		public float
			brakingDistance = 4f;
		[Header("Runtime")]
		public bool
			braking = false;
		public float distanceTraveledSinceStop = 0f;
		public float distanceSinceLastReset = 0f;
		public bool ignoring = false;
		public Vector3 lastPosition;
		public bool moved = false;
		public Direction interruptIgnoreDirection = Direction.North;

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

			// Are we ignoring?
			if (ignoring) {
				return;
			}

			braking = true;
			distanceTraveledSinceStop = 0;
		}

		// Update is called once per frame
		void FixedUpdate ()
		{
			var motion = (lastPosition - transform.position).magnitude;
			distanceTraveledSinceStop += motion;
			distanceSinceLastReset += motion;
			moved = !(lastPosition == transform.position);
			lastPosition = transform.position;

			if (ignoring
				&& distanceSinceLastReset >= resetDistance) {
				ignoring = false;
			}

			if (braking
				&& distanceTraveledSinceStop >= brakingDistance) {
				moveController.moving = false;
				braking = false;
			}
		}

		// If you swipe in any way, cancel your should stop
		void SwipeNorth ()
		{
			CheckReset (Direction.North);
		}

		void SwipeEast ()
		{
			CheckReset (Direction.East);
		}

		void SwipeSouth ()
		{
			CheckReset (Direction.South);
		}

		void SwipeWest ()
		{
			CheckReset (Direction.West);
		}

		Direction Opposite (Direction direction)
		{
			switch (direction) {
			case Direction.North:
				return Direction.South;
			case Direction.East:
				return Direction.West;
			case Direction.West:
				return Direction.East;
			case Direction.South:
				return Direction.North;
			}
			return Direction.North;
		}

		void CheckReset (Direction direction)
		{
			// Are we swiping while breaking?
			if (braking
				|| !moved) {
				braking = false;
				ignoring = true;
				distanceSinceLastReset = 0f;
				// When we swipe in the opposite of the direction we swiped during an ignore,
				// we stop ignoring.
				interruptIgnoreDirection = Opposite (direction);
			}

			// Are we ignoring? We have to check if we should interrupt an ignore
			if (ignoring
				&& (direction == interruptIgnoreDirection)) {
				// Stop ignoring braking
				ignoring = false;
			}

			// Resume moving in any case
			braking = false;
		}
	}
}
