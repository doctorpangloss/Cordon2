using UnityEngine;
using System.Collections;

namespace Scratch
{
	public class KillOnCollision : MonoBehaviour
	{
		public LayerMask killableLayer;
		[Header("Runtime")]
		public int
			kills = 0;
		public GameObject hitThisFrame;

		void OnCollisionEnter (Collision collision)
		{
			OnCollide (collision.gameObject);
		}

		void OnTriggerEnter (Collider other)
		{
			OnCollide (other.gameObject);
		}

		void OnControllerColliderHit (ControllerColliderHit hit)
		{
			OnCollide (hit.gameObject);
		}

		void LateUpdate ()
		{
			hitThisFrame = null;
		}

		void OnCollide (GameObject other)
		{
			if (other == hitThisFrame) {
				return;
			}

			hitThisFrame = other;

			// Did we fail to hit something we can kill?
			if (((1 << other.layer) & killableLayer.value) == 0) {
				return;
			}

			// Otherwise, we hit something we can kill. Check it has a killable component
			var killable = other.GetComponent<Killable> ();
			if (killable == null) {
				return;
			}

			killable.Kill ();
			kills++;
		}
	}
}