using UnityEngine;
using System.Collections;

namespace Scratch
{
	public class GameOverPanel : MonoBehaviour
	{
		public GameObject gameOverPanel;
		public GameController gameController;
		// Use this for initialization
		void Start ()
		{
			gameController.DidGameOver += OnGameOver;
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