﻿using UnityEngine;
using System.Collections;

namespace Scratch
{
	public class AnimationManager : MonoBehaviour
	{
		public string speedParameter;
		public Animator animator;
		public CharacterController controller;
	
		// Update is called once per frame
		void LateUpdate ()
		{
			if (animator == null
				|| controller == null) {
				return;
			}

			animator.SetFloat (speedParameter, controller.velocity.magnitude);
		}
	}
}
