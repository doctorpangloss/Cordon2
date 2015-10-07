using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

namespace Scratch
{
	public class ClientNetworkManager : MonoBehaviour
	{
		public NetworkManager networkManager;
		public float requestListTimeout = 4f;
		public bool forceAutojoin = false;

		public bool autojoin {
			get {
				if (forceAutojoin) {
					return true;
				}

				foreach (var autojoinPlatform in autojoinPlatforms) {
					if (autojoinPlatform == Application.platform) {
						return true;
					}
				}

				return false;
			}
		}

		public RuntimePlatform[] autojoinPlatforms;
		public bool hasMatch = false;
		JoinMatchResponse joinMatchResponse;
		public bool joining = false;
		public bool requestingMatch = false;

		void Start ()
		{
			if (autojoin) {
				networkManager.StartMatchMaker ();
				StartCoroutine (StartRequestMatch ());
			}
		}

		void OnMatchList (UnityEngine.Networking.Match.ListMatchResponse matchList)
		{
			requestingMatch = false;
			networkManager.OnMatchList (matchList);
			Debug.Log ("Matches received.");

			if (!matchList.success
				|| matchList.matches == null
				|| matchList.matches.Count == 0) {
				StartCoroutine (StartRequestMatch ());
				return;
			}

			if (!UnityEngine.Networking.NetworkServer.active
				&& !Application.isEditor
				|| autojoin) {
				// Connect to a random match!
				networkManager.matchMaker.JoinMatch (matchList.matches [UnityEngine.Random.Range (0, matchList.matches.Count - 1)].networkId, "", CustomOnMatchJoined);
			}
		}

		public void CustomOnMatchJoined (JoinMatchResponse matchInfo)
		{
			if (LogFilter.logDebug) {
				Debug.Log ("NetworkManager OnMatchJoined ");
			}
			if (matchInfo.success) {
				Utility.SetAccessTokenForNetwork (matchInfo.networkId, new NetworkAccessToken (matchInfo.accessTokenString));
				hasMatch = true;
				joinMatchResponse = matchInfo;
			} else {
				if (LogFilter.logError) {
					Debug.LogError ("Join Failed:" + matchInfo);
					StartCoroutine (StartRequestMatch ());
				}
			}
		}

		public void StartGame ()
		{
			networkManager.StartClient (new MatchInfo (joinMatchResponse));
		}

		/// <summary>
		/// This is really raised when the player has joined
		/// </summary>
		/// <param name="conn">Conn.</param>
		void OnClientConnect (NetworkConnection conn)
		{
			networkManager.OnClientConnect (conn);
			joining = false;
		}

		void OnStartClient (NetworkClient client)
		{
			networkManager.OnStartClient (client);
			joining = true;
		}

		IEnumerator StartRequestMatch ()
		{
			yield return new WaitForSeconds (requestListTimeout);
			Debug.Log ("Listing matches...");
			if (!requestingMatch) {
				requestingMatch = true;
				if (networkManager.matchMaker == null) {
					requestingMatch = false;
					yield break;
				}
				networkManager.matchMaker.ListMatches (0, 20, "", OnMatchList);
			}
		}
	}
}