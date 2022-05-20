using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using GameDevTV.Utils;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float aggroCooldownTime = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;
        [SerializeField] float shoutDistance = 5f;
        [Range(0, 1)] [SerializeField] float patrolSpeedFraction = 0.2f;

        private PlayerController player;
        private Fighter fighter;
        private Health health;
        //private Vector3 guardPosition;
        private LazyValue<Vector3> guardPosition;
        private Mover mover;
        private float timeSinceLastSaw = Mathf.Infinity;
        private int currentWaypointIndex = 0;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private float timeSinceAggrevated = Mathf.Infinity;

        private void Awake()
        {
            health = GetComponent<Health>();
            player = FindObjectOfType<PlayerController>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            guardPosition = new LazyValue<Vector3>(GetInitialisedGuardPosition);
        }

        private Vector3 GetInitialisedGuardPosition()
        {
            return transform.position;
        }

        private void Start()
        {
            guardPosition.ForceInit();
            //guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (IsAggrevated() && fighter.CanAttack(player.gameObject))
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

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSaw += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        public void Aggrevate()
        {
            timeSinceAggrevated = 0;
        }

        private void PatrolState()
        {
            Vector3 nextPosition = guardPosition.value;
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
            timeSinceLastSaw = 0;
            fighter.Attack(player.gameObject);

            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits= Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0f);

            foreach (RaycastHit hit in hits)
            {
                AIController AI = hit.collider.GetComponent<AIController>();
                if (AI == null) continue;

                AI.Aggrevate();
            }
        }

        private bool IsAggrevated()
        {            
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);            
            return distanceToPlayer < chaseDistance || timeSinceAggrevated < aggroCooldownTime;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);            
        }
    }
}