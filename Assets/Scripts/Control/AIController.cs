using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;

        private PlayerController player;
        private Fighter fighter;
        private void Awake()
        {
            player = FindObjectOfType<PlayerController>();
            fighter = GetComponent<Fighter>();
        }
        private void Update()
        {
           
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player.gameObject))
            {
                fighter.Attack(player.gameObject);
            }
            else
            {
                fighter.Cancel();
            }

        }

        private bool InAttackRangeOfPlayer()
        {            
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }
    }
}