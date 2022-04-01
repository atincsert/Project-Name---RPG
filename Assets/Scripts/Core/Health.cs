using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 5f;

        private bool isDead;

        public bool IsDead()
        {
            return isDead;
        }
        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            //Debug.Log(health);
            if (health == 0)
            {
                Die();
            }
        }

        public void Die()
        {
            if (isDead == true) return;
            
            Animator animator = GetComponent<Animator>();
            animator.SetTrigger("die");
            isDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}