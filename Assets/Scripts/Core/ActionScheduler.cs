using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        private IAction currentAction;
        private String currentTypeName;
        public void StartAction(IAction action) {
            
            if (currentAction == null) {
                StartActionAndPrint(action);
                return;
            }
            if (currentAction.GetType() != action.GetType()) {
                print("Stopping action: " + currentAction.GetType().ToString());
                currentAction.Cancel();
                StartActionAndPrint(action);
            }
           
        }

        private void StartActionAndPrint(IAction action) {
            print("starting action: "+action.GetType().ToString());
            currentAction = action;
        }
    }
    
}

