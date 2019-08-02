using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(AudioSource))]
    public class PlayerController2 : MonoSingleton<PlayerController2>
    {
        public AudioClip walkingMusic;
        [Tooltip("The Layers which represent gameobjects that the Character Controller can be grounded on.")]
        public LayerMask groundedLayerMask;

        public float speed = 2.5f;//x方向移动速率
        public float groundedRaycastDistance = 50f;
        private float touchThreshold;

        private Rigidbody2D m_rigidbody2D;
        private CapsuleCollider2D m_capsule;
        private ContactFilter2D m_contactFilter;
        private AudioSource m_audioSource;

        private Vector2 m_destPos;//目标点
        private float m_centerHeight;
        private SpriteRenderer m_renderer;
        private Animator m_animator;
        private int IsWalking;
        private bool forwardable;

        private WalkSakura sakura;//点击反馈

        //女主控制
        public GameObject girl;
        private Rigidbody2D m_girlRigidbody2D;
        private Animator m_girlAnimator;
        private float m_girlCenterHeight;
        private SpriteRenderer m_girlRenderer;
        private CapsuleCollider2D m_girlCapsuleCollider2D;


        //当前控制的是谁
        public int m_whoInControl;//0为男主，1为女主
        private float m_followDistance;
        private bool m_canAnotherFollow = true;


        void Awake()
        {

            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            //设置人物脚步声
            m_audioSource = GetComponent<AudioSource>();
            m_audioSource.clip = walkingMusic;
            m_audioSource.volume = AudioController.Instance.soundEffectVolume;

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
            touchThreshold = m_capsule.bounds.max.y - m_capsule.bounds.min.y + 0.8f;

            //女主获取
            m_girlRigidbody2D = girl.GetComponent<Rigidbody2D>();
            m_girlAnimator = girl.GetComponent<Animator>();
            m_girlRenderer = girl.GetComponent<SpriteRenderer>();
            m_girlCapsuleCollider2D = girl.GetComponent<CapsuleCollider2D>();

            m_followDistance = m_rigidbody2D.position.x - m_girlRigidbody2D.position.x;

            //Debug.Log("touchThreold" + touchThreshold);

            //因为人的包围盒的中心与Transform的中心不一样
            UpdateCenterHeight();
        }

        private void OnEnable()
        {
            SelfDestination(); UpdateCenterHeight();
        }

        /// <summary>
        /// 如果人物正在走路，禁用人物控制使人停止走路
        /// </summary>
        private void OnDisable()
        {
            m_animator.SetBool(IsWalking, false);
            m_girlAnimator.SetBool("isWalking", false);
        }

        //此处也分流，分为人物点击判断与人物行走点击判断
        //暂时先不在InitSetting中设置双人模式与单人模式而调用不同的单例，看第3关要不要整合先

        public void OnUpdate()
        {
            //PC端鼠标左键点击
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


                //点人的话就返回
                if (m_capsule.bounds.Contains(touchPos))
                {
                    setWhoInControl(0);
                    return;
                }
                else if (m_girlCapsuleCollider2D.bounds.Contains(touchPos))
                {
                    setWhoInControl(1);
                    return;
                }

                RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.down, touchThreshold, groundedLayerMask.value);
                if (hit.collider != null)
                {
                    m_destPos = touchPos;
                    sakura.SetSakura(new Vector3(m_destPos.x, m_destPos.y, this.transform.position.z));
                    //控制人物转向
                    if (m_destPos.x < m_rigidbody2D.position.x)
                    {
                        Debug.Log("左转");
                        if (m_whoInControl == 0)
                            m_renderer.flipX = true;
                        if (m_whoInControl == 1)
                            m_girlRenderer.flipX = false;
                    }
                    else
                    {
                        Debug.Log("右转");

                        if (m_whoInControl == 0)
                            m_renderer.flipX = false;
                        if (m_whoInControl == 1)
                            m_girlRenderer.flipX = true;
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

                    if (m_capsule.bounds.Contains(touchPos))
                    {
                        m_whoInControl = 0;
                        return;
                    }
                    else if (m_girlCapsuleCollider2D.bounds.Contains(touchPos))
                    {
                        m_whoInControl = 1;
                        return;
                    }


                    RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.down, touchThreshold, groundedLayerMask.value);
                    if (hit.collider != null)
                    {
                        m_destPos = touchPos;
                        sakura.SetSakura(new Vector3(m_destPos.x, m_destPos.y, this.transform.position.z));
                        //控制人物转向
                        if (m_destPos.x < m_rigidbody2D.position.x)
                        {
                            Debug.Log("左转");
                            if (m_whoInControl == 0)
                                m_renderer.flipX = true;
                            if (m_whoInControl == 1)
                                m_girlRenderer.flipX = false;
                        }
                        else
                        {
                            Debug.Log("右转");

                            if (m_whoInControl == 0)
                                m_renderer.flipX = false;
                            if (m_whoInControl == 1)
                                m_girlRenderer.flipX = true;
                        }
                    }
                }
            }

            //Debug.Log("更新目的地，目的地为:" + m_destPos);
            //控制动画播放                
        }

        void FixedUpdate()
        {
            Debug.Log("另一个人是否能跟随" + m_canAnotherFollow);
            //Debug.Log("刚体位置"+m_rigidbody2D.position.ToString("f9"));
            //Debug.Log("目的地"+m_destPos.ToString("f9"));
            //Debug.Log(Mathf.Abs(m_rigidbody2D.position.x - m_destPos.x) < 0.1f);
            //Debug.Log((m_destPos.x > m_rigidbody2D.position.x && !forwardable));

            //控制男主
            if (m_whoInControl == 0)
            {
                if (Mathf.Abs(m_rigidbody2D.position.x - m_destPos.x) < 0.1f || (m_destPos.x > m_rigidbody2D.position.x && !forwardable))
                {
                    Debug.Log("走不动了");
                    m_animator.SetBool(IsWalking, false);
                    m_girlAnimator.SetBool("isWalking", false);
                    m_destPos = m_rigidbody2D.position;
                    return;
                }

                /*人物不设重力，手动控制人物与地形之间的关系*/
                //男主移动
                Vector2 nextPos = Vector2.MoveTowards(m_rigidbody2D.position, m_destPos, speed * Time.deltaTime);
                RaycastHit2D[] hitBuffer = new RaycastHit2D[1];
                //地面检测
                if (Physics2D.Raycast(nextPos, Vector2.down, m_contactFilter, hitBuffer, groundedRaycastDistance) > 0)
                {
                    //修正y方向位置
                    //Debug.Log("地面"+hitBuffer[0].collider.gameObject.name);

                    Vector2 hitPos = hitBuffer[0].point;
                    Debug.DrawLine(nextPos, hitPos, Color.red);
                    nextPos.y = hitPos.y + m_centerHeight;
                    //设置人物行走动画与音乐
                    m_animator.SetBool(IsWalking, true);
                    m_rigidbody2D.MovePosition(nextPos);
                }

                //女主转身
                m_girlRenderer.flipX = m_rigidbody2D.position.x > m_girlRigidbody2D.position.x;
                m_followDistance = Mathf.Sign(m_rigidbody2D.position.x - m_girlRigidbody2D.position.x) * Mathf.Abs(m_followDistance);
                if (Mathf.Abs(m_rigidbody2D.position.x - m_girlRigidbody2D.position.x) - Mathf.Abs(m_followDistance) > 0)
                {
                    m_canAnotherFollow = true;
                }
                else
                {
                    m_canAnotherFollow = false;
                }
                //女主移动
                if (m_canAnotherFollow)
                {
                    Vector2 girlNextPos = new Vector2(nextPos.x - m_followDistance, m_girlRigidbody2D.position.y);
                    if (Physics2D.Raycast(girlNextPos, Vector2.down, m_contactFilter, hitBuffer, groundedRaycastDistance) > 0)
                    {
                        //修正y方向位置
                        //Debug.Log("地面"+hitBuffer[0].collider.gameObject.name);

                        Vector2 hitPos = hitBuffer[0].point;
                        Debug.DrawLine(girlNextPos, hitPos, Color.red);
                        girlNextPos.y = hitPos.y + m_girlCenterHeight;
                        //设置人物行走动画与音乐
                        m_girlAnimator.SetBool("isWalking", true);
                        m_girlRigidbody2D.MovePosition(girlNextPos);
                    }
                }
            }
            //控制女主
            else
            {
                if (Mathf.Abs(m_girlRigidbody2D.position.x - m_destPos.x) < 0.1f || (m_destPos.x > m_girlRigidbody2D.position.x && !forwardable))
                {
                    Debug.Log("走不动了");
                    m_animator.SetBool(IsWalking, false);
                    m_girlAnimator.SetBool("isWalking", false);
                    m_destPos = m_girlRigidbody2D.position;
                    return;
                }

                /*人物不设重力，手动控制人物与地形之间的关系*/
                //男主移动
                Vector2 nextPos = Vector2.MoveTowards(m_girlRigidbody2D.position, m_destPos, speed * Time.deltaTime);
                RaycastHit2D[] hitBuffer = new RaycastHit2D[1];
                //地面检测
                if (Physics2D.Raycast(nextPos, Vector2.down, m_contactFilter, hitBuffer, groundedRaycastDistance) > 0)
                {
                    //修正y方向位置
                    //Debug.Log("地面"+hitBuffer[0].collider.gameObject.name);

                    Vector2 hitPos = hitBuffer[0].point;
                    Debug.DrawLine(nextPos, hitPos, Color.red);
                    nextPos.y = hitPos.y + m_girlCenterHeight;
                    //设置人物行走动画与音乐
                    m_girlAnimator.SetBool("isWalking", true);
                    m_girlRigidbody2D.MovePosition(nextPos);
                }

                //男主转身
                m_renderer.flipX = m_rigidbody2D.position.x > m_girlRigidbody2D.position.x;
                m_followDistance = Mathf.Sign(m_rigidbody2D.position.x - m_girlRigidbody2D.position.x) * Mathf.Abs(m_followDistance);
                //因为被跟随者先移动，所以被跟随者转身时另一个人不能移动
                if (Mathf.Abs(m_rigidbody2D.position.x - m_girlRigidbody2D.position.x) - Mathf.Abs(m_followDistance) > 0)
                {
                    m_canAnotherFollow = true;
                }
                else
                {
                    m_canAnotherFollow = false;
                }
                //男主移动
                if (m_canAnotherFollow)
                {
                    Vector2 anotherNextPos = new Vector2(nextPos.x + m_followDistance, m_rigidbody2D.position.y);
                    if (Physics2D.Raycast(anotherNextPos, Vector2.down, m_contactFilter, hitBuffer, groundedRaycastDistance) > 0)
                    {
                        //修正y方向位置
                        //Debug.Log("地面"+hitBuffer[0].collider.gameObject.name);

                        Vector2 hitPos = hitBuffer[0].point;
                        Debug.DrawLine(anotherNextPos, hitPos, Color.red);
                        anotherNextPos.y = hitPos.y + m_centerHeight;
                        //设置人物行走动画与音乐
                        m_animator.SetBool("isWalking", true);
                        m_rigidbody2D.MovePosition(anotherNextPos);
                    }
                }
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

            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Quest"))
                forwardable = true;
        }

        /// <summary>
        /// 人物每次缩放后都需要重新调用此接口修正人物身高
        /// </summary>
        public void UpdateCenterHeight()
        {
            RaycastHit2D[] hitBuffer = new RaycastHit2D[1];
            if (Physics2D.Raycast(m_rigidbody2D.position, Vector2.down, m_contactFilter, hitBuffer, groundedRaycastDistance) > 0)
            {
                Vector2 hitPos = hitBuffer[0].point;
                m_centerHeight = (m_rigidbody2D.position - hitPos).y;
            }
            //女主高度初始化
            if (Physics2D.Raycast(m_girlRigidbody2D.position, Vector2.down, m_contactFilter, hitBuffer, groundedRaycastDistance) > 0)
            {
                Vector2 hitPos = hitBuffer[0].point;
                m_girlCenterHeight = (m_girlRigidbody2D.position - hitPos).y;
            }
        }

        /// <summary>
        /// 当PlayerController临时停用后恢复调用时，使用此接口重置位置
        /// </summary>
        public void SelfDestination()
        {
            m_destPos = m_rigidbody2D.position;
        }

        public void ChangeWalkigVolume(float volume)
        {
            m_audioSource.volume = volume;
        }

        public void PlayWalkingMusic()
        {
            m_audioSource.Play();
        }

        public void StopWalkingMusic()
        {
            m_audioSource.Stop();
        }

        //为true时为男主
        public void setWhoInControl(int who)
        {
            if (m_whoInControl != who)
                m_destPos = who == 0 ? m_rigidbody2D.position : m_girlRigidbody2D.position;
            m_whoInControl = who;
        }
    }
}