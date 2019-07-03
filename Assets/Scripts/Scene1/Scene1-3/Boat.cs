using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class Boat : MonoBehaviour
    {
        private GameObject m_character;
        private Vector2 m_offset;
        private Vector2 m_nowPosition;
        private Vector2 m_oldPosition;
        private bool m_playerInBoat = false;
        private GameObject m_boat;
        private GameObject m_camera;
        //private AnimatorStateInfo m_animatorInfo;

        private void Start()
        {
            m_character = GameObject.Find("Boy");
            m_camera = GameObject.Find("Additional Camera");
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            Debug.Log("enter");
            if (collider.gameObject.name == "Boy")
            {
                m_oldPosition = m_character.transform.position;
                PlayerController.Instance.enabled = false;
                m_playerInBoat = true;
                Debug.Log("people enter");
                m_offset = (Vector2)transform.position - m_oldPosition;
                Debug.Log("offset" + m_offset);

                m_boat = GameObject.Find("Boat");
                m_boat.GetComponent<Animator>().SetBool("PlayerOnBoatTriggerFired", true);
                m_camera.GetComponent<Animator>().enabled = true;
                //collider.gameObject.transform.SetParent(transform);
            }
            if (collider.gameObject.name == "Embankment1")
            {
                Debug.Log("EmbankmentTriggerFired");
                m_playerInBoat = false;
                //collider.gameObject.transform.SetParent(transform.parent);
                GetComponent<Collider2D>().enabled = false;
                PlayerController.Instance.enabled = true;
            }
        }

        void LateUpdate()
        {
            if (m_playerInBoat)
            {
                m_nowPosition = (Vector2)transform.position - m_offset;
                m_character.transform.position = new Vector3(m_nowPosition.x, m_nowPosition.y, 0.0f);
            }
            
            //m_animatorInfo = m_boat.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            //if ((m_animatorInfo.normalizedTime > 1.0f) && (m_animatorInfo.IsName("BoatMove")))//normalizedTime: 范围0 -- 1, 0是动作开始，1是动作结束
            //{
            //    m_playerInBoat = false;
            //    //collider.gameObject.transform.SetParent(transform.parent);
            //    GetComponent<Collider2D>().enabled = false;
            //    PlayerController.Instance.enabled = true;
            //}
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            Debug.Log("exit");
            if (collider.gameObject.name == "Boy")
            {
                m_playerInBoat = false;
                //collider.gameObject.transform.SetParent(transform.parent);
            }
        }
    }

}
