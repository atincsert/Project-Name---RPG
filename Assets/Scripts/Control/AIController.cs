using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;
        [Range(0, 1)] [SerializeField] float patrolSpeedFraction = 0.2f;

        private PlayerController player;
        private Fighter fighter;
        private Health health;
        private Vector3 guardPosition;
        private Mover mover;
        private float timeSinceLastSaw = Mathf.Infinity;
        private int currentWaypointIndex = 0;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private void Awake()
        {
            health = GetComponent<Health>();
            player = FindObjectOfType<PlayerController>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();

            guardPosition = transform.position;
        }
        private void Update()
        {            
            if (health.IsDead()) return;

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player.gameObject))
            {
                timeSinceLastSaw = 0f;
                AttackState();
            }
            else if (timeSinceLastSaw < suspicionTime)
            {
                SuspicionState();
            }
            else
            {
                PatrolState();
            }

            timeSinceLastSaw += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolState()
        {
            Vector3 nextPosition = guardPosition;
            if (patrolPath != null)
            {
                timeSinceArrivedAtWaypoint += Time.deltaTime;
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0f;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void SuspicionState()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackState()
        {
            fighter.Attack(player.gameObject);
        }

        private bool InAttackRangeOfPlayer()
        {            
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);            
        }
    }
}