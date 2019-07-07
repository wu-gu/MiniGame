using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class StandingMagpieQuest : MonoBehaviour, QuestBehavior
    {
        //业务变量
        private bool m_isSuccess = false;
        private float m_lastTime;
        public float ping;
        public AudioClip m_magpieAudioClip;
        private GameObject m_flyingMagpie;
        private Collider2D m_collider2D;

        void Start()
        {
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
            m_flyingMagpie = GameObject.Find("FlyingMagpie");
            m_collider2D = GetComponent<Collider2D>();
        }

        void Update()
        {
            if (ping > 0 && m_lastTime > 0 && Time.time - m_lastTime > ping)
            {
                m_isSuccess = true;
            }

            if(m_isSuccess)
            {
                m_isSuccess = false;
                QuestController.Instance.UnRegisterQuest(gameObject.ToString());
                Destroy(gameObject);
                m_flyingMagpie.GetComponent<Animator>().SetBool("MagpieQuestFired", true);
                AudioController.Instance.PushClip(m_magpieAudioClip);
                this.enabled = false;
            }
        }

        public void OnUpdate()
        {
            this.enabled = true;
            m_lastTime = Time.time;
        }

        // Pre-stable version
        //void Update()
        //{
        //    //OnUpdate放置在Update中会导致在屏幕任何一处点击都会触发事件，移动端待检验
        //    OnUpdate();
        //    if (ping > 0 && m_lastTime > 0 && Time.time - m_lastTime > ping)
        //    {
        //        m_isSuccess = true;
        //    }
        //}
        //public void OnUpdate()
        //{
        //    // Android
        //    //if (Input.touchCount == 1)
        //    //{
        //    //    Touch touch = Input.GetTouch(0);
        //    //    Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
        //    //    if(!m_collider2D.bounds.Contains(touchPos))
        //    //    {
        //    //        return;
        //    //    }
        //    //    m_lastTime = Time.time;
        //    //    m_collider2D.enabled = false;
        //    //}
        //    //if (m_isSuccess)
        //    //{
        //    //    //成功动画触发
        //    //    QuestController.Instance.UnRegisterQuest(gameObject.ToString();
        //    //    GameObject.Destroy(gameObject);
        //    //    //gameObject.GetComponent<Animator>().SetBool("MagpieQuestFired", true);
        //    //    m_flyingMagpie.GetComponent<Animator>().SetBool("MagpieQuestFired", true);
        //    //    AudioController.Instance.PushClip(m_magpieAudioClip);
        //    //    m_isSuccess = false;
        //    //}
        //    //else
        //    //{
        //    //    //喜鹊尖叫
        //    //}

        //    //PC端
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //        Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;
        //        if (bounds.Contains(mousePoint))
        //        {

        //            Debug.Log("点击到位，触发事件");
        //            //成功动画触发
        //            GameObject.Destroy(gameObject);
        //            //gameObject.GetComponent<Animator>().SetBool("MagpieQuestFired", true);
        //            m_flyingMagpie.GetComponent<Animator>().SetBool("MagpieQuestFired", true);
        //            AudioController.Instance.PushClip(m_magpieAudioClip);
        //        }

        //    }

        //}
    }
}

