using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using System;
using System.Linq;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Mover mover;
        private Fighter fighter;
        // Start is called before the first frame update
        void Start()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
            {
                UpdateCombat();
            }

            if (!UpdateMovement(Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)))
            {
                print("Not a good target");
            }


        }

        private bool UpdateCombat()
        {

            List<RaycastHit> hitList = new List<RaycastHit>(Physics.RaycastAll(GetMouseRay()));
            RaycastHit hit = hitList.FirstOrDefault(h => h.collider.gameObject.GetComponent<CombatTarget>() != null);
            if (hit.collider != null)
            {
                fighter.Attack(hit.transform.GetComponent<CombatTarget>());
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

