using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class PlateQuest : MonoBehaviour, QuestBehavior
    {
        /*业务变量*/

        public GameObject destGameobject;//关联机关，此处为树枝
        private bool m_isSuccess = false;//此处机关是否成功破解
        private Animator m_animator;

        /*平移基础操作所需变量*/
        private Vector2 m_originPos;//原来位置
        private Vector2 m_offset;//触摸位置与物体中心点的偏移


        void Awake()
        {
            m_originPos = transform.position;
            m_animator = Camera.main.GetComponent<Animator>();
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
        }


        public void OnUpdate()
        {
            this.enabled = true;
        }

        void Update()
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.touches[0];
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (touch.phase == TouchPhase.Began)
                {
                    m_offset = new Vector2(transform.position.x, transform.position.y) - touchPos;
                }
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 currPos = touchPos + m_offset;
                    transform.position = new Vector3(currPos.x, currPos.y, transform.position.z);
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    if (m_isSuccess)
                    {
                        Debug.Log("盘子机关成功");
                        m_animator.enabled = true;
                        m_animator.Play("Scene1ToScene2");
                        QuestController.Instance.UnRegisterQuest(gameObject.ToString());
                        gameObject.GetComponent<Collider2D>().enabled = false;
                        this.enabled = false;
                    }
                    else
                    {
                        Debug.Log("盘子机关未成功");
                        //瞬时回到原来位置
                        m_animator.enabled = false;
                        transform.position = new Vector3(m_originPos.x, m_originPos.y, transform.position.z);
                        this.enabled = false;
                        //过渡回到原来位置
                    }
                }
            }
        }

        /// <summary>
        /// PC端使用
        /// </summary>
        private void OnMouseDown()
        {
            if (Input.GetMouseButton(0))
            {
                m_animator.enabled = false;
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                m_offset = new Vector2(transform.position.x, transform.position.y) - touchPos;
            }
        }

        private void OnMouseDrag()
        {
            //Debug.Log("拖拽物体");
            if (Input.GetMouseButton(0))
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 currPos = touchPos + m_offset;
                transform.position = new Vector3(currPos.x, currPos.y, transform.position.z);
            }
        }

        /// <summary>
        /// 在此处做机关触发判断
        /// </summary>
        private void OnMouseUp()
        {
            Debug.Log("进来啦" + m_isSuccess);
            //先做机关破解成功判断
            if (m_isSuccess)
            {
                Debug.Log("机关成功");
                gameObject.GetComponent<Collider2D>().enabled = false;
                m_animator.enabled = true;
                m_animator.Play("Scene1ToScene2");
                QuestController.Instance.UnRegisterQuest(gameObject.ToString());
                this.enabled = false;
            }
            else
            {
                //瞬时回到原来位置
                transform.position = new Vector3(m_originPos.x, m_originPos.y, transform.position.z);
                m_animator.enabled = false;
                this.enabled = false;
                //过渡回到原来位置
            }
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            //可以采取距离判断或者物体碰撞的方式判断机关是否已经破解。此处采用物体碰撞
            if (collision.gameObject.Equals(destGameobject))
            {
                Debug.Log("盘子碰到位置了");
                //可以让目的物体发白光
                m_isSuccess = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.Equals(destGameobject))
            {
                Debug.Log("盘子离开位置了");
                m_isSuccess = false;
            }
        }
    }
}

