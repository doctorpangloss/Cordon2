using UnityEngine;
using System.Collections;

namespace Scratch
{
    public class PlayerMovementSoundEffect : MonoBehaviour
    {
        public MoveController moveController;
        public AudioSource footStep;
        private bool playing = false;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (moveController.moving)
            {
                if (!footStep.isPlaying)
                {
                    footStep.Play();
                }
            } else
            {
                if (footStep.isPlaying)
                {
                    footStep.Stop();
                }
            }
        }
    }
}
