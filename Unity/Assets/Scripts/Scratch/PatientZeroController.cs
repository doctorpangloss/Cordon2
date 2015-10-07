using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Pathfinding;

namespace Scratch
{
	public class PatientZeroController : NetworkBehaviour
	{
		public static PatientZeroController instance {
			get;
			private set;
		}

		public float speed = 8f;
		public Transform[] waypoints;
		public float minTimeOnDirection = 1f;
		public float waypointChoosingDelay = 1f;
		[Header("Runtime")]
		public Transform
			destinationWaypoint;
		public int currentWaypointIndex;
		public Path path;
		public float nextWaypointDistance = 1f;
		public Vector3 currentDirection = Vector3.zero;
		public float timeOnDirection = 0;
		CharacterController controller;
		Seeker seeker;
		Vector3[] cardinalDirections = new Vector3[] {
			new Vector3 (1, 0, 0),
			new Vector3 (0, 0, 1),
			new Vector3 (-1, 0, 0),
			new Vector3 (0, 0, -1)
		};

		void Awake ()
		{
			instance = this;
		}

		public void WorldChanged (bool isGraphUpdated = true)
		{
			// TODO: What happens when patient zero hits an intersections?
			// Note, the graph has been updated at this point
			ChooseWaypoint ();
		}

		// Use this for initialization
		void Start ()
		{
			// AI is disabled on clients
			if (!isServer) {
				enabled = false;
				return;
			}

			controller = GetComponent<CharacterController> ();
			seeker = GetComponent<Seeker> ();

			FindWaypoints ();

			ChooseWaypoint ();

			GameController.instance.DidGameOver += HandleOnGameOver;
		}

		void HandleOnGameOver ()
		{
			enabled = false;
		}

		void ChooseWaypoint ()
		{
			if (waypoints.Length <= 2) {
				return;
			}

			destinationWaypoint = waypoints [UnityEngine.Random.Range (0, waypoints.Length - 1)];
			path = null;
			currentDirection = Vector3.zero;
			currentWaypointIndex = 0;
			seeker.StartPath (transform.position, destinationWaypoint.position, OnPathComplete);
		}

		void FindWaypoints ()
		{
			var sceneWaypoints = GameObject.FindObjectsOfType<PatientZeroWaypoint> ();
			waypoints = new Transform[sceneWaypoints.Length];
			for (var i = 0; i < waypoints.Length; i++) {
				waypoints [i] = sceneWaypoints [i].transform;
			}
		}

		void OnPathComplete (Path p)
		{
			if (p.error) {
				StartCoroutine (DelayedChooseWaypoint ());
				return;
			}

			path = p;
		}

		IEnumerator DelayedChooseWaypoint ()
		{
			yield return new WaitForSeconds (waypointChoosingDelay);
			ChooseWaypoint ();
		}

		void FixedUpdate ()
		{
			if (path == null) {
				return;
			}


			if (currentWaypointIndex >= path.vectorPath.Count) {
				ChooseWaypoint ();
				return;
			}


			// Find the direction closest towards the next waypoint
			var nextWaypoint = path.vectorPath [currentWaypointIndex];
			var naturalDirection = (nextWaypoint - transform.position).normalized;
			var maxCardinalDirection = new Vector3Dot();

			for (var i = 0; i < cardinalDirections.Length; i++) {
				var _cardinalDirection = cardinalDirections[i];
				var temp = new Vector3Dot () {
					dot = Vector3.Dot(_cardinalDirection, naturalDirection),
					direction = _cardinalDirection
				};
				if (temp.dot > maxCardinalDirection.dot) {
					maxCardinalDirection = temp;
				}
			}

			var cardinalDirection = maxCardinalDirection.direction;

			// Ensure that we don't zig zag really fast
			if (currentDirection != cardinalDirection
				&& timeOnDirection >= minTimeOnDirection) {
				timeOnDirection = 0f;
				currentDirection = cardinalDirection;
			}

			// Turn
			if (currentDirection != Vector3.zero) {
				transform.forward = currentDirection;
			}

			// Move
			controller.Move (transform.forward * speed * Time.fixedDeltaTime);

			// Update timers
			timeOnDirection += Time.fixedDeltaTime;

			// Have we arrived?
			if (Vector3.Distance (transform.position, nextWaypoint) < nextWaypointDistance) {
				currentWaypointIndex++;
			}
		}
	}
}

