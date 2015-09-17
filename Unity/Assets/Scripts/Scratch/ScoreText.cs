using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Scratch
{
	public class ScoreText : MonoBehaviour
	{
		Text text;

		void Start ()
		{
			text = GetComponent<Text> ();
		}
		// Update is called once per frame
		void Update ()
		{
			text.text = string.Format ("Score: {0}", Scoring.instance.Score);
		}
	}
}