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

        public float speed = 2.5f;//x方向移动速率
        public float groundedRaycastDistance = 50f;
        private float touchThreshold;

        private Rigidbody2D m_rigidbody2D;
        private CapsuleCollider2D m_capsule;
        private ContactFilter2D m_contactFilter;
        private Vector2 m_destPos;//目标点
        private float m_centerHeight;
        private SpriteRenderer m_renderer;
        private Animator m_animator;
        private int IsWalking;
        private bool forwardable;

        private WalkSakura sakura;//点击反馈

        private float m_nowTouchTime;
        private float m_oldTouchTime;

        void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            forwardable = true;
            m_renderer = GetComponent<SpriteRenderer>();
            m_animator = GetComponent<Animator>();
            IsWalking = Animator.StringToHash("isWalking");
            m_capsule = GetComponent<CapsuleCollider2D>();
            m_rigidbody2D = GetComponent<Rigidbody2D>();
            m_contactFilter.layerMask = groundedLayerMask;
            m_contactFilter.useLayerMask = true;
            m_contactFilter.useTriggers = true;
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

            Debug.Log("更新目的地，目的地为:" + m_destPos);
            //控制动画播放

            //控制人物转向
            if (m_destPos.x < m_rigidbody2D.position.x)
                m_renderer.flipX = true;
            else
                m_renderer.flipX = false;
        }

        void FixedUpdate()
        {
            if (Mathf.Abs(m_rigidbody2D.position.x - m_destPos.x) < 0.01f || (m_destPos.x > m_rigidbody2D.position.x && !forwardable))
            {
                m_animator.SetBool(IsWalking, false);
                return;
            }
            //人物不设重力，手动控制人物与地形之间的关系
            Vector2 nextPos = Vector2.MoveTowards(m_rigidbody2D.position, m_destPos, speed * Time.deltaTime);
            RaycastHit2D[] hitBuffer = new RaycastHit2D[1];
   
            //地面检测
            if (Physics2D.Raycast(nextPos, Vector2.down, m_contactFilter, hitBuffer, groundedRaycastDistance) > 0)
            {
                //修正y方向位置
                //Debug.Log(hitBuffer[0].collider.gameObject.name);

                Vector2 hitPos = hitBuffer[0].point;
                Debug.DrawLine(nextPos, hitPos, Color.red);
                nextPos.y = hitPos.y + m_centerHeight;
                m_animator.SetBool(IsWalking, true);
                m_rigidbody2D.MovePosition(nextPos);
            }
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            // 判断是否撞到quest层物体，撞到代表不可继续前进，在FixedUpdate中对前进的判定直接返回

            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Quest"))
            {
                Debug.Log("撞到机关，，不可前行，目的地为当前位置");
                m_destPos = m_rigidbody2D.position;
                forwardable = false;
            }
        }

        public void OnCollisionExit2D(Collision2D collision)
        {
            // 离开quest层物体：往回走/机关完成（机关完成取消collider的情况），恢复前进判定

            if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Quest"))
                forwardable = true;
        }

        public void SetDestPos(Vector3 destPos)
        {
            m_destPos = destPos;
        }
    }
}
