using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float health = 5f;

        private bool isDead;

        
        private void Start()
        {
            health = GetComponent<BaseStats>().GetHealth();    
        }

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

        public float GetHealthForDisplay()
        {
            return health; 
        }

        public void Die()
        {
            if (isDead == true) return;
            
            Animator animator = GetComponent<Animator>();
            animator.SetTrigger("die");
            isDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return health;  
        }

        public void RestoreState(object state)
        {
            health = (float)state;

            if (health == 0)
            {
                Die();
            }
        }
    }
}