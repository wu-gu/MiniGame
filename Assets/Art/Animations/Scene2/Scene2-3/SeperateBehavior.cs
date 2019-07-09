using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeperateBehavior : StateMachineBehaviour
{
    private bool m_first = true;

    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > 1.0f && m_first)
        {
            m_first = false;
            Animator m_animator = GameObject.Find("Stage2-3").GetComponent<Animator>();
            int IsFadeTime = Animator.StringToHash("IsFadeTime");
            m_animator.SetBool(IsFadeTime, true);
            GameObject m_character = GameObject.Find("Character");
            GameObject m_girl = GameObject.FindGameObjectWithTag("Girl");
            m_girl.transform.SetParent(m_character.transform.parent);
            Destroy(m_character);
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
