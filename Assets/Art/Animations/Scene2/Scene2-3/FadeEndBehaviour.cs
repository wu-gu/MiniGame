using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class FadeEndBehaviour : StateMachineBehaviour
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.normalizedTime > 1.0f)
            {
                string nextStageName = "Stage2-4";

                Debug.Log("Load 2-4");
                if (nextStageName != null)
                {
                    GameController.Instance.LoadNextStageGameObjects(nextStageName);
                    //调整摄像机边界限定
                    //Camera.main.gameObject.GetComponent<CameraController>().UpdateBackgounrdLeft(GameObject.Find(nextStageName+"-L"));
                    //加载时调整右边，卸载时调整左边
                    Camera.main.gameObject.GetComponent<CameraController>().UpdateBackgounrdRight(GameObject.Find(nextStageName + "-R"));
                    Destroy(this);
                }
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

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
