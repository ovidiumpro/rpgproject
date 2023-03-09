using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        public float health = 20f;

        public void TakeDamage(float damageValue) {
            health = Mathf.Max(0, health - damageValue);
            print("Current Health of " + this.GetType().ToString() + " is: " + health);
        }
    }

}