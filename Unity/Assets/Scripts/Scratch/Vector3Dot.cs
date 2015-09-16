using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Pathfinding;
using System.Linq;

namespace Scratch
{
	public struct Vector3Dot : System.IComparable<Vector3Dot>
	{
		public float dot;
		public Vector3 direction;
		
		public int CompareTo (Vector3Dot other)
		{
			return this.dot.CompareTo (other.dot);
		}
	}
}