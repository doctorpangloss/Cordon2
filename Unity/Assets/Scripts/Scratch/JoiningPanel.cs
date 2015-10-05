using UnityEngine;
using System.Collections;

namespace Scratch
{
	public class JoiningPanel : MonoBehaviour
	{
		public ClientNetworkManager basicNetworkManager;
		public GameObject panel;
		bool previousJoining = false;
		// Use this for initialization
		void Start ()
		{
			previousJoining = basicNetworkManager.joining;
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (previousJoining == basicNetworkManager.joining) {
				return;
			}

			panel.SetActive (basicNetworkManager.joining);

			previousJoining = basicNetworkManager.joining;
		}
	}
}
