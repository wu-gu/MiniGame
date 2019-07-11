using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{

    public class TurnBehavior : StateMachineBehaviour
    {
        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            GameController.Instance.UpdateStageProgress(2);
            Animator leaves = GameObject.Find("Leaves").GetComponent<Animator>();
            int shouldTurn = Animator.StringToHash("ShouldTurn");
            leaves.SetBool(shouldTurn, true);
            leaves.speed = 0.4f;
        }
    }
}
