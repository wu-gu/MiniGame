using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MiniGame
{

    public class CameraScale : MonoBehaviour, QuestBehavior
    {
        private Camera m_camera;
        private float m_orthograp;
        public float m_dest;
        private Animator m_animator;
        private Touch pre1;
        private Touch pre2;


        // Start is called before the first frame update
        void Start()
        {
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
            m_camera = Camera.main;
            m_animator = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
            //m_camera = GetComponent<Camera>();
           // m_animator = GetComponent<Animator>();
            m_orthograp = m_camera.orthographicSize;
            m_animator.enabled = false;
            InputController.Instance.SetPlayerCanMove(false);
            this.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {

            float mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");

            float currentSize = m_camera.orthographicSize;

            if (mouseScrollWheel + currentSize <= m_orthograp)
            {
                m_camera.orthographicSize = m_orthograp;
                return;
            }
            else
            {
                if (mouseScrollWheel + currentSize >= m_dest)
                {
                    m_camera.orthographicSize = m_dest;
                    m_animator.enabled = true;

                }
                else
                    m_camera.orthographicSize = currentSize + mouseScrollWheel;
            }

            //if (Input.touchCount == 2)
            //{
            //    Touch curr1 = Input.GetTouch(0);
            //    Touch curr2 = Input.GetTouch(1);
            //    if (curr2.phase == TouchPhase.Began)
            //    {
            //        pre2 = curr2;
            //        pre1 = curr1;
            //        return;
            //    }

            //    // Process when the user input is clearly the act of scaling
            //    else
            //    {
            //        // Disable collider component when scale starts, or player will be pushed out 

            //        float preDist = Vector2.Distance(pre1.position, pre2.position);
            //        float currDist = Vector2.Distance(curr1.position, curr2.position);

            //        // Scale distance
            //        float offset = currDist - preDist;

            //        // Calculate scale offset and new scale vector per frame
            //        float scaleFactor = offset / 10000f;

            //        if (scaleFactor + currentSize <= m_orthograp)
            //        {
            //            m_camera.orthographicSize = m_orthograp;
            //            return;
            //        }
            //        else
            //        {
            //            if (scaleFactor + currentSize >= m_dest)
            //            {
            //                m_camera.orthographicSize = m_dest;
            //                m_animator.enabled = true;

            //            }
            //            else
            //                m_camera.orthographicSize = currentSize + scaleFactor;
            //        }

            //        pre1 = curr1;
            //        pre2 = curr2;

            //    }
            //}

            AnimatorStateInfo animatorInfo;
            animatorInfo = m_animator.GetCurrentAnimatorStateInfo(0);
            if (animatorInfo.normalizedTime > 1.0f&&animatorInfo.IsName("Stage2-1Scale"))
            {
                QuestController.Instance.UnRegisterQuest(gameObject.ToString());
                InputController.Instance.SetPlayerCanMove(true);
                Destroy(gameObject.GetComponent<Collider2D>());
                Destroy(this);
            }
        }

        public void OnUpdate()
        {
            this.enabled = true;
        }
    }

}