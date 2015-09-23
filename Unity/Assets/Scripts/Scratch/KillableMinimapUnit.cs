using UnityEngine;
using System.Collections;

namespace Scratch
{
	public class KillableMinimapUnit : MinimapUnit
	{
		[Header("Killable")]
		public MinimapIcon
			killIcon;

		void OnKill ()
		{
			uiIconOverride = killIcon;
			StartCoroutine (StartOnKill ());
		}

		IEnumerator StartOnKill ()
		{
			yield return new WaitForEndOfFrame ();
			RemoveFromMinimap ();
			uiIconOverride = killIcon;
			yield return new WaitForEndOfFrame ();
			AddToMinimap ();
		}
	}
}
