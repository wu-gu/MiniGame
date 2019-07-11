using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame {

    public class BlowBehavior : StateMachineBehaviour
    {
        [Tooltip("风吹落叶声音")]
        public AudioClip windBlowLeavesClip;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            int isFloating = Animator.StringToHash("IsFloating");
            Animator m_animator = GameObject.Find("Leaves").GetComponent<Animator>();
            m_animator.SetBool(isFloating, true);
            AudioController.Instance.PushClip(windBlowLeavesClip);
        }
    }
}
