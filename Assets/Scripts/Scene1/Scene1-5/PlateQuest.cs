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
        private GameObject m_camera;
        private Vector3 m_cameraOriPos;
        private GameObject m_moon;

        /*平移基础操作所需变量*/
        private Vector2 m_originPos;//原来位置
        private Vector2 m_offset;//触摸位置与物体中心点的偏移

        //相机跟随
        private Vector3 m_offsetBetweenCamAndPlate;
        private Vector3 m_nowPosition;
        private Vector3 m_oldPosition;
        private float minY = 0.0f;
        private float maxY = 17.0f;
        public float m_speed = 0.05f;

        private bool m_canFollow = false;

        void Start()
        {
            m_originPos = transform.position;
            m_animator = Camera.main.GetComponent<Animator>();
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
            m_camera = GameObject.FindGameObjectWithTag("MainCamera");
            m_moon = GameObject.Find("Moon");
            m_cameraOriPos = m_camera.transform.position;
            m_offsetBetweenCamAndPlate = transform.position - m_camera.transform.position;
            Debug.Log("camera OriPos"+m_cameraOriPos);
        }


        public void OnUpdate()
        {
            
            if (Input.touchCount == 1)
            {
                Touch touch = Input.touches[0];
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (touch.phase == TouchPhase.Began)
                {
                    m_offset = new Vector2(transform.position.x, transform.position.y) - touchPos;
                    //m_camera.GetComponent<FollowObjectCamera>().enabled = true;
                }
            }
            m_canFollow = true;
            this.enabled = true;
        }

        void CameraFollow()
        {
            if (m_canFollow)
            {
                m_nowPosition = transform.position - m_offsetBetweenCamAndPlate;
                m_nowPosition.y = Mathf.Clamp(m_nowPosition.y, minY, maxY);
                //m_nowPosition.y = Mathf.MoveTowards(m_camera.transform.position.y, m_nowPosition.y, m_speed);
                Debug.Log("现在位置" + m_nowPosition + "偏差" + m_offsetBetweenCamAndPlate);
                m_camera.transform.position = new Vector3(m_cameraOriPos.x, m_nowPosition.y, m_cameraOriPos.z);
            }

        }

        void Update()
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.touches[0];
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 currPos = touchPos + m_offset;
                    transform.position = new Vector3(currPos.x, currPos.y, transform.position.z);
                    CameraFollow();
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    m_canFollow = false;
                    if (m_isSuccess)
                    {
                        Debug.Log("盘子机关成功");
                        m_animator.enabled = true;
                        //m_animator.Play("Scene1ToScene2");
                        m_moon.transform.position = new Vector3(transform.position.x, transform.position.y, m_moon.transform.position.z);
                        m_moon.GetComponent<Animator>().enabled = true;
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
                        m_camera.transform.position = m_cameraOriPos;
                        //过渡回到原来位置
                        //m_camera.GetComponent<FollowObjectCamera>().enabled = false;
                        
                        this.enabled = false;
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
                m_canFollow = true;
                //m_camera.GetComponent<FollowObjectCamera>().enabled = true;
            }
        }

        private void OnMouseDrag()
        {
            Debug.Log("拖拽物体");
            if (Input.GetMouseButton(0))
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 currPos = touchPos + m_offset;
                transform.position = new Vector3(currPos.x, currPos.y, transform.position.z);
                CameraFollow();
            }
        }

        /// <summary>
        /// 在此处做机关触发判断
        /// </summary>
        private void OnMouseUp()
        {
            m_canFollow = false;
            Debug.Log("进来啦" + m_isSuccess);
            //先做机关破解成功判断
            if (m_isSuccess)
            {
                Debug.Log("机关成功");
                gameObject.GetComponent<Collider2D>().enabled = false;
                m_animator.enabled = true;
                m_moon.transform.position = new Vector3(transform.position.x, transform.position.y, m_moon.transform.position.z);
                m_moon.GetComponent<Animator>().enabled = true;
                //m_animator.Play("Scene1ToScene2");
                QuestController.Instance.UnRegisterQuest(gameObject.ToString());
                this.enabled = false;
            }
            else
            {

                //瞬时回到原来位置
                transform.position = new Vector3(m_originPos.x, m_originPos.y, transform.position.z);
                m_animator.enabled = false;
                m_camera.transform.position = m_cameraOriPos;
                //过渡回到原来位置
                //m_camera.GetComponent<FollowObjectCamera>().enabled = false;
                
                this.enabled = false;
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

