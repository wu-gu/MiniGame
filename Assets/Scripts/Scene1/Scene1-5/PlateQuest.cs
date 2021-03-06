﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class PlateQuest : MonoBehaviour, QuestBehavior
    {
        /*业务变量*/
        //成功音乐
        public AudioClip audioClip;

        public GameObject destGameobject;//关联机关，此处为树枝
        private bool m_isSuccess = false;//此处机关是否成功破解
        private bool m_isLastSuccess = false;

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
        public float minY = 1.2f;
        public float maxY = 9.36f;
        public float followSpeed = 2.0f;
        public float returnSpeed = 2.3f;

        private bool m_canFollow = false;//相机能否跟随
        private bool m_isSlideBack = false;//相机缓缓回去原位
        private bool m_isAlreadySlideBack = true;//相机是否已经回到原位，用于避免空中接盘 

        void Start()
        {
            
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
            m_camera = GameObject.FindGameObjectWithTag("MainCamera");
            m_animator = m_camera.GetComponent<Animator>();
            m_moon = GameObject.Find("Moon");
            Debug.Log("platePriPos" + m_originPos);

            //m_cameraOriPos = m_camera.transform.position;
            //m_offsetBetweenCamAndPlate = transform.position - m_camera.transform.position;
            Debug.Log("camera OriPos"+m_cameraOriPos);

            this.enabled = false;
        }


        public void OnUpdate()
        {
            //if (Input.touchCount == 1)
            //{
            //    Touch touch = Input.touches[0];
            //    Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //    if (touch.phase == TouchPhase.Began)
            //    {
            //        //Debug.Log("盘子原始位置"+transform.position);
            //        m_camera.GetComponent<CameraController>().enabled = false;
            //        m_originPos = transform.position;
            //        m_cameraOriPos = m_camera.transform.position;
            //        m_offsetBetweenCamAndPlate = transform.position - m_camera.transform.position;
            //        m_offset = new Vector2(transform.position.x, transform.position.y) - touchPos;
            //        //m_camera.GetComponent<FollowObjectCamera>().enabled = true;
            //        m_canFollow = true;
            //        this.enabled = true;
            //    }
            //}

        }

        void CameraFollow()
        {
            if (m_canFollow)
            {
                //m_nowPosition = transform.position - m_offsetBetweenCamAndPlate;
                float y = transform.position.y - m_offsetBetweenCamAndPlate.y;
                m_nowPosition.y = y;

                m_nowPosition.y = Mathf.Clamp(m_nowPosition.y, minY, maxY);
                m_nowPosition.y = Mathf.MoveTowards(m_camera.transform.position.y, m_nowPosition.y, followSpeed * Time.deltaTime);
                m_camera.transform.position = new Vector3(m_cameraOriPos.x, m_nowPosition.y, m_cameraOriPos.z); 
                //Debug.Log("现在位置" + m_camera.transform.position + "偏差" + m_offsetBetweenCamAndPlate);
            }

        }

        void Update()
        {
            if (m_isLastSuccess)
            {
                AnimatorStateInfo m_animatorInfo = m_moon.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                if ((m_animatorInfo.normalizedTime > 1.0f) && (m_animatorInfo.IsName("MoonAppear")))
                {
                    GameController.Instance.UpdateStageProgress(4);
                    AudioController.Instance.MuteJustBackground(5);
                    StartCoroutine(WaitToTransition());
                    this.enabled = false;
                }
            }


            //if (Input.touchCount == 1)
            //{
            //    Touch touch = Input.touches[0];
            //    Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
            //    if (touch.phase == TouchPhase.Moved)
            //    {
            //        Vector2 currPos = touchPos + m_offset;
            //        transform.position = new Vector3(currPos.x, currPos.y, transform.position.z);
            //        CameraFollow();
            //    }
            //    if (touch.phase == TouchPhase.Ended)
            //    {
            //        m_canFollow = false;
            //        m_camera.GetComponent<CameraController>().enabled = true;
            //        if (m_isSuccess)
            //        {
            //            Debug.Log("盘子机关成功");
            //            AudioController.Instance.PushClip(audioClip);
            //            m_animator.enabled = true;
            //            //m_animator.Play("Scene1ToScene2");
            //            m_moon.transform.SetParent(transform.parent.parent);
            //            m_moon.GetComponent<Animator>().enabled = true;
            //            m_moon.transform.position = new Vector3(transform.position.x, transform.position.y, 15.0f);
            //            gameObject.GetComponent<Animator>().enabled = true;
            //            m_animator.SetBool("PlateQuestFired", true);
                        
            //            QuestController.Instance.UnRegisterQuest(gameObject.ToString());
            //            gameObject.GetComponent<Collider2D>().enabled = false;

            //            m_isLastSuccess = true;
                        
            //        }
            //        else
            //        {
            //            Debug.Log("盘子机关未成功");
            //            //瞬时回到原来位置
            //            m_animator.enabled = false;
            //            transform.position = new Vector3(m_originPos.x, m_originPos.y, transform.position.z);
            //            //m_camera.transform.position = m_cameraOriPos;
            //            ////过渡回到原来位置
            //            ////m_camera.GetComponent<FollowObjectCamera>().enabled = false;

            //            //this.enabled = false;

            //            m_isSlideBack = true;
            //        }
            //    }
            //}
            if (m_isSlideBack)
            {
                SlidBackOriPos();
            }
            /// <remarks>
            /// 这里的问题在于成功后脚本即被禁用 与 需要等月亮缩放到目标大小再禁用 之间的矛盾
            /// </remarks>
            //if (m_isSuccess)
            //{
            //    float destS = Mathf.MoveTowards(m_moon.transform.localScale.x, 0.3f, 0.005f);
            //    m_moon.transform.localScale = new Vector3(destS, destS, 1.0f);
            //    if(0.3f - m_moon.transform.localScale.x < 0.05f)
            //    {
            //        this.enabled = false;
            //    }
            //}
        }


        /// <summary>
        /// PC端使用
        /// </summary>
//#if UNITY_STANDALONE_WIN
        private void OnMouseDown()
        {
            if (Input.GetMouseButton(0)&& m_isAlreadySlideBack)
            {
                m_camera.GetComponent<CameraController>().enabled = false;

                m_animator.enabled = false;
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                m_offset = new Vector2(transform.position.x, transform.position.y) - touchPos;
                //Debug.Log("盘子原始位置" + transform.position);
                m_originPos = transform.position;
                m_cameraOriPos = m_camera.transform.position;
                m_offsetBetweenCamAndPlate = transform.position - m_camera.transform.position;
                m_offset = new Vector2(transform.position.x, transform.position.y) - touchPos;

                m_canFollow = true;
                m_isAlreadySlideBack = false;
                this.enabled = true;
                //m_camera.GetComponent<FollowObjectCamera>().enabled = true;
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
                CameraFollow();
                //Debug.Log("盘子位置" + transform.position);
            }
        }

        /// <summary>
        /// 在此处做机关触发判断
        /// </summary>
        private void OnMouseUp()
        {
            m_canFollow = false;
            //Debug.Log("进来啦" + m_isSuccess);
            m_camera.GetComponent<CameraController>().enabled = true;
            //先做机关破解成功判断
            if (m_isSuccess)
            {
                Debug.Log("盘子机关成功");
                gameObject.GetComponent<Collider2D>().enabled = false;
                m_animator.enabled = true;
                AudioController.Instance.PushClip(audioClip);

                m_moon.transform.SetParent(transform.parent.parent);
                Debug.Log("stage位置" + transform.parent.transform.position.x);
                Debug.Log("moon位置" + transform.position.x);
                m_moon.GetComponent<Animator>().enabled = true;
                m_moon.transform.position = new Vector3(transform.position.x, transform.position.y, 15.0f);

                m_animator.SetBool("PlateQuestFired", true);
                gameObject.GetComponent<Animator>().enabled = true;
                //m_animator.Play("Scene1ToScene2");
                QuestController.Instance.UnRegisterQuest(gameObject.ToString());
                //this.enabled = false;

                m_isLastSuccess = true;
            }
            else
            {

                //瞬时回到原来位置
                Debug.Log("盘子机关未成功");
                transform.position = new Vector3(m_originPos.x, m_originPos.y, transform.position.z);
                m_animator.enabled = false;
                //m_camera.transform.position = m_cameraOriPos;
                //this.enabled = false;
                //过渡回到原来位置
                //m_camera.GetComponent<FollowObjectCamera>().enabled = false;
                m_isSlideBack = true;
            }
        }
//#endif

        private void SlidBackOriPos()
        {
            float destY = Mathf.MoveTowards(m_camera.transform.position.y, m_cameraOriPos.y,  returnSpeed *Time.deltaTime);
            m_camera.transform.position = new Vector3(m_camera.transform.position.x, destY, 5.0f);
            if(m_camera.transform.position.y- m_cameraOriPos.y<0.1f)
            {
                m_camera.transform.position = m_cameraOriPos;
                m_isSlideBack = false;
                m_isAlreadySlideBack = true;
                this.enabled = false;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
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

        IEnumerator  WaitToTransition()
        {
            yield return new WaitForSeconds(5);
            GameController.Instance.TransitionToNextLevel();
        }
    }
}

