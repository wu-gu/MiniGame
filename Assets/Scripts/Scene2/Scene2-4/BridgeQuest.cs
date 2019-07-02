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

        private GameObject m_bridge;
        private bool m_angleCounter;

        void Awake()
        {
            m_success = false;
        }

        void Start()
        {
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
            m_bridge = GameObject.Find("Bridge");
            m_angleCounter
            //this.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            // PC --stable
            if (Input.GetMouseButton(0))
            {
                Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;
                if (bounds.Contains(mousePoint))
                {
                    Vector2 touchPos = (Vector2)(transform.position);
                    touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 currDirection = touchPos - (Vector2)(transform.position);

                    Vector3 preDirectionVec3 = new Vector3(m_preDirection.x, m_preDirection.y, transform.position.z).normalized;
                    Vector3 currDirectionVec3 = new Vector3(currDirection.x, currDirection.y, transform.position.z).normalized;

                    float angle = Vector3.Angle(preDirectionVec3, currDirectionVec3);
                    Vector3 normal = Vector3.Cross(preDirectionVec3, currDirectionVec3);

                    //计算顺时针还是逆时针
                    angle *= Mathf.Sign(Vector3.Dot(normal, transform.forward));
                    //transform.Rotate(new Vector3(0, 0, angle));
                    transform.localEulerAngles = new Vector3(0.0f, 0.0f, angle);
                    m_preDirection = currDirection;
                    if (Mathf.Abs(angle) > 100)
                    {
                        m_success = true;
                    }
                }
                    
            }
            //Android--stable
            if (Input.touchCount > 0)
            {
                Vector2 touchPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;
                if (bounds.Contains(touchPoint))
                {
                    Touch currTouch = Input.GetTouch(0);
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
                        Debug.Log(angle);
                        transform.Rotate(new Vector3(0, 0, angle));
                        m_preDirection = currDirection;
                        if (Mathf.Abs(angle) > 100)
                        {
                            m_success = true;
                        }
                    }
                }
            }
            Debug.Log("旋转到位没？" + m_success);
            if (m_success)
            {
                m_success = false;
                Debug.Log("旋转到位就执行");
                transform.Rotate(new Vector3(0, 0, 180));
                gameObject.GetComponent<Collider2D>().enabled = false;
                m_bridge.GetComponent<Collider2D>().enabled = true;
                QuestController.Instance.UnRegisterQuest(gameObject.ToString());
            }
            //this.enabled = false;
        }

        public void OnUpdate()
        {
            Vector2 touchPos = (Vector2)(transform.position);

            //Android--stable
            //Touch currTouch = Input.GetTouch(0);
            //touchPos = Camera.main.ScreenToWorldPoint(currTouch.position);

            // PC --stable
            Vector2 currTouch = (Vector2)Input.mousePosition;
            touchPos = Camera.main.ScreenToWorldPoint(currTouch);

            Vector2 currDirection = touchPos - (Vector2)(transform.position);
            m_preDirection = currDirection;
            Debug.Log("Began");
            this.enabled = true;
        }
    }
}