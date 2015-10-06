using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Scratch
{
	public class ConstructableIntersection : NetworkBehaviour
	{
		[Header("Options")]
		public float constructionCost = 10f;
		public float constructionRate = 1f;
		public float constructionTickRate = 0.5f;
		public float depletionRate = 0.5f;
		public Collider block;

		[Header("Animation")]
		public GameObject fallingBricks;
		public Animator fallingBricksAnimator;
		public string fallingBricksAnimationTrigger = "falling_t";
		public string fallingBricksAnimationSpeedParameter = "speed_f";
		public int fallingBricksAnimationFrames = 5008;

		// Use SyncVar to ensure that players who join late are given the correct blocking state
		[Header("Runtime")]
		[SyncVar]
		public bool
			blocking;
		[SyncVar]
		public float
			accumulatedConstruction;
		public HashSet<IntersectionTriggerable> constructions = new HashSet<IntersectionTriggerable> ();

		GameController gameController;

		void Start ()
		{
			gameController = GameController.instance;
			SetBlock (blocking);

			gameController.DidGameStart += HandleDidGameStart;
		}

		void HandleDidGameStart ()
		{
			if (isServer) {
				StartCoroutine (TickConstruction ());
			}
		}

		IEnumerator TickConstruction ()
		{
			while (enabled
			       && !gameController.gameOver) {
				yield return new WaitForSeconds (constructionTickRate);

				if (constructions.Count > 0) {
					var addition = 0f;

					foreach (var triggerable in constructions) {
						addition += constructionRate * constructionTickRate;
					}

					accumulatedConstruction = Mathf.Clamp (0f, constructionCost, accumulatedConstruction + addition);
				} else {
					// Unattended constructions deplete
					accumulatedConstruction = Mathf.Clamp (0f, constructionCost, accumulatedConstruction - depletionRate * constructionTickRate);
				}
			}
		}
	
		// Update is called once per frame
		void Update ()
		{
			// Should we construct the block
			if (isServer
				&& !blocking
				&& accumulatedConstruction >= constructionCost) {
				blocking = true;
			}

			// Client and server: when blocking is true, make sure the block is set
			if (blocking != block.enabled) {
				SetBlock (blocking);
			}
		}
	
		void UpdateAnimationSpeed()
		{
			// Using the amount of time left and the current rate of construction, calculate what speed to set for the animation
			if (fallingBricksAnimator == null) {
				return;
			}

			// How fast are we building?
			var rateOfConstruction = constructions.Count * constructionRate * Time.deltaTime;
			// How much construction is remaining?
			var constructionRemaining = constructionCost - accumulatedConstruction;
			// How much animation is remaining?
			var normalizedTime = fallingBricksAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
			var remainingNormalized = 1 - (normalizedTime - Mathf.Floor(normalizedTime));


			var seconds = (constructionCost - accumulatedConstruction) / rateOfConstruction;
		}

		void SetBlock (bool blocking)
		{
			// Happens on both the client and the server
			block.enabled = blocking;
		
			if (isServer) {
				AstarPath.active.UpdateGraphs (block.bounds);
				if (PatientZeroController.instance != null) {
					PatientZeroController.instance.WorldChanged (true);
				}
			}
		}
	
		/// <summary>
		/// Handler for the entrance trigger. See the Trigger Redirector component on the Entrance Trigger object to see why this is called.
		/// </summary>
		/// <param name="other">Other.</param>
		void OnEntranceTriggerEnter (Collider other)
		{
			var triggerable = other.gameObject.GetComponent<IntersectionTriggerable> ();
			if (triggerable == null) {
				return;
			}
		
			constructions.Add (triggerable);
		}
	
		void OnEntranceTriggerExit (Collider other)
		{
			var triggerable = other.gameObject.GetComponent<IntersectionTriggerable> ();
			if (triggerable == null) {
				return;
			}
		
			constructions.Remove (triggerable);
		}
	}
}
