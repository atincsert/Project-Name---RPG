using RPG.Control;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private GameObject player;
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");            
        }

        private void OnEnable()
        {
            GetComponent<PlayableDirector>().played += DisableController;
            GetComponent<PlayableDirector>().stopped += EnableController;
        }

        private void OnDisable()
        {
            GetComponent<PlayableDirector>().played -= DisableController;
            GetComponent<PlayableDirector>().stopped -= EnableController;
        }

        private void DisableController(PlayableDirector pd)
        {            
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
            Debug.Log("Disable Controller");
        }

        private void EnableController(PlayableDirector pd)
        {
            player.GetComponent<PlayerController>().enabled = true;
            Debug.Log("Enable Controller");
        }
    }
}

