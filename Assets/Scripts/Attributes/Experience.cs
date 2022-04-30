
using RPG.Saving;
using RPG.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0f;
        
        public void GainExperience(float experience)
        {
            experiencePoints += experience;
        }

        public float GetExperiencePointsForDisplay()
        {
            return experiencePoints;
        }


        public object CaptureState()
        {
            return experiencePoints;
        }

        
        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }   
}