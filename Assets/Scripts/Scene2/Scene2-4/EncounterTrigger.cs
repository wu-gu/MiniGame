using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class EncounterTrigger : MonoBehaviour
    {
        private GameObject m_girl;
        private GameObject m_couple;
        private GameObject m_girlCover;
        private Animator m_mainCam;
        private int IsFocusTime;

        private void Start()
        {
            m_girl = GameObject.FindGameObjectWithTag("Girl");
            m_couple = GameObject.Find("Couple");
            m_girlCover = GameObject.Find("GirlCover");
            m_mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
            IsFocusTime = Animator.StringToHash("IsFocusTime");
            Debug.Log("找到Couple" + m_couple);
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            Debug.Log("enter");
            if (collider.gameObject == m_girl)
            {
                m_girl.transform.SetParent(m_girlCover.transform);
                InputController.Instance.SetPlayerCanMove(false);
                m_girl.GetComponent<PlayerController>().enabled = false;
                gameObject.GetComponent<Collider2D>().enabled = false;
                m_couple.GetComponent<Animator>().enabled = true;
                m_mainCam.enabled = true;
                m_mainCam.SetTrigger(IsFocusTime);
            }
        }
    }
}

