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

        void Awake()
        {
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
        }
        
        void Update()
        {
            OnUpdate();
            if (ping > 0 && m_lastTime > 0 && Time.time - m_lastTime > ping)
            {
                m_isSuccess = true;
            }
        }

        public void OnUpdate()
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.touches[0];
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (touch.phase == TouchPhase.Began)
                {
                    m_lastTime = Time.time;
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    //先做机关破解成功判断
                    if (m_isSuccess)
                    {
                        //成功动画触发
                        gameObject.GetComponent<Animator>().SetBool("MagpieQuestFired", true);
                        AudioController.Instance.PushClip(m_magpieAudioClip);
                    }
                    else
                    {
                        //喜鹊尖叫
                    }
                }
            }
            else
            {
                //PC端
                if (Input.GetMouseButtonDown(0))
                {
                    //成功动画触发
                    gameObject.GetComponent<Animator>().SetBool("MagpieQuestFired", true);
                    AudioController.Instance.PushClip(m_magpieAudioClip);
                }
            }
        }
    }
}

