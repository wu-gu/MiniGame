using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class LeafQuest : MonoBehaviour, QuestBehavior
    {
        //业务变量
        private bool m_isSuccess = false;
        /// <summary>
        /// 机关音效
        /// </summary>
        public AudioClip m_magpieAudioClip;
        /// <summary>
        /// 缩放业务变量
        /// </summary>
        public float scaleUnit = 0.3f;
        private float m_oldDistance;
        private Vector2 m_originScale;
        private AnimatorStateInfo m_animatorInfo;
        private GameObject m_willow;
        private GameObject m_boat;
        private GameObject m_shield;
        public Vector3 threshold = new Vector3(3.0f,3.0f,1.0f);

        /// <summary>
        /// 注册机关
        /// </summary>
        void Awake()
        {
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
        }

        /// <summary>
        /// 初始化比例系数
        /// </summary>
        void Start()
        {
            //m_originScale = transform.localScale;
            m_originScale = new Vector2(0.5F, 0.5F);
            m_willow = GameObject.Find("Willow");
            m_boat = GameObject.Find("Boat");
            m_shield = GameObject.Find("Shield");
        }

        public void OnUpdate()
        {
            m_animatorInfo = m_willow.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1);
            if ((m_animatorInfo.normalizedTime > 1.0f) && (m_animatorInfo.IsName("LeafFall")))//normalizedTime: 范围0 -- 1, 0是动作开始，1是动作结束
            {
                m_willow.GetComponent<Animator>().enabled = false;
            }
            if (transform.localScale.x > threshold.x && transform.localScale.y > threshold.y)
            {
                m_isSuccess = true;
                m_willow.GetComponent<Animator>().enabled = true;
                gameObject.GetComponent<Collider2D>().enabled = false;
                m_willow.GetComponent<Animator>().SetBool("LeafQuestFired", true);
                m_willow.GetComponent<Animator>().Play("LeafHide");
                m_boat.GetComponent<Animator>().enabled = true;
                GameObject.Destroy(m_shield);
                //注销机关
                QuestController.Instance.UnRegisterQuest(gameObject.ToString());
            }

            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                //第二个接触点落下，计算初始两触点距离
                if (touch2.phase == TouchPhase.Began)
                {
                    m_oldDistance = Vector2.Distance(touch1.position, touch2.position);
                    return;
                }

                if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
                {
                    float newDistance = Vector2.Distance(touch1.position, touch2.position);
                    //计算缩放比例，并更新物体缩放系数
                    float newScale = (newDistance - m_oldDistance) * scaleUnit;
                    transform.localScale += new Vector3(newScale, newScale, 1);
                    m_oldDistance = newDistance;
                }

                //机关触发判断，如果不触发，恢复原来大小
                if (touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended)
                {
                    //float originScale = m_oldDistance / newDistance * scaleUnit;
                    //transform.localScale.Scale(new Vector3(originScale, originScale, 1));
                    transform.localScale = m_originScale;
                }
            }
            /// <summary>
            /// 滚轮，放大
            /// </summary>
            else if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                scaleUnit += Input.GetAxis("Mouse ScrollWheel");
                transform.localScale += new Vector3(1 * scaleUnit, 1 * scaleUnit, 1 * scaleUnit);//改变物体大小
            }
            else if(Input.GetAxis("Mouse ScrollWheel") == 0)
            {
                transform.localScale = m_originScale;
            }
        }

        void Update()
        {
            OnUpdate();
        }
    }
}

