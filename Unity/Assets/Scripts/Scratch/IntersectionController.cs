using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Scratch
{
	public class IntersectionController : NetworkBehaviour
	{
		public Collider block;
		public Collider trigger;
		public GameObject fence;
		public GameObject exitBlock;
		// Use SyncVar to ensure that players who join late are given the correct blocking state
		[SyncVar]
		public bool
			blocking;
		public HashSet<IntersectionTriggerable> occupants = new HashSet<IntersectionTriggerable> ();

		void Start ()
		{
			SetBlock (blocking);
		}

		// Update is called once per frame
		void Update ()
		{
			if (block.enabled != blocking) {
				SetBlock (blocking);
			}
		}

		void SetBlock (bool blocking)
		{
			// Happens on both the client and the server
			block.enabled = blocking;
			fence.SetActive (blocking);
			
			if (isServer) {
				AstarPath.active.Scan ();
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

			occupants.Add (triggerable);
			if (isServer
				&& blocking != true) {
				blocking = true;
			}
		}

		void OnEntranceTriggerExit (Collider other)
		{
			var triggerable = other.gameObject.GetComponent<IntersectionTriggerable> ();
			if (triggerable == null) {
				return;
			}

			occupants.Remove (triggerable);
			if (isServer
				&& blocking == true
				&& occupants.Count == 0) {
				blocking = false;
			}
		}
	}
}
