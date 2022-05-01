using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        float health = -1f;

        private bool isDead;

        
        private void Start()
        {
            if (health < 0)
            {
                health = GetComponent<BaseStats>().GetStat(Stat.Health);    
            }
        }

        public bool IsDead()
        {
            return isDead;
        }
        public void TakeDamage(GameObject instigator, float damage)
        {
            health = Mathf.Max(health - damage, 0);
            //Debug.Log(health);
            if (health <= 0)
            {
                Die();
                AwardExperience(instigator);
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

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
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