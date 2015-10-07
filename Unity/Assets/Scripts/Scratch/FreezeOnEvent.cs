using UnityEngine;
using System.Collections;

namespace Scratch
{
	public class FreezeOnEvent : MonoBehaviour
	{
		public GameObject objectToMove;
		public float positionY = 4f;
		public void AnimationEventOnFreeze()
		{
			var position = objectToMove.transform.position;
			position.y = positionY;
			objectToMove.transform.position = position;
		}
	}
}