using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class CatWalkOut : MonoBehaviour, QuestBehavior
    {
        private Animator m_animator;
        private bool RegQuest = false;
        Collider2D cat_cl2d;

        private bool m_canCatWalk;
        private Vector2 m_catDestPos;
        public float speed;


        public void OnUpdate()
        {

            m_animator.SetBool("walkout", true);
            cat_cl2d.enabled = false;
            Debug.Log("walkout");
            QuestController.Instance.UnRegisterQuest(gameObject.ToString());
            this.enabled = false;

        }

        // Start is called before the first frame update
        void Awake()
        {
            m_animator = GetComponent<Animator>();
            cat_cl2d = GetComponent<Collider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            AnimatorStateInfo animatorInfo;
            animatorInfo = m_animator.GetCurrentAnimatorStateInfo(0);
            if (animatorInfo.normalizedTime > 1.0f)
            {
                GameController.Instance.enabled = true;
                if (RegQuest == false)
                {
                    cat_cl2d.enabled = true;
                    QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
                    RegQuest = true;

                }
            }

            if (m_canCatWalk)
            {
                transform.position = Vector2.MoveTowards(transform.position, m_catDestPos, speed * Time.deltaTime);
            }
        }

        public void SetCatDestPos()
        {
            m_canCatWalk = true;
            m_catDestPos = GameObject.Find("Girl").transform.position;
        }

    }
}