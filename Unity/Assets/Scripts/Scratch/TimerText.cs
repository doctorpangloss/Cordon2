using UnityEngine;
using System.Collections;

namespace Scratch
{
	public class TimerText : MonoBehaviour
	{
		UnityEngine.UI.Text text;
		// Use this for initialization
		void Start ()
		{
			text = GetComponent<UnityEngine.UI.Text> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
			text.text = GameController.instance.seconds.ToString ();
		}
	}
}