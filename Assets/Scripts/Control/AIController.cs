using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;

        private PlayerController player;
        private Fighter fighter;
        private Health health;
        private void Awake()
        {
            health = GetComponent<Health>();
            player = FindObjectOfType<PlayerController>();
            fighter = GetComponent<Fighter>();
        }
        private void Update()
        {
            if (health.IsDead()) return;
           
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