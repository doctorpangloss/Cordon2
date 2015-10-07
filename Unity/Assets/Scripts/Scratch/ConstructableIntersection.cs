using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Scratch
{
	public class ConstructableIntersection : NetworkBehaviour
	{
		[Header("Options")]
		public float
			constructionCost = 10f;
		public float constructionRate = 1f;
		public float constructionTickRate = 0.5f;
		public float depletionRate = 0.5f;
		public Collider block;
		[Header("Animation")]
		public GameObject
			fallingBricks;
		public Animator fallingBricksAnimator;
		public float fallingBricksAnimationLength = 83.45f;
		public string fallingBricksAnimationBool = "falling_b";
		public string fallingBricksAnimationSpeedParameter = "speed_f";
		public string fallingBricksAnimationFrozenBool = "frozen_b";
		public string fallingBricksAnimationState = "Falling";

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
			HandleDidGameStart ();
			SetBlock (blocking);
		}

		void HandleDidGameStart ()
		{
			if (isServer) {
				StartCoroutine (TickConstruction ());
			}
		}

		IEnumerator TickConstruction ()
		{
			while (enabled) {
				yield return new WaitForSeconds (constructionTickRate);

				if (gameController.gameOver) {
					continue;
				}

				if (constructions.Count > 0) {
					var addition = constructionRate * constructionTickRate * constructions.Count;
					accumulatedConstruction = Mathf.Clamp (accumulatedConstruction + addition, 0f, constructionCost);
				} else if (constructions.Count == 0
					&& !blocking) {
					// Unattended constructions deplete
					accumulatedConstruction = Mathf.Clamp (accumulatedConstruction - depletionRate * constructionTickRate, 0f, constructionCost);
				}
			}
		}
	
		// Update is called once per frame
		void Update ()
		{
			// Animation
			SetAnimationStates ();
			UpdateAnimationSpeed ();
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

		void SetAnimationStates ()
		{
			var fallingBricksActive = accumulatedConstruction > 0f;
			fallingBricks.SetActive (fallingBricksActive);
			if (fallingBricksActive) {
				fallingBricksAnimator.SetBool (fallingBricksAnimationFrozenBool, blocking);
				fallingBricksAnimator.SetBool (fallingBricksAnimationBool, constructions.Count > 0
					&& accumulatedConstruction < constructionCost);
			}

		}
	
		void UpdateAnimationSpeed ()
		{
			// Using the amount of time left and the current rate of construction, calculate what speed to set for the animation
			if (fallingBricksAnimator == null) {
				return;
			}

			if (!fallingBricks.activeInHierarchy) {
				return;
			}

//			// If we're in the middle of construction, pause the animation
//			if (constructions.Count == 0) {
//				fallingBricksAnimator.SetFloat (fallingBricksAnimationSpeedParameter, 0f);
//				// TODO: Hide / unhide bricks that have not yet deployed.
//				return;
//			}

			if (accumulatedConstruction >= constructionCost) {
				return;
			}

			if (accumulatedConstruction == 0) {
				return;
			}

			var state = fallingBricksAnimator.GetCurrentAnimatorStateInfo (0);

			if (!state.IsName (fallingBricksAnimationState)
				&& constructions.Count > 0
				&& accumulatedConstruction < constructionCost) {
				return;
			}

			var realSecondsLength = fallingBricksAnimationLength;
			var rateOfConstruction = constructions.Count == 0 ? -depletionRate : constructions.Count * constructionRate;
			if (rateOfConstruction == 0) {
				return;
			}

			// How much construction is remaining?
			var normalizedTime = state.normalizedTime;

			var remainingNormalized = 1 - (normalizedTime - Mathf.Floor (normalizedTime));
			// Deal with depletion
			if (rateOfConstruction < 0) {
				remainingNormalized = normalizedTime;
			}


			// What speed do we need to set the animation in order to finish the construction at the right time?
			var animationSecondsRemaining = remainingNormalized * realSecondsLength;
			var remainingConstruction = (rateOfConstruction > 0) ? (constructionCost - accumulatedConstruction) : accumulatedConstruction;
			var constructionSecondsRemaining = Mathf.Abs (remainingConstruction / rateOfConstruction);

			// Calculate a new speed. We want the number of real seconds remaining to equal the construction seconds remaining
			var newSpeed = Mathf.Clamp (rateOfConstruction * (animationSecondsRemaining / constructionSecondsRemaining), -120f, 120f);
			fallingBricksAnimator.SetFloat (fallingBricksAnimationSpeedParameter, newSpeed);
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
