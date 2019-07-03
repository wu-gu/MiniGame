using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MiniGame
{

    public class camerascale : MonoBehaviour, QuestBehavior
    {
        private Camera m_camera;
        private float m_orthograp;
        private float m_dest = 0.75756f;
        private Animator m_animator;



        public void OnUpdate()
        {
            this.enabled = true;
        }

        // Start is called before the first frame update
        void Awake()
        {

            m_camera = this.GetComponent<Camera>();
            m_animator = GetComponent<Animator>();
            m_orthograp = m_camera.orthographicSize;
            m_animator.enabled = false;
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
            AnimatorStateInfo animatorInfo;
            animatorInfo = m_animator.GetCurrentAnimatorStateInfo(0);
            if (animatorInfo.normalizedTime > 1.0f)
            {
                Destroy(gameObject);
            }
        }
    }

}