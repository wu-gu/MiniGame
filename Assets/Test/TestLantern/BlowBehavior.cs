using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowBehavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int isFloating = Animator.StringToHash("IsFloating");
        Animator m_animator = GameObject.Find("Leaves").GetComponent<Animator>();
        m_animator.speed = 0.3f;
        m_animator.SetBool(isFloating, true);
    }
}
