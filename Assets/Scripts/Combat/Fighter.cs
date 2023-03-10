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
        Health targetHealth;
        private Mover mover;
        private ActionScheduler scheduler;
        private Animator animator;
        private float timeSinceLastAttack = Mathf.Infinity;


        private void Start()
        {
            mover = GetComponent<Mover>();
            scheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }
        public void Update()
        {

            timeSinceLastAttack = Mathf.Min(timeBetweenAttacks,timeSinceLastAttack+Time.deltaTime);
            if (target == null) return;
            if (targetHealth.IsDead()) return;
            if (IsNotInRange())
            {
                mover.MoveTo(target.position, false);
            }
            else
            {
                mover.Cancel();
                transform.LookAt(target.position);
                AttackBehaviour();
            }


        }
        public bool canAttack(GameObject target)
        {
            return target != null && !target.GetComponent<Health>().IsDead();
        }

        private void AttackBehaviour()
        {
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                //this will trigger the Hit() event which belongs to the animation in the state triggered
                animator.ResetTrigger("stopAttack");
                animator.SetTrigger("attack");
                timeSinceLastAttack = 0;
                return;
            }
        }
        //Animation event 
        void Hit()
        {
            if (!target) { return; }
            target.GetComponent<Health>().TakeDamage(damage);
        }

        private bool IsNotInRange()
        {
            return Vector3.Distance(transform.position, target.position) > weaponRange;
        }
        public void Attack(GameObject combatTarget)
        {
            scheduler.StartAction(this);
            target = combatTarget.transform;
            targetHealth = target.GetComponent<Health>();
        }
        public void Cancel()
        {
            target = null;
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
        }

    }
}

