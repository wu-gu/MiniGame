using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class End : MonoBehaviour
    {
        // Start is called before the first frame update
        private Animator m_animator;


        void Start()
        {
            m_animator = GetComponent<Animator>();

        }

        // Update is called once per frame
        void Update()
        {

        }

        //动画事件
        public void TransitionToStart()
        {
            GameController.Instance.TransitionToNewLevel("Start");
        }
    }
}
