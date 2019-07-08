using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{

    public class CatControl : MonoBehaviour, QuestBehavior
    {
        private FollowPlayerCamera m_follw;
        private GameObject m_girl;
        private GameObject m_desout;
        private Vector3 m_girlposition;
        private Vector3 m_outposition;
        private Animator m_cataimator;
        private Collider2D m_catcolider2D;
        private Collider2D m_doorcolider2D;
        private SpriteRenderer m_catrenderer;
        public float speed;
        public float Xoffset;
        public float Yoffset;
        private bool isIn = true;
        private bool isOut = false;

        // Start is called before the first frame update
        void Start()
        {
            m_girl = GameObject.Find("ground");
            m_desout = GameObject.Find("catdes");
            m_follw = GameObject.Find("Main Camera").GetComponent<FollowPlayerCamera>();
            m_cataimator = GetComponent<Animator>();
            m_catcolider2D = GetComponent<Collider2D>();
            m_doorcolider2D = GameObject.Find("Door").GetComponent<Collider2D>();
            m_catrenderer = GetComponent<SpriteRenderer>();
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
        }

        // Update is called once per frame
        void Update()
        {
            if (isIn)
            {
                m_girlposition = m_girl.transform.position;
                m_girlposition = new Vector3(m_girlposition.x - Xoffset, m_girlposition.y - Yoffset, 0);
                if (Vector3.Distance(m_girlposition, this.transform.position) > 0.1f)
                {
                    Vector3 offSet = m_girlposition - this.transform.position;
                    transform.position += offSet.normalized * speed * Time.deltaTime;
                }
                else
                {
                    m_catcolider2D.enabled = true;
                    m_cataimator.SetBool("catlook", true);
                    isIn = false;
                }
            }
            if (isOut)
            {
                m_catcolider2D.enabled = false;
                m_catrenderer.flipX = false;
                m_cataimator.SetBool("catlook", false);
                m_cataimator.SetBool("catwalk", true);
                m_doorcolider2D.enabled = true;
                QuestController.Instance.UnRegisterQuest(gameObject.ToString());
                m_outposition = m_desout.transform.position;
                if (Vector3.Distance(m_outposition, this.transform.position) > 0.1f)
                {
                    Vector3 offSet = m_outposition - this.transform.position;
                    transform.position += offSet.normalized * speed * Time.deltaTime;
                }
                else
                {
                    InputController.Instance.SetPlayerCanMove(true);
                    m_follw.enabled = true;
                    Destroy(gameObject);
                }
            }
        }

        public void OnUpdate()
        {
            isOut = true;
        }
    }
}