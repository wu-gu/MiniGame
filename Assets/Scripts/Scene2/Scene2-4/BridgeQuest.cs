using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class BridgeQuest : MonoBehaviour, QuestBehavior
    {
        //业务变量
        Vector2 pre;
        Vector2 curr;
        private Vector2 m_firstDirection;
        private Vector2 m_preDirection;
        private bool m_success;
        private bool m_firstTouch;
        private CircleCollider2D m_downEdge;
        private Renderer m_firstMagpie;
        private GameObject m_buildBridgeMagpies;
        private GameObject m_flyingMagpie;

        private GameObject m_bridge;
        private float m_angleCounter;

        void Awake()
        {
            m_success = false;
        }

        void Start()
        {
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
            m_bridge = GameObject.Find("Bridge");
            m_angleCounter = 0f;
            m_firstTouch = true;
            m_downEdge = GameObject.Find("Down Edge").GetComponent<CircleCollider2D>();
            m_firstMagpie = GameObject.Find("FlyingMagpie (15)").GetComponent<Renderer>();
            m_buildBridgeMagpies = GameObject.Find("BuildingBridgeMagpies");
            m_flyingMagpie = GameObject.Find("FlyingMagpie");
            this.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            //    Vector2 touchPos = (Vector2)(transform.position);

            //    //Android--stable
            //    Touch currTouch = Input.GetTouch(0);
            //    touchPos = Camera.main.ScreenToWorldPoint(currTouch.position);

            //    // PC --stable
            //    //Vector2 currTouch = (Vector2)Input.mousePosition;
            //    //touchPos = Camera.main.ScreenToWorldPoint(currTouch);

            //    Vector2 currDirection = touchPos - (Vector2)(transform.position);
            //    m_preDirection = currDirection;
            //}

            // PC --stable
            //if (Input.GetMouseButton(0))
            //{
            //    Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //    Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;

            //    if (bounds.Contains(mousePoint) && !m_downEdge.bounds.Contains(mousePoint))
            //    {
            //        Vector2 touchPos = (Vector2)(transform.position);
            //        touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //        Vector2 currDirection = touchPos - (Vector2)(transform.position);

            //        Vector3 preDirectionVec3 = new Vector3(m_preDirection.x, m_preDirection.y, transform.position.z).normalized;
            //        Vector3 currDirectionVec3 = new Vector3(currDirection.x, currDirection.y, transform.position.z).normalized;

            //        float angle = Vector3.Angle(preDirectionVec3, currDirectionVec3);
            //        Vector3 normal = Vector3.Cross(preDirectionVec3, currDirectionVec3);

            //        //计算顺时针还是逆时针
            //        angle *= Mathf.Sign(Vector3.Dot(normal, transform.forward));

            //        if (m_angleCounter > 180)
            //        {
            //            m_success = true;
            //        }
            //        if (angle < 0)
            //        {
            //            m_angleCounter += Mathf.Abs(angle);
            //            transform.Rotate(new Vector3(0, 0, angle));
            //        }
            //        //transform.localEulerAngles = new Vector3(0.0f, 0.0f, angle);                    
            //        m_preDirection = currDirection;

            //    }

            //    else
            //    {
            //        Vector2 touchPos = (Vector2)(transform.position);
            //        Vector2 currTouch = (Vector2)Input.mousePosition;
            //        touchPos = Camera.main.ScreenToWorldPoint(currTouch);

            //        Vector2 currDirection = touchPos - (Vector2)(transform.position);
            //        m_preDirection = currDirection;
            //    }

            //}

            //Android--stable
            if (Input.touchCount == 1)
            {
                Touch currTouch = Input.GetTouch(0);
                Vector2 touchPoint = Camera.main.ScreenToWorldPoint(currTouch.position);
                Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;

                if (bounds.Contains(touchPoint) && !m_downEdge.bounds.Contains(touchPoint))
                {

                    Vector2 touchPos = (Vector2)(transform.position);
                    touchPos = Camera.main.ScreenToWorldPoint(currTouch.position);
                    Vector2 currDirection = touchPos - (Vector2)(transform.position);

                    if (currTouch.phase == TouchPhase.Moved)
                    {
                        Debug.Log("Moved");
                        Vector3 preDirectionVec3 = new Vector3(m_preDirection.x, m_preDirection.y, transform.position.z).normalized;
                        Vector3 currDirectionVec3 = new Vector3(currDirection.x, currDirection.y, transform.position.z).normalized;

                        Debug.Log("PreDirection" + preDirectionVec3);
                        Debug.Log("CurrDirection" + currDirectionVec3);

                        float angle = Vector3.Angle(preDirectionVec3, currDirectionVec3);
                        Vector3 normal = Vector3.Cross(preDirectionVec3, currDirectionVec3);

                        //计算顺时针还是逆时针
                        angle *= Mathf.Sign(Vector3.Dot(normal, transform.forward));

                        if (m_angleCounter > 180)
                        {
                            m_success = true;
                        }
                        if (angle < 0)
                        {
                            m_angleCounter += Mathf.Abs(angle);
                            transform.Rotate(new Vector3(0, 0, angle));
                        }
                        //transform.localEulerAngles = new Vector3(0.0f, 0.0f, angle);                    
                        m_preDirection = currDirection;

                    }
                }

                else
                {
                    Vector2 touchPos = (Vector2)(transform.position);
                    touchPos = Camera.main.ScreenToWorldPoint(currTouch.position);

                    Vector2 currDirection = touchPos - (Vector2)(transform.position);
                    m_preDirection = currDirection;
                }
            }


            Debug.Log("旋转到位没？" + m_success);
            if (m_success)
            {
                m_success = false;
                Debug.Log("旋转到位就执行");
                transform.localRotation = new Quaternion(0, 0, 180f, 0);
                gameObject.GetComponent<Collider2D>().enabled = false;
                m_buildBridgeMagpies.GetComponent<Animator>().SetBool("BridgeQuestFired", true);
                m_bridge.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
                Destroy(m_downEdge.gameObject);
                Destroy(m_flyingMagpie.gameObject);
                GameController.Instance.UpdateStageProgress(4);
                QuestController.Instance.UnRegisterQuest(gameObject.ToString());
            }
            
        }

        public void OnUpdate()
        {          
            Vector2 touchPos = (Vector2)(transform.position);

            //Android--stable
            Touch currTouch = Input.GetTouch(0);
            touchPos = Camera.main.ScreenToWorldPoint(currTouch.position);

            // PC --stable
            //Vector2 currTouch = (Vector2)Input.mousePosition;
            //touchPos = Camera.main.ScreenToWorldPoint(currTouch);

            if (m_firstTouch)
            {
                if (!m_firstMagpie.bounds.Contains(touchPos))
                    return;
            }

            m_firstTouch = false;
            Vector2 currDirection = touchPos - (Vector2)(transform.position);
            m_preDirection = currDirection;
            Debug.Log("Began");
            this.enabled = true;
        }
    }
}