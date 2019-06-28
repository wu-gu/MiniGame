using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class catanimtion : MonoBehaviour, QuestBehavior
    {
        public GameObject destGameobject;
        private Animator m_animator;
        SpriteRenderer CatRenderer;
        pavilionscale mypavi;
        Collider2D cl2d;


        public void OnUpdate()
        {
            this.enabled = true;
            m_animator.enabled = true;
            
        }

        // Start is called before the first frame update
        void Awake()
        {
            m_animator = GetComponent<Animator>();
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
            CatRenderer = GetComponent<SpriteRenderer>();
            mypavi = GameObject.Find("Pavilion").GetComponent<pavilionscale>();
            cl2d = GetComponent<Collider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            AnimatorStateInfo animatorInfo;
            animatorInfo = m_animator.GetCurrentAnimatorStateInfo(0);

            if ((animatorInfo.normalizedTime > 1.0f) )
            {
                m_animator.enabled = false;
                CatRenderer.enabled = false;
                cl2d.enabled = false;
                QuestController.Instance.UnRegisterQuest(gameObject.ToString());
               
                mypavi.settrue();
                Destroy(gameObject);
            }
        }
    }
}