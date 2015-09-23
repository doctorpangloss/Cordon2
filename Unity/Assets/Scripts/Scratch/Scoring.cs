using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace Scratch
{
	public class Scoring : NetworkBehaviour
	{
		public static Scoring instance;
		[Header("Options")]
		public float
			blockPoints = 10000;
		public float deathPoints = -8000;
		public float timePoints = 1000;
		[Header("Runtime")]
		[SyncVar]
		public int
			blocks;
		[SyncVar]
		public int
			deaths;

		void Awake ()
		{
			instance = this;
		}

		public int Score {
			get {
				return (int)Mathf.Floor (blockPoints * blocks + deathPoints * deaths + timePoints * GameController.instance.seconds);
			} 
		}
	}
}