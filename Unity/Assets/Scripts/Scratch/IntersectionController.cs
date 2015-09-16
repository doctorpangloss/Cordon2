using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace Scratch
{
	public class IntersectionController : NetworkBehaviour
	{
		public Collider block;
		// Use SyncVar to ensure that players who join late are given the correct blocking state
		[SyncVar]
		public bool
			blocking;
	
		// Update is called once per frame
		void Update ()
		{
			if (block.enabled != blocking) {
				// Happens on both the client and the server
				block.enabled = blocking;

				if (isServer) {
					AstarPath.active.Scan ();
					PatientZeroController.instance.WorldChanged ();
				}
			}
		}
	}
}
