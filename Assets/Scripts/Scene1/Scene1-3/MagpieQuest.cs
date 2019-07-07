//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace MiniGame
//{
//    public class MagpieQuest : MonoBehaviour, QuestBehavior
//    {
//        //业务变量
//        private bool m_isSuccess = false;
//        private float m_lastTime;
//        public float ping;
//        public AudioClip m_magpieAudioClip;

//        void Start()
//        {
//            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
//        }

//        void Update()
//        {
//            OnUpdate();
//            if (ping > 0 && m_lastTime > 0 && Time.time - m_lastTime > ping)
//            {
//                m_isSuccess = true;
//            }
//        }

//        public void OnUpdate()
//        {
//            if (Input.touchCount == 1)
//            {
//                Touch touch = Input.touches[0];
//                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//                if (touch.phase == TouchPhase.Began)
//                {
//                    m_lastTime = Time.time;
//                }
//                if (touch.phase == TouchPhase.Ended)
//                {
//                    //先做机关破解成功判断
//                    if (m_isSuccess)
//                    {
//                        //成功动画触发
//                        Debug.Log("喜鹊点击机关成功");
//                        gameObject.GetComponent<Animator>().SetBool("MagpieQuestFired", true);
//                        AudioController.Instance.PushClip(m_magpieAudioClip);
//                        m_isSuccess = false;

//                    }
//                    else
//                    {
//                        Debug.Log("喜鹊点击机关不成功");
//                        //喜鹊尖叫
//                    }
//                }
//            }
//            else
//            {
//                //PC端

//                if (Input.GetMouseButtonDown(0))
//                {
//                    //Debug.Log("进来PC");
//                    Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//                    Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;

//                    if (bounds.Contains(mousePoint))
//                    {
//                        //Debug.Log("进来没" + bounds.Contains(mousePoint));
//                        //成功动画触发
//                        Debug.Log("喜鹊点击机关成功");
//                        gameObject.GetComponent<Animator>().SetBool("MagpieQuestFired", true);
//                        AudioController.Instance.PushClip(m_magpieAudioClip);
//                        m_isSuccess = false;
//                    }

//                }
//            }
//        }

//        private void OnMouseDown()
//        {
//            if (Input.GetMouseButtonDown(0))
//            {
//                Debug.Log("进来PC");
//                Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//                Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;
//                Debug.Log("进来没" + bounds.Contains(mousePoint));
//                if (bounds.Contains(mousePoint))
//                {
//                    //Debug.Log("进来没yyyy" + bounds.Contains(mousePoint));
//                    //成功动画触发
//                    Debug.Log("喜鹊点击机关成功");
//                    gameObject.GetComponent<Animator>().SetBool("MagpieQuestFired", true);
//                    AudioController.Instance.PushClip(m_magpieAudioClip);
//                    m_isSuccess = false;
//                }

//            }
//        }
//    }


//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class MagpieQuest : MonoBehaviour, QuestBehavior
    {
        //业务变量
        private bool m_isSuccess = false;
        private float m_lastTime;
        public float ping;
        public AudioClip m_magpieAudioClip;

        void Start()
        {
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
            this.enabled = false;
        }

        void Update()
        {
            //if (ping > 0 && m_lastTime > 0 && Time.time - m_lastTime > ping)
            //{
            //    m_isSuccess = true;
            //}

            if (Input.touchCount == 1)
            {
                Touch touch = Input.touches[0];
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //if (touch.phase == TouchPhase.Began)
                //{
                //    m_lastTime = Time.time;
                //}
                if (touch.phase == TouchPhase.Ended)
                {
                    //先做机关破解成功判断
                    m_isSuccess = true;
                    if (m_isSuccess)
                    {
                        //成功动画触发
                        Debug.Log("喜鹊点击机关成功");
                        gameObject.GetComponent<Animator>().SetBool("MagpieQuestFired", true);
                        AudioController.Instance.PushClip(m_magpieAudioClip);
                        QuestController.Instance.UnRegisterQuest(gameObject.ToString());
                        Destroy(this);

                    }
                    else
                    {
                        Debug.Log("喜鹊点击机关不成功");
                        //喜鹊尖叫
                    }
                }
            }
            //else
            //{
            //    //PC端

            //    if (Input.GetMouseButtonDown(0))
            //    {
            //        //Debug.Log("进来PC");
            //        Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //        Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;

            //        if (bounds.Contains(mousePoint))
            //        {
            //            //Debug.Log("进来没" + bounds.Contains(mousePoint));
            //            //成功动画触发
            //            Debug.Log("喜鹊点击机关成功");
            //            gameObject.GetComponent<Animator>().SetBool("MagpieQuestFired", true);
            //            AudioController.Instance.PushClip(m_magpieAudioClip);
            //            m_isSuccess = false;
            //        }

            //    }
            //}
        }

        public void OnUpdate()
        {
            this.enabled = true;
        }

        private void OnMouseDown()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("进来PC");
                m_isSuccess = true;
                //Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;
                //Debug.Log("进来没" + bounds.Contains(mousePoint));
                if (m_isSuccess)
                {
                    //Debug.Log("进来没yyyy" + bounds.Contains(mousePoint));
                    //成功动画触发
                    Debug.Log("喜鹊点击机关成功");
                    gameObject.GetComponent<Animator>().SetBool("MagpieQuestFired", true);
                    AudioController.Instance.PushClip(m_magpieAudioClip);
                    QuestController.Instance.UnRegisterQuest(gameObject.ToString());
                    Destroy(this);
                }

            }
        }
    }


}



