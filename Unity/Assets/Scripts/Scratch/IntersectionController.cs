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
		// Use SyncVar to ensure that players who join late are given the correct blocking state
		[SyncVar]
		public bool
			blocking;
		public HashSet<IntersectionTriggerable> occupants = new HashSet<IntersectionTriggerable> ();


		// Update is called once per frame
		void Update ()
		{
			if (block.enabled != blocking) {
				// Happens on both the client and the server
				block.enabled = blocking;

				if (isServer) {
					AstarPath.active.Scan ();
					if (PatientZeroController.instance != null) {
						PatientZeroController.instance.WorldChanged ();
					}
				}
			}
		}

		void OnTriggerEnter (Collider other)
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

		void OnTriggerExit (Collider other)
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
