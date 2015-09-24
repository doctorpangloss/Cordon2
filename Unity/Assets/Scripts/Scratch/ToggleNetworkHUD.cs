using UnityEngine;
using System.Collections;

namespace Scratch
{
	public class ToggleNetworkHUD : MonoBehaviour
	{
		public Behaviour networkManagerHUD;
		public UnityEngine.Networking.NetworkLobbyManager lobbyManager;
		[Header("HUD Starts Enabled")]
		public bool
			onMobile = false;
		public bool others = true;

		void Start ()
		{

			if (Application.isMobilePlatform) {
				if (networkManagerHUD != null) {
					networkManagerHUD.enabled = onMobile;
				}
				if (lobbyManager != null) {
					lobbyManager.showLobbyGUI = onMobile;
				}
			} else {
				if (networkManagerHUD != null) {
					networkManagerHUD.enabled = others;
				}
				if (lobbyManager != null) {
					lobbyManager.showLobbyGUI = others;
				}
			}
			
		}

		public void Toggle ()
		{
			if (networkManagerHUD != null) {
				networkManagerHUD.enabled = !networkManagerHUD.enabled;
			}
			if (lobbyManager != null) {
				lobbyManager.showLobbyGUI = ! lobbyManager.showLobbyGUI;
			}
		}
	}
}
