using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70f;
        [SerializeField] UnityEvent<float> takeDamage;
        [SerializeField] UnityEvent onDie;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        private LazyValue<float> health;         
        private bool isDead;


        private void Awake()
        {
            health = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start()
        {
            health.ForceInit();
            //if (health < 0)
            //{
            //    health = GetComponent<BaseStats>().GetStat(Stat.Health);    
            //}
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public bool IsDead()
        {
            return isDead;
        }
        public void TakeDamage(GameObject instigator, float damage)
        {
            //Debug.Log($"{gameObject.name} took {damage} damage");

            health.value = Mathf.Max(health.value - damage, 0);
            takeDamage.Invoke(damage);

            if (health.value <= 0)
            {
                onDie.Invoke();
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetHealthForDisplay()
        {
            return health.value; 
        }

        public float GetFraction()
        {
            return health.value / GetComponent<BaseStats>().GetStat(Stat.Health);
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

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            health.value = Mathf.Max(health.value, regenHealthPoints);
        }

        public object CaptureState()
        {
            return health.value;  
        }

        public void RestoreState(object state)
        {
            health.value = (float)state;

            if (health.value == 0)
            {
                Die();
            }
        }
    }
}