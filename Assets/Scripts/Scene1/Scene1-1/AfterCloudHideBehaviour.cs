using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{


    public class AfterCloudHideBehaviour : StateMachineBehaviour
    {
        //private bool m_isFirst = true;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //if (stateInfo.normalizedTime > 1.0f&&m_isFirst)
            //{
            //    GameObject.Find("GoAction").GetComponent<GoToPavilionQuest>().DoWorking();
            //    m_isFirst = false;         
            //}

            if (stateInfo.normalizedTime > 1.0f)
            {
                GameObject.Find("GoAction").GetComponent<GoToPavilionQuest>().DoWorking();
                Destroy(this);
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}
