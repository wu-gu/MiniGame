using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class BridgeQuest : MonoBehaviour, QuestBehavior
    {
        //业务变量
        private bool m_isSuccess = false;
        /// <summary>
        /// 机关音效
        /// </summary>
        public AudioClip m_moonAndSunAudioClip;
        /// <summary>
        /// 缩放业务变量
        /// </summary>
        public float rotationUnit = 0.3f;
        private Vector2 m_oldVector;
        private Quaternion m_originRotation;
        private int m_clockwise = -1;
        public Vector3 threshold = new Vector3(3.0f, 3.0f, 1.0f);

        private GameObject m_mainCamera;
        private GameObject m_flyingMagpie;

        private GameObject m_magpiesBridge;

        /// <summary>
        /// 注册机关
        /// </summary>
        void Awake()
        {
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
        }

        /// <summary>
        /// 初始化比例系数
        /// </summary>
        void Start()
        {
            m_originRotation = transform.localRotation;
            m_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            m_flyingMagpie = GameObject.Find("FlyingMagpie");
            m_magpiesBridge = GameObject.Find("BuildingBridgeMagpies");
        }

        public void OnUpdate()
        {

            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                //第二个接触点落下，计算初始两触点距离
                if (touch2.phase == TouchPhase.Began)
                {
                    m_oldVector = new Vector2(touch1.position.x - touch2.position.x, touch1.position.y - touch2.position.y);
                    return;
                }

                if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
                {
                    Vector2 newVector = new Vector2(touch1.position.x - touch2.position.x, touch1.position.y - touch2.position.y);
                    //计算缩放比例，并更新物体缩放系数
                    float newRotation = Vector2.Dot(newVector, m_oldVector) / (Vector2.SqrMagnitude(newVector) * Vector2.SqrMagnitude(m_oldVector));
                    if (newVector.x - m_oldVector.x > 0)
                    {
                        m_clockwise = 1;
                    }
                    else
                    {
                        m_clockwise = -1;
                    }
                    transform.eulerAngles = new Vector3(m_clockwise * newRotation, m_clockwise * newRotation, 0.0f);
                    //transform.Rotate(m_clockwise * newRotation, m_clockwise * newRotation, 0.0f);
                    m_oldVector = newVector;
                }

                //机关触发判断，如果不触发，恢复原来大小
                if (touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended)
                {
                    //float originScale = m_oldDistance / newDistance * scaleUnit;
                    //transform.localScale.Scale(new Vector3(originScale, originScale, 1));
                    transform.localRotation = m_originRotation;
                }
            }
            else
            {
                //PC端
                if (Input.GetMouseButtonDown(0))
                {
                    //成功动画触发
                    transform.localEulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
                    Debug.Log("相机名字" + m_mainCamera.name);
                    m_mainCamera.GetComponent<Animator>().enabled = true;
                    m_flyingMagpie.GetComponent<Animator>().enabled = true;
                    Debug.Log("相机动画" + m_mainCamera.GetComponent<Animator>().name);

                    gameObject.GetComponent<Collider2D>().enabled = false;
                    AudioController.Instance.PushClip(m_moonAndSunAudioClip);
                    //注销机关
                    QuestController.Instance.UnRegisterQuest(gameObject.ToString());
                    m_magpiesBridge.GetComponent<Collider2D>().enabled = true;
                }
            }
        }

        void Update()
        {
            OnUpdate();
            if (transform.localRotation.x > threshold.x && transform.localRotation.y > threshold.y)
            {
                m_isSuccess = true;
                gameObject.GetComponent<Collider2D>().enabled = false;
                AudioController.Instance.PushClip(m_moonAndSunAudioClip);
                //注销机关
                QuestController.Instance.UnRegisterQuest(gameObject.ToString());
            }
        }
    }
}
