using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField]
        private float weaponRange = 2f;
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
                
                if (IsNotInRange())
                {
                    mover.MoveTo(target.position);
                }
                else
                {
                    mover.Stop();
                }

            }
        }
        private bool IsNotInRange() {
            return Vector3.Distance(transform.position, target.position) >weaponRange;
        }
        public void Attack(CombatTarget combatTarget)
        {
            print("Attacking a target");
            target = combatTarget.transform;
        }
        public void Cancel() {
            target = null;
        }
    }
}

