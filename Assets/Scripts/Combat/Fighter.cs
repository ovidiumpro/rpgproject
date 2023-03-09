using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField]
        private float weaponRange = 2f;
        [SerializeField]
        private float timeBetweenAttacks = 1f;

        [SerializeField]
        private float damage = 5f;
        Transform target;
        private Mover mover;
        private ActionScheduler scheduler;
        private Animator animator;
        private float timeSinceLastAttack = 0;
        private bool isAttacking = false;


        private void Start()
        {
            mover = GetComponent<Mover>();
            scheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }
        public void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target != null)
            {

                if (IsNotInRange())
                {
                    mover.MoveTo(target.position);
                }
                else
                {
                    mover.Cancel();
                    AttackBehaviour();
                }

            }
        }

        private void AttackBehaviour()
        {
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                //this will trigger the Hit() event which belongs to the animation in the state triggered
                animator.SetTrigger("attack");
                timeSinceLastAttack = 0;
                return;
            }
        }
        //Animation event 
        void Hit()
        {
            target.GetComponent<Health>().TakeDamage(damage);
        }

        private bool IsNotInRange()
        {
            return Vector3.Distance(transform.position, target.position) > weaponRange;
        }
        public void Attack(CombatTarget combatTarget)
        {
            print("Attacking a target");
            scheduler.StartAction(this);
            target = combatTarget.transform;
        }
        public void Cancel()
        {
            target = null;
            if (animator.GetBool("attack"))
            {
                print("Target is currently attacking, so cancelling attack");
                animator.ResetTrigger("attack");
            }
        }

    }
}

