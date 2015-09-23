using UnityEngine;
using System.Collections;

namespace Scratch
{
	public class GameOverPanel : MonoBehaviour
	{
		public GameObject gameOverPanel;

		// Use this for initialization
		void Start ()
		{
			GameController.instance.OnGameOver += OnGameOver;
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void OnGameOver ()
		{
			gameOverPanel.SetActive (true);
		}
	}
}