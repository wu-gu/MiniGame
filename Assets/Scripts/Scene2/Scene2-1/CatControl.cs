using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{

    public class CatControl : MonoBehaviour, QuestBehavior
    {
        private CameraController m_follw;
        private Animator m_mainCamAnimator;
        private GameObject m_ground;
        private GameObject m_desout;
        private Vector3 m_girlposition;
        private Vector3 m_outposition;
        private Animator m_cataimator;
        private Collider2D m_catcolider2D;
        private Collider2D m_doorcolider2D;
        private SpriteRenderer m_catrenderer;
        public float speed;
        public float Xoffset;
        public float FilpXoffset;
        public float Yoffset;
        private bool isIn = true;
        private bool isOut = false;

        // Start is called before the first frame update
        void Start()
        {
            GameObject mainCam = GameObject.FindGameObjectWithTag("MainCamera");
            m_ground = GameObject.Find("ground");
            m_desout = GameObject.Find("catdes");
            m_follw = mainCam.GetComponent<CameraController>();
            m_mainCamAnimator = mainCam.GetComponent<Animator>();
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
                m_girlposition = m_ground.transform.position;
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
                    m_mainCamAnimator.enabled = false;
                    Destroy(gameObject);

                    string nextStageName = "Stage2-2";
                    Debug.Log("Load 2-2");
                    if (nextStageName != null)
                    {
                        GameController.Instance.LoadNextStageGameObjects(nextStageName);
                        //调整摄像机边界限定
                        //Camera.main.gameObject.GetComponent<CameraController>().UpdateBackgounrdLeft(GameObject.Find(nextStageName+"-L"));
                        //加载时调整右边，卸载时调整左边
                        Camera.main.gameObject.GetComponent<CameraController>().UpdateBackgounrdRight(GameObject.Find(nextStageName + "-R"));
                        Destroy(this);
                    }
                    
                }
            }
        }

        public void OnUpdate()
        {
            isOut = true;
            GameObject.FindGameObjectWithTag("Girl").GetComponent<SpriteRenderer>().flipX = true;
            transform.position = new Vector3(transform.position.x +FilpXoffset, transform.position.y, transform.position.z);
            m_catrenderer.flipX = false;
        }
    }
}