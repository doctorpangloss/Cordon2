using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Pathfinding;
using System.Linq;

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
		}

		void ChooseWaypoint ()
		{
			destinationWaypoint = waypoints [UnityEngine.Random.Range (0, waypoints.Length - 1)];
			path = null;
			currentDirection = Vector3.zero;
			currentWaypointIndex = 0;
			seeker.StartPath (transform.position, destinationWaypoint.position, OnPathComplete);
		}

		void FindWaypoints ()
		{
			waypoints = waypoints.Concat (GameObject.FindObjectsOfType<PatientZeroWaypoint>()
				.Select<PatientZeroWaypoint, Transform> (go => go.transform))
				.Distinct ()
				.ToArray ();
		}

		void OnPathComplete (Path p)
		{
			if (p.error) {
				Debug.LogError ("Path error!");
				enabled = false;
				return;
			}

			path = p;
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
			var cardinalDirection = cardinalDirections
				.Select<Vector3, Vector3Dot> (_cardinalDirection => new Vector3Dot () {dot = Vector3.Dot(_cardinalDirection, naturalDirection), direction = _cardinalDirection})
				.Max ()
				.direction;

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

