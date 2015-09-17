using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace Scratch
{
	public class GameController : NetworkBehaviour
	{
		public static GameController instance;
		[Header("Options")]
		public float
			maxSeconds = 120f;
		[Header("Runtime")]
		[SyncVar]
		public float
			gameStartTime;

		void Awake ()
		{
			instance = this;
		}

		// Use this for initialization
		void Start ()
		{
			Application.targetFrameRate = 60;
		}

		public override void OnStartServer ()
		{
			base.OnStartServer ();
			// TODO: Synchronize time
			gameStartTime = Time.time;
		}

		void OnGUI ()
		{

		}
	}
}
