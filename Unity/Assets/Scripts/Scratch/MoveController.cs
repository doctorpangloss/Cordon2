using UnityEngine;
using System.Collections;

namespace Scratch
{
	public class MoveController : MonoBehaviour
	{
		Animator animator;

		public float speed = 0.85f;

		// Use this for initialization
		void Start ()
		{
			animator = GetComponent<Animator> ();
			animator.SetFloat ("Speed_f", speed);
		}
	
		// Update is called once per frame
		void Update ()
		{

		}

		void SwipeNorth ()
		{
			transform.rotation = Quaternion.Euler (0f, 0f, 0f);
		}

		void SwipeWest ()
		{
			transform.rotation = Quaternion.Euler (0f, -90f, 0f);
		}

		void SwipeEast ()
		{
			transform.rotation = Quaternion.Euler (0f, 90f, 0f);
		}

		void SwipeSouth ()
		{
			transform.rotation = Quaternion.Euler (0f, 180f, 0f);
		}
	}
}