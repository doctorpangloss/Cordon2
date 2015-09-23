using UnityEngine;
using System.Collections;

namespace Scratch
{
	public class ToggleNetworkHUD : MonoBehaviour
	{
		public Behaviour networkManagerHUD;
		[Header("HUD Starts Enabled")]
		public bool onMobile = false;
		public bool others = true;

		void Start ()
		{
			if (Application.isMobilePlatform) {
				networkManagerHUD.enabled = onMobile;
			} else {
				networkManagerHUD.enabled = others;
			}
		}

		public void Toggle ()
		{
			networkManagerHUD.enabled = !networkManagerHUD.enabled;
		}
	}
}
