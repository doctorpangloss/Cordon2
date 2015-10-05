using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

namespace Scratch
{
	public class BasicNetworkManager : NetworkManager
	{
		public float requestListTimeout = 4f;
		public bool forceAutojoin = false;
		public bool hasMatch = false;
		JoinMatchResponse joinMatchResponse;
		public bool joining = false;
		public bool requestingMatch = false;

		void Start ()
		{
			if (!Application.isEditor
				|| forceAutojoin) {
				StartMatchMaker ();
				StartCoroutine (StartRequestMatch ());
			}
		}

		public override void OnMatchList (UnityEngine.Networking.Match.ListMatchResponse matchList)
		{
			requestingMatch = false;
			base.OnMatchList (matchList);
			Debug.Log ("Matches received.");

			if (!matchList.success
				|| matchList.matches == null
				|| matchList.matches.Count == 0) {
				StartCoroutine (StartRequestMatch ());
				return;
			}

			if (!UnityEngine.Networking.NetworkServer.active
				&& !Application.isEditor
				|| forceAutojoin) {
				// Connect to a random match!
				matchMaker.JoinMatch (matchList.matches [UnityEngine.Random.Range(0,matchList.matches.Count - 1)].networkId, "", CustomOnMatchJoined);
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
			this.StartClient (new MatchInfo (joinMatchResponse));
		}

		/// <summary>
		/// This is really raised when the player has joined
		/// </summary>
		/// <param name="conn">Conn.</param>
		public override void OnClientConnect (NetworkConnection conn)
		{
			base.OnClientConnect (conn);
			joining = false;
		}

		public override void OnStartClient (NetworkClient client)
		{
			base.OnStartClient (client);
			joining = true;
		}

		IEnumerator StartRequestMatch ()
		{
			yield return new WaitForSeconds (requestListTimeout);
			Debug.Log ("Listing matches...");
			if (!requestingMatch) {
				requestingMatch = true;
				matchMaker.ListMatches (0, 20, "", OnMatchList);
			}
		}
	}
}