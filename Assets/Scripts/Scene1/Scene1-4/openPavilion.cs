using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class OpenPavilion : MonoBehaviour, QuestBehavior
    {
        private Collider2D m_collider2D;
        private Animator m_animator;
        private GameObject destGameobject;
        private Vector3 boypos;
        private bool canopen = true;
        private bool isFirst = true;
        public float distance;
        public AudioClip openDoorAudio;

        //转场动画
        //private GameObject m_stage1_5;
        private GameObject m_camera;


        public void OnUpdate()
        {
            if (canopen && isFirst)
            {
                isFirst = false;
                AudioController.Instance.PushClip(openDoorAudio);
                m_animator.enabled = true;
                StartCoroutine(WaitForOpenDoorMusic());
            }
        }

        void Start()
        {
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
            m_animator = GameObject.Find("Pavilion").GetComponent<Animator>();
            m_collider2D = GetComponent<Collider2D>();
            destGameobject = GameObject.Find("Boy");
            //m_stage1_5 = GameObject.Find("--- Level1-5 ---");
            m_camera = GameObject.FindGameObjectWithTag("MainCamera");
            this.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            AnimatorStateInfo animatorInfo;
            animatorInfo = m_animator.GetCurrentAnimatorStateInfo(0);
            boypos = destGameobject.transform.position;
            //Debug.Log("distance" + Mathf.Abs(transform.position.x - boypos.x));
            if (Mathf.Abs(transform.position.x - boypos.x) < distance)
            {
                canopen = true;
            }
            else
            {
                canopen = false;
            }
            //if (animatorInfo.normalizedTime > 1.0f && animatorInfo.IsName("Open"))
            //{


            //}
        }

        IEnumerator WaitForOpenDoorMusic()
        {
            yield return new WaitForSeconds(2);

            QuestController.Instance.UnRegisterQuest(gameObject.ToString());
            m_animator.enabled = false;
            m_collider2D.enabled = false;
            m_camera.GetComponent<Animator>().enabled = true;
            m_camera.GetComponent<Animator>().SetTrigger("Stage4To5Trigger");
            Destroy(this);

        }

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if (collision.gameObject.Equals(destGameobject))
        //    {
        //        canopen = true;
        //    }
        //}

        //private void OnTriggerExit2D(Collider2D collision)
        //{
        //    if (collision.gameObject.Equals(destGameobject))
        //    {
        //        canopen = false;
        //    }
        //}
    }
}

