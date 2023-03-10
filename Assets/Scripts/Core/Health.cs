using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        public float healthPoints = 20f;
        private Animator animator;
        bool isDead = false;
        public void Start() {
            animator = GetComponent<Animator>();
        }

        public bool IsDead() {
            return isDead;
        }

        public void TakeDamage(float damageValue) {
            healthPoints = Mathf.Max(0, healthPoints - damageValue);
            print("Current Health of " + this.GetType().ToString() + " is: " + healthPoints);
            if (healthPoints == 0 && !isDead)
            {
                Die();
            }
        }

        private void Die()
        {
            animator.SetTrigger("die");
            isDead = true;
            GetComponent<ActionScheduler>().ClearSchedule();
        }
    }

}