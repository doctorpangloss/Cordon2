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
			seconds = 120f;
		public string gameOverMessage;
		public bool started;
		public bool gameOver;
		public bool autostart = false;
		float startTime;

		public event System.Action DidGameOver;
		public event System.Action DidGameStart;

		void Awake ()
		{
			instance = this;
		}

		// Use this for initialization
		void Start ()
		{
			Application.targetFrameRate = 60;
			seconds = maxSeconds;
		}

		public override void OnStartServer ()
		{
			base.OnStartServer ();
			if (autostart) {
				StartGame ();
			}
		}

		public void StartGame ()
		{
			started = true;
			gameOver = false;
			seconds = maxSeconds;
			startTime = Time.time;
			StartCoroutine (Tick (1f));
			if (DidGameStart != null) {
				DidGameStart ();
			}
		}

		public float elapsedTime {
			get {
				return Time.time - startTime;
			}
		}

		public override void OnStartClient ()
		{
			base.OnStartClient ();
			if (seconds > 0) {
				started = true;
				gameOver = false;
			} else {
				started = true;
				gameOver = false;
			}
		}

		IEnumerator Tick (float tick)
		{
			while (seconds > 0) {
				yield return new WaitForSeconds (tick);
				seconds -= tick;
			}
		}

		void Update ()
		{
			if (seconds <= 0
				&& !gameOver) {
				gameOver = true;
				BroadcastMessage (gameOverMessage, SendMessageOptions.DontRequireReceiver);
				if (DidGameOver != null) {
					DidGameOver ();
				}
			}
		}
	}
}
