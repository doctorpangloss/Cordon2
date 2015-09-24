using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Scratch
{
	public class IntroPanel : MonoBehaviour
	{
		public BasicNetworkManager basicNetworkManager;
		public Text buttonLabel;
		public string hasMatchText;
		public string noMatchText;
		bool previousHasMatchValue;
		// Use this for initialization
		void Start ()
		{
			if (basicNetworkManager == null) {
				enabled = false;
				return;
			}
			previousHasMatchValue = basicNetworkManager.hasMatch;
			UpdateText ();
		}

		public void OnClickStartButton ()
		{
			if (basicNetworkManager.hasMatch) {
				basicNetworkManager.StartGame ();
				gameObject.SetActive (false);
			}
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (basicNetworkManager.hasMatch == previousHasMatchValue) {
				return;
			}

			UpdateText ();
			previousHasMatchValue = basicNetworkManager.hasMatch;
		}

		void UpdateText ()
		{
			if (basicNetworkManager.hasMatch) {
				buttonLabel.text = hasMatchText;
			} else {
				buttonLabel.text = noMatchText;
			}
		}
	}

}