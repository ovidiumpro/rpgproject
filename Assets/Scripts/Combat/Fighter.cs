using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField]
        private float range = 2f;
        Transform target;
        private Mover mover;

        private void Start()
        {
            mover = GetComponent<Mover>();
        }
        public void Update()
        {
            if (target != null)
            {
                float distance = Vector3.Distance(transform.position, target.position);
                if (distance > range)
                {
                    mover.MoveTo(target.position);
                }
                else
                {
                    mover.Stop();
                }

            }
        }
        public void Attack(CombatTarget combatTarget)
        {
            print("Attacking a target");
            target = combatTarget.transform;
        }
    }
}

