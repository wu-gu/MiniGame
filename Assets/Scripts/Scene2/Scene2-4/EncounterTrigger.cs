using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class EncounterTrigger : MonoBehaviour
    {
        [Tooltip("2-5相遇音乐")]
        public AudioClip meetingClip;

        private GameObject m_girl;
        private GameObject m_couple;
        private GameObject m_girlCover;
        private Animator m_mainCamAnimator;
        private int IsFocusTime;

        private void Start()
        {
            m_girl = GameObject.FindGameObjectWithTag("Girl");
            m_couple = GameObject.Find("Couple");
            m_girlCover = GameObject.Find("GirlCover");
            m_mainCamAnimator = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
            IsFocusTime = Animator.StringToHash("IsFocusTime");
            Debug.Log("找到Couple" + m_couple);
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            Debug.Log("enter");
            if (collider.gameObject == m_girl)
            {
                m_girl.transform.SetParent(m_girlCover.transform);
                m_girl.GetComponent<PlayerController>().enabled = false;
                InputController.Instance.SetPlayerCanMove(false);
                gameObject.GetComponent<Collider2D>().enabled = false;
                StartCoroutine(WaitToChangeBackground());
            }
        }

        IEnumerator WaitToChangeBackground()
        {
            AudioController.Instance.MuteJustBackground(1.0f);
            yield return new WaitForSeconds(1.0f);
            AudioController.Instance.ChangeBackground(meetingClip);
            AudioController.Instance.PlayJustBackground();
            AudioController.Instance.UnmuteJustBackground(1.0f);

            m_couple.GetComponent<Animator>().enabled = true;
            m_mainCamAnimator.enabled = true;
            m_mainCamAnimator.SetTrigger(IsFocusTime);
        }
    }
}

