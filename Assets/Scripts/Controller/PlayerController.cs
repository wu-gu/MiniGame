using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class PlayerController : MonoSingleton<PlayerController>
    {
        [Tooltip("The Layers which represent gameobjects that the Character Controller can be grounded on.")]
        public LayerMask groundedLayerMask;

        public float speed = 3.0f;//x方向移动速率
        public float groundedRaycastDistance = 50f;
        private float touchThreshold;

        private Rigidbody2D m_rigidbody2D;
        private CapsuleCollider2D m_capsule;
        private ContactFilter2D m_contactFilter;
        private Animator m_animator;

        private Vector2 m_destPos;//目标点
        private float m_centerHeight;

        private WalkSakura sakura;//点击反馈

        void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            m_capsule = GetComponent<CapsuleCollider2D>();
            m_rigidbody2D = GetComponent<Rigidbody2D>();
            m_contactFilter.layerMask = groundedLayerMask;
            m_contactFilter.useLayerMask = true;
            m_contactFilter.useTriggers = false;
            m_animator = GetComponent<Animator>();
            sakura = GameObject.Find("OnClickSakura").GetComponent<WalkSakura>();

            m_destPos = m_rigidbody2D.position;
            touchThreshold = m_capsule.bounds.max.y- m_capsule.bounds.min.y + 0.8f;
            //Debug.Log("touchThreold" + touchThreshold);

            RaycastHit2D[] hitBuffer = new RaycastHit2D[1];
            if(Physics2D.Raycast(m_rigidbody2D.position, Vector2.down, m_contactFilter, hitBuffer, groundedRaycastDistance) > 0)
            {
                Vector2 hitPos = hitBuffer[0].point;
                m_centerHeight = (m_rigidbody2D.position - hitPos).y;
            }

        }

        public void OnUpdate()
        {
            //PC端鼠标左键点击
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.down, touchThreshold, groundedLayerMask.value);
                if (hit.collider != null)
                {
                    m_destPos = touchPos;
                    sakura.SetSakura(m_destPos);
                    //控制人物转向
                    if (m_destPos.x > m_rigidbody2D.position.x)
                    {
                        GetComponent<SpriteRenderer>().flipX = false;
                    }
                    else
                    {
                        GetComponent<SpriteRenderer>().flipX = true;
                    }
                }
            }

            //Android端单击
            if (Input.touchCount == 1)
            {
                Touch touch = Input.touches[0];
                if (touch.phase == TouchPhase.Ended)
                {
                    Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                    RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.down, touchThreshold, groundedLayerMask.value);
                    if (hit.collider != null)
                    {
                        m_destPos = touchPos;
                        sakura.SetSakura(m_destPos);
                    }
                }
            }

        }

        void FixedUpdate()
        {
            if (Mathf.Approximately(m_destPos.x,m_rigidbody2D.position.x))
            //if (Mathf.Abs(m_destPos.x-m_rigidbody2D.position.x)<0.01f)
            {
                Debug.Log("走到目的地了");
                m_animator.SetBool("isWalking", false);
                return;
            }

            //人物不设重力，手动控制人物与地形之间的关系
            Vector2 nextPos = Vector2.MoveTowards(m_rigidbody2D.position, m_destPos, speed * Time.deltaTime);

            if (Mathf.Abs(nextPos.x- m_destPos.x)<0.1f){
                nextPos.x = m_destPos.x;
            }


            //Vector2 nextPos = Vector2.Lerp(m_rigidbody2D.position, m_destPos, speed * Time.deltaTime);
            //Debug.Log("rigidPos" + m_rigidbody2D.position.ToString("f10"));
            //Debug.Log("m_destPos" + m_destPos.ToString("f10"));
            RaycastHit2D[] hitBuffer = new RaycastHit2D[1];
            //地面检测
            //此处需要注意netxPos的位置如果就在地面以下会出问题，后期可以考虑改正
            if (Physics2D.Raycast(nextPos, Vector2.down, m_contactFilter, hitBuffer, groundedRaycastDistance) > 0)
            {
                //修正y方向位置
                //Debug.Log(hitBuffer[0].collider.gameObject.name);

                Vector2 hitPos = hitBuffer[0].point;
                //Debug.DrawLine(nextPos, hitPos, Color.red);
                nextPos.y = hitPos.y + m_centerHeight;
                m_animator.SetBool("isWalking", true);
                m_rigidbody2D.MovePosition(nextPos);
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            Debug.Log("collision");
            Debug.Log(collision.collider.gameObject.layer + " haha " + InputController.Instance.questLayerMask.value);
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Quest"))
            {
                Debug.Log("blocked");
                m_destPos = m_rigidbody2D.position;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("enter");
        }
    }
}
