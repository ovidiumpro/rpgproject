using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using System;
using System.Linq;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Mover mover;
        private Fighter fighter;
        private Health health;
        // Start is called before the first frame update
        void Start()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            if (health.IsDead())
            {
                return;
            }
            if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
            {
                UpdateCombat();
            }

            if (!UpdateMovement(Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)))
            {
                
            }


        }

        private bool UpdateCombat()
        {

            // List<RaycastHit> hitList = new List<RaycastHit>(Physics.RaycastAll(GetMouseRay()));
            // RaycastHit hit = hitList.FirstOrDefault(h => h.collider.gameObject.GetComponent<CombatTarget>() != null);
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null || !fighter.canAttack(target.gameObject)) continue;

                fighter.Attack(target.gameObject);
                return true;

            }
            return false;
        }

        private bool UpdateMovement(bool mouseClicked)
        {

            return mover.MoveToRayIfHit(GetMouseRay(), mouseClicked);

        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

