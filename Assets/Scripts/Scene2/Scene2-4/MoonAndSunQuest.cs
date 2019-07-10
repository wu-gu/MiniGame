using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class MoonAndSunQuest : MonoBehaviour, QuestBehavior
    {
        //业务变量
        /// <summary>
        /// 机关音效
        /// </summary>
        public AudioClip m_moonAndSunAudioClip;
        /// <summary>
        /// 缩放业务变量
        /// </summary>
        private Vector2 m_oldVector;
        private Quaternion m_originRotation;
        private GameObject m_mainCam;
        private GameObject m_flyingMagpie;
        private Collider2D m_collider;
        //日月跟随 -- 改为不跟（未定）

        private float m_angleCounter;
        /// <summary>
        /// 注册机关
        /// 初始化比例系数
        /// </summary>
        void Start()
        {
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
            m_originRotation = transform.localRotation;
            m_mainCam = GameObject.FindGameObjectWithTag("MainCamera");
            m_flyingMagpie = GameObject.Find("FlyingMagpie");
            m_collider = this.GetComponent<Collider2D>();
            m_angleCounter = 0f;
            this.enabled = false;
        }

        // Android enabled
        void Update()
        {
            // Android --stable
            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);
                Vector2 touchPos1, touchPos2;
                touchPos1 = Camera.main.ScreenToWorldPoint(touch1.position);
                touchPos2 = Camera.main.ScreenToWorldPoint(touch2.position);

                if (touch2.phase == TouchPhase.Began)
                {
                    Debug.Log("Touch2 began");
                    m_oldVector = touchPos1 - touchPos2;
                    return;
                }
                if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
                {
                    Debug.Log("Moving");
                    Vector2 newVector = touch1.position - touch2.position;
                    Vector3 preDirectionVec3 = new Vector3(m_oldVector.x, m_oldVector.y, transform.position.z);
                    Vector3 currDirectionVec3 = new Vector3(newVector.x, newVector.y, transform.position.z);

                    float angle = Vector3.Angle(preDirectionVec3, currDirectionVec3);
                    Vector3 normal = Vector3.Cross(preDirectionVec3, currDirectionVec3);
                    angle *= Mathf.Sign(Vector3.Dot(normal, transform.forward));
                    m_angleCounter += angle;
                    transform.Rotate(new Vector3(0, 0, angle));
                    m_oldVector = newVector;

                    if (Mathf.Abs(m_angleCounter) >= 180f)
                    {
                        this.transform.localRotation = new Quaternion(0, 0, 180f, 0);
                        gameObject.GetComponent<Collider2D>().enabled = false;
                        AudioController.Instance.PushClip(m_moonAndSunAudioClip);
                        m_mainCam.GetComponent<CameraController>().enabled = false;
                        m_mainCam.GetComponent<CameraNaturalTransition>().NaturalTransition1();
                        m_flyingMagpie.GetComponent<Animator>().enabled = true;
                        QuestController.Instance.UnRegisterQuest(gameObject.ToString());
                        this.enabled = false;
                    }
                }                
                if (touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended)
                {
                    if (Mathf.Abs(m_angleCounter) < 180f)
                        SlidBackOriPos();

                }               
            }
        }
       
        public void OnUpdate()
        {
            // Android -- stable
            //Debug.Log("Android");
            //this.enabled = true;
            //Touch touch1 = Input.GetTouch(0);
            //if (Input.touchCount == 2)
            //{
            //    Debug.Log("Two touches at the same time");
            //    Touch touch2 = Input.GetTouch(1);
            //    Vector2 touchPos1, touchPos2;
            //    touchPos1 = Camera.main.ScreenToWorldPoint(touch1.position);
            //    touchPos2 = Camera.main.ScreenToWorldPoint(touch2.position);

            //    m_oldVector = touchPos1 - touchPos2;
            //}

            // PC-- Directly controlled here
            //Debug.Log("PC");
            this.transform.localRotation = new Quaternion(0, 0, 180f, 0);
            m_mainCam.GetComponent<CameraController>().enabled = false;
            m_mainCam.GetComponent<CameraNaturalTransition>().NaturalTransition1();
            m_flyingMagpie.GetComponent<Animator>().enabled = true;
            gameObject.GetComponent<Collider2D>().enabled = false;
            AudioController.Instance.PushClip(m_moonAndSunAudioClip);
            QuestController.Instance.UnRegisterQuest(gameObject.ToString());
        }

        private void SlidBackOriPos()
        {
            transform.localRotation = m_originRotation;
            m_angleCounter = 0f;
            this.enabled = false;
        }

        // Pre-stable version
        //public void OnUpdate()
        //{
        //    //Android --stable
        //    //if (Input.touchCount > 0)
        //    //{
        //    //    Touch touch1 = Input.GetTouch(0);
        //    //    Vector2 touchPos1, touchPos2;
        //    //    touchPos1 = Camera.main.ScreenToWorldPoint(touch1.position);

        //    //    if (!m_collider.bounds.Contains(touchPos1))
        //    //    {
        //    //        Debug.Log("Not on quest");
        //    //        return;
        //    //    }


        //    //    if (Input.touchCount == 2)
        //    //    {
        //    //        Touch touch2 = Input.GetTouch(1);
        //    //        touchPos2 = Camera.main.ScreenToWorldPoint(touch2.position);

        //    //        //第二个接触点落下，计算初始两触点距离
        //    //        if (touch2.phase == TouchPhase.Began)
        //    //        {
        //    //            Debug.Log("Began phase enter");
        //    //            m_oldVector = touchPos1 - touchPos2;
        //    //            return;
        //    //        }

        //    //        if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
        //    //        {

        //    //            Debug.Log("Moving");
        //    //            Vector2 newVector = touch1.position - touch2.position;
        //    //            Vector3 preDirectionVec3 = new Vector3(m_oldVector.x, m_oldVector.y, transform.position.z);
        //    //            Vector3 currDirectionVec3 = new Vector3(newVector.x, newVector.y, transform.position.z);

        //    //            float angle = Vector3.Angle(preDirectionVec3, currDirectionVec3);
        //    //            Vector3 normal = Vector3.Cross(preDirectionVec3, currDirectionVec3);
        //    //            angle *= Mathf.Sign(Vector3.Dot(normal, transform.forward));
        //    //            m_angleCounter += angle;
        //    //            transform.Rotate(new Vector3(0, 0, angle));
        //    //            m_oldVector = newVector;
        //    //        }

        //    //        //机关触发判断，如果不触发，恢复原来位置
        //    //        if (touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended)
        //    //        {
        //    //            //float originScale = m_oldDistance / newDistance * scaleUnit;
        //    //            //transform.localScale.Scale(new Vector3(originScale, originScale, 1));
        //    //            //立刻恢复
        //    //            //transform.localRotation = m_originRotation;
        //    //            //缓缓恢复
        //    //            if (Mathf.Abs(m_angleCounter) < 180f)
        //    //                m_isSlidBack = true;


        //    //        }

        //    //    }
        //    //}

        //    //if (Mathf.Abs(m_angleCounter) >= 180f)
        //    //{
        //    //    this.transform.localRotation = new Quaternion(0, 0, 180f, 0);
        //    //    gameObject.GetComponent<Collider2D>().enabled = false;
        //    //    AudioController.Instance.PushClip(m_moonAndSunAudioClip);
        //    //    m_mainCamAnimator.GetComponent<Animator>().enabled = true;
        //    //    m_flyingMagpie.GetComponent<Animator>().enabled = true;
        //    //    //注销机关
        //    //    QuestController.Instance.UnRegisterQuest(gameObject.ToString());
        //    //}

        //    //PC端
        //    if (Input.GetMouseButtonDown(0))
        //    {

        //        Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //        Debug.Log("点击!!!" + mousePoint);
        //        Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;
        //        Debug.Log("点击!!!" + mousePoint + bounds.Contains(mousePoint));
        //        if (bounds.Contains(mousePoint))
        //        {
        //            Debug.Log("点击到位，触发事件");
        //            //成功动画触发
        //            transform.localEulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
        //            Debug.Log("相机名字" + m_mainCamAnimator.name);
        //            m_mainCamAnimator.GetComponent<Animator>().enabled = true;
        //            m_flyingMagpie.GetComponent<Animator>().enabled = true;
        //            Debug.Log("相机动画" + m_mainCamAnimator.GetComponent<Animator>().name);

        //            gameObject.GetComponent<Collider2D>().enabled = false;
        //            AudioController.Instance.PushClip(m_moonAndSunAudioClip);
        //            //注销机关
        //            QuestController.Instance.UnRegisterQuest(gameObject.ToString());
        //        }
        //    }
        //}

        //void Update()
        //{
        //    //OnUpdate放置在Update中会导致在屏幕任何一处点击都会触发事件，移动端待检验
        //    OnUpdate();
        //    if (m_isSlidBack)
        //    {
        //        SlidBackOriPos();
        //    }
        //}
    }
}

