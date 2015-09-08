using UnityEngine;
using System.Collections;

namespace Scratch
{
	public class MakeRun : MonoBehaviour
	{
		public float resetAfterSeconds;
		public float delay;
		Vector3 startPosition;
		// Use this for initialization
		IEnumerator Start ()
		{
			startPosition = transform.position;
			yield return new WaitForSeconds(delay);
			Set ();
			StartCoroutine (Reset ());
		}

		IEnumerator Reset ()
		{
			yield return new WaitForSeconds (resetAfterSeconds);
			transform.position = startPosition;
			Set ();
			StartCoroutine (Reset ());
		}

		void Set ()
		{
			GetComponent<Animator> ().SetFloat ("Speed_f", 1f);
		}
	}
}
