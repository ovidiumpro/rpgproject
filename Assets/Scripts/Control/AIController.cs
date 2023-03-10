using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.Core;
namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField]
        private float chaseDistance = 10f;
        private GameObject player;
        private Fighter fighter;
        private Mover mover;
        private ActionScheduler scheduler;
        private Health health;
        private Vector3 originPosition;

        void Start()
        {
            player = GameObject.Find("Player");
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            scheduler = GetComponent<ActionScheduler>();
            health = GetComponent<Health>();
            originPosition = transform.position;
        }
        // Update is called once per frame
        void Update()
        {
            if (health.IsDead())
            {
                return;
            }
            if (IsInChaseRange() && fighter.canAttack(player))
            {
                    fighter.Attack(player);
            }
            else
            {
                mover.MoveTo(originPosition, true);
            }
        }

        private bool IsInChaseRange()
        {
            return Vector3.Distance(transform.position, player.transform.position) <= chaseDistance;
        }

        void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }


}

