using UnityEngine;
using System.Collections;

namespace Scratch
{
	public class SmoothDampXZCamera : MonoBehaviour
	{
		[Header("Options")]
		public Transform
			target;
		public float smoothTime = 0.3f;
		[Header("Runtime")]
		public Vector3
			difference;
		public Vector3 velocity;

		// Use this for initialization
		void Start ()
		{
			difference = transform.position - target.position;
		}
	
		// Update is called once per frame
		void LateUpdate ()
		{
			transform.position = Vector3.SmoothDamp (transform.position, target.transform.position + difference, ref velocity, smoothTime);
		}
	}

}