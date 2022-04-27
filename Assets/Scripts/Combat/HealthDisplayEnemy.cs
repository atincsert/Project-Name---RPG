
using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class HealthDisplayEnemy : MonoBehaviour
    {
        Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            if (fighter.GetTargetHealthForDisplay() == null)
            {
                GetComponent<Text>().text = "N/A";
            }
            Health health = fighter.GetTargetHealthForDisplay();
            GetComponent<Text>().text = health.GetHealthForDisplay().ToString();
        }
    }

}
