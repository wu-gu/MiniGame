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
            //控制动画播放

            //控制人物转向

        }

        void FixedUpdate()
        {
            if (m_destPos == m_rigidbody2D.position)
                return;
            //人物不设重力，手动控制人物与地形之间的关系
            Vector2 nextPos = Vector2.MoveTowards(m_rigidbody2D.position, m_destPos, speed * Time.deltaTime);
            RaycastHit2D[] hitBuffer = new RaycastHit2D[1];
            //地面检测
            if (Physics2D.Raycast(nextPos, Vector2.down, m_contactFilter, hitBuffer, groundedRaycastDistance) > 0)
            {
                //修正y方向位置
                //Debug.Log(hitBuffer[0].collider.gameObject.name);

                Vector2 hitPos = hitBuffer[0].point;
                //Debug.DrawLine(nextPos, hitPos, Color.red);
                nextPos.y = hitPos.y + m_centerHeight;
                m_rigidbody2D.MovePosition(nextPos);
            }
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Quest"))
            {
                //Debug.Log("blocked");
                m_destPos = m_rigidbody2D.position;
            }
        }
    }
}
