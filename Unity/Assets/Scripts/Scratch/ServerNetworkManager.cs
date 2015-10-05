using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;

namespace Scratch
{
	public class ServerNetworkManager : MonoBehaviour
	{
		[Header("Options")]
		public bool
			useScheduledStart = false;
		public DateTime
			startGameUtc;
		public NetworkManager networkManager;
		// Use this for initialization
		void Start ()
		{
			networkManager.StartMatchMaker ();
			networkManager.matchMaker.CreateMatch (matchName: UnityEngine.Random.value.ToString (),
			                       matchSize: networkManager.matchSize,
			                       matchAdvertise: true,
			                       matchPassword: "",
			                       callback: OnMatchCreate);
		}

		IEnumerator WaitForGame ()
		{
			if (useScheduledStart) {
				while (DateTime.UtcNow < startGameUtc) {
					yield return new WaitForSeconds (1f);
				}
			}

			StartGame ();
		}

		void OnMatchCreate (UnityEngine.Networking.Match.CreateMatchResponse matchInfo)
		{
			Debug.Log ("NetworkManager OnMatchCreate " + matchInfo);
			if (matchInfo.success) {
				UnityEngine.Networking.Utility.SetAccessTokenForNetwork (matchInfo.networkId, new NetworkAccessToken (matchInfo.accessTokenString));
				networkManager.StartHost (new MatchInfo (matchInfo));
			} else {
				if (LogFilter.logError) {
					Debug.LogError ("Create Failed:" + matchInfo);
				}
			}

			StartCoroutine (WaitForGame ());	
		}

		void StartGame ()
		{
			GameController.instance.StartGame ();
		}
	}
}
