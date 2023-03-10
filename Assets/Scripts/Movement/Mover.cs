using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        public float maxAngleDifference = 120f; // the maximum angle difference between current direction and desired direction for manual rotation
        public float manualRotationSpeed = 360f; // the rotation speed when manually rotating the character
        public float acceleration = 10f; // the acceleration when changing velocity
        public float deceleration = 10f; // the deceleration when stopping

        private NavMeshAgent agent;
        private Vector3 targetPosition;
        private Vector3 desiredVelocity;
        private ActionScheduler scheduler;
        private Animator animator;
        private Health health;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            scheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            agent.enabled = !health.IsDead();

            UpdateAnimator();
        }

        public bool MoveToRayIfHit(Ray ray, bool move)
        {

            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);
            if (!hasHit) return false;

            targetPosition = hit.point;
            if (!move)
            {
                return HasPath(targetPosition);
            }
            return MoveTo(targetPosition, true);
        }

        public bool MoveTo(Vector3 destination, bool schedule)
        {
            if (HasPath(destination))
            {
                // A path exists, set the destination for the NavMeshAgent

                agent.isStopped = false;
                // Check if a path exists between the character's current position and the target position

                if (schedule) {
                    scheduler.StartAction(this);
                }
                //calculate angle difference between new direction and current
                Vector3 direction = destination - transform.position;
                float angleDifference = Vector3.Angle(transform.forward, direction);
                //decide to manually rotate char or not based on angle diff. This is to avoid having deceleration because of rotation
                if (angleDifference > maxAngleDifference)
                {
                    // Rotate the character manually
                    transform.LookAt(destination);
                    agent.updateRotation = false;
                }
                else
                {
                    // Use the NavMeshAgent's rotation
                    agent.updateRotation = true;
                }
                desiredVelocity = direction.normalized * agent.speed;
                agent.SetDestination(destination);
                return true;

            }
            else
            {
                // No path exists, do not set the destination
                Debug.LogWarning("No path exists to the target position!");
                return false;
            }
        }
        private bool HasPath(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            return agent.CalculatePath(destination, path);
        }

        public void Cancel()
        {
            agent.isStopped = true;
            animator.SetFloat("ForwardSpeed", 0f);
            desiredVelocity = Vector3.zero;
        }
        private void UpdateAnimator()
        {
            // // Calculate the speed from the desired velocity
            // Vector3 currentVelocity = agent.velocity;
            // Vector3 newVelocity = Vector3.MoveTowards(currentVelocity, desiredVelocity, acceleration * Time.deltaTime);
            // float speed = newVelocity.z;
            // Debug.Log(currentVelocity.magnitude);

            // Update the animator with the speed
            Vector3 localVel = transform.InverseTransformDirection(agent.velocity);
            float speed = localVel.z;
            animator.SetFloat("ForwardSpeed", speed);

        }


        private void FixedUpdate()
        {
            // Move the character using the desired velocity
            Vector3 currentVelocity = agent.velocity;
            Vector3 newVelocity = Vector3.MoveTowards(currentVelocity, desiredVelocity, acceleration * Time.deltaTime);
            if (newVelocity.magnitude < 0.01f)
            {
                // Stop the character if the desired velocity is small enough
                newVelocity = Vector3.zero;
            }
            else if (currentVelocity.magnitude < newVelocity.magnitude)
            {
                // If the character is accelerating, use acceleration
                newVelocity = Vector3.MoveTowards(currentVelocity, newVelocity, acceleration * Time.deltaTime);
            }
            else
            {
                // If the character is decelerating, use deceleration
                newVelocity = Vector3.MoveTowards(currentVelocity, newVelocity, deceleration * Time.deltaTime);
            }
            agent.velocity = newVelocity;
        }
    }
}
