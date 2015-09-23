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
			seconds;
		public string gameOverMessage;
		public bool started;
		public bool gameOver;
		public event System.Action OnGameOver;

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
			started = true;
			gameOver = false;
			seconds = maxSeconds;
			StartCoroutine (Tick (1f));
		}

		IEnumerator Tick (float tick)
		{
			while (seconds > 0) {
				yield return new WaitForSeconds (tick);
				seconds -= tick;
			}

			BroadcastMessage (gameOverMessage, SendMessageOptions.DontRequireReceiver);
			gameOver = true;
			if (OnGameOver != null) {
				OnGameOver ();
			}
		}
	}
}
