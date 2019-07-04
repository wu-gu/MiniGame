using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class CatAnimtion : MonoBehaviour, QuestBehavior
    {
        private Animator m_animator;
        private PavilionScale m_pavilionScale;
        private Collider2D m_collider2D;


        public void OnUpdate()
        {
            m_animator.enabled = true;
            this.enabled = true;
        }

        void Awake()
        {
            m_animator = GetComponent<Animator>();
            m_collider2D = GetComponent<Collider2D>();
        }

        void Start()
        {
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
            m_pavilionScale = GameObject.Find("Pavilion").GetComponent<PavilionScale>();
        }

        // Update is called once per frame
        void Update()
        {
            AnimatorStateInfo animatorInfo;
            animatorInfo = m_animator.GetCurrentAnimatorStateInfo(0);

            if ((animatorInfo.normalizedTime > 1.0f) )
            {
                m_animator.enabled = false;
                m_collider2D.enabled = false;
                QuestController.Instance.UnRegisterQuest(gameObject.ToString());         
                m_pavilionScale.setTrue();
                Destroy(gameObject);
            }
        }
    }
}