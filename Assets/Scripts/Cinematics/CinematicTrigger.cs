using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Combat
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool isPlayed;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (isPlayed == true) return;

                GetComponent<PlayableDirector>().Play();

                isPlayed = true;
            }
        }
    }

}