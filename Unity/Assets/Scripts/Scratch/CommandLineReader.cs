using UnityEngine;
using System.Collections;
using System;

namespace Scratch
{
	public class CommandLineReader : MonoBehaviour
	{
		public ClientNetworkManager clientNetworkManager;
		public ServerNetworkManager serverNetworkManager;
		public string serverArg = "--server";
		public string scheduleArg = "--schedule";
		public string forceAutojoin = "--autojoin";

		// Use this for initialization
		void Start ()
		{
			var args = System.Environment.GetCommandLineArgs ();
			var isServer = false;
			var useScheduledStart = false;
			var isForcingAutojoin = false;
			var scheduledTime = DateTime.UtcNow;
			for (var i = 0; i < args.Length; i++) {
				if (args [i].StartsWith (forceAutojoin)) {
					isForcingAutojoin = true;
					continue;
				}
				if (args [i].StartsWith (serverArg)) {
					isServer = true;
					continue;
				}

				if (args [i].StartsWith (scheduleArg)) {
					useScheduledStart = true;
					if (args.Length > i) {
						var dateString = args [i + 1];
						i++;
						DateTime.TryParse (dateString, System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.AssumeUniversal, out scheduledTime);
					} 
					continue;
				}
			}

			serverNetworkManager.useScheduledStart = useScheduledStart;
			if (useScheduledStart) {
				serverNetworkManager.startGameUtc = scheduledTime;
			}
			serverNetworkManager.enabled = isServer;

			clientNetworkManager.forceAutojoin = isForcingAutojoin;
			clientNetworkManager.enabled = !isServer;
		}
	}
}
