using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{

    public class EncounterAnimation : MonoBehaviour
    {
        private GameObject m_shield;
        private GameObject m_girl;
        private GameObject m_boy;
        private Animator m_girlAnimator;
        // Start is called before the first frame update
        void Start()
        {
            m_girl = GameObject.FindGameObjectWithTag("Girl");
            m_boy = GameObject.Find("Couple");
            m_shield = GameObject.Find("Shield");
            m_girlAnimator = m_girl.GetComponent<Animator>();
        }

        private void Update()
        {
            AnimatorStateInfo stateInfo = m_girlAnimator.GetCurrentAnimatorStateInfo(0);
            //if (stateInfo.normalizedTime > 1.0f && stateInfo.IsName("Stage1To2")&&isFirst)
            if (stateInfo.normalizedTime > 1.0f && stateInfo.IsName("GirlShy"))
            {
                StartCoroutine(TransitionToEnd());
                this.enabled=false;
            }
        }

        void DestoryShield()
        {
            GameObject.Destroy(m_shield);
        }

        void MeetingTriggerFired()
        {
            m_boy.GetComponent<Animator>().SetBool("MeetingTriggerFired", true);
            m_girl.GetComponent<Animator>().SetBool("MeetingTriggerFired", true);
        }

        IEnumerator TransitionToEnd()
        {
            yield return new WaitForSeconds(6);
            GameController.Instance.TransitionToNewLevel("End");
        }
    }
}
