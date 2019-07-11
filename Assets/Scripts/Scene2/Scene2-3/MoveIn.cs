using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class MoveIn : MonoBehaviour
    {
        public float speed = 2.5f;
        private ParticleSystem m_particleSystem;
        private GameObject m_character;
        private SpriteRenderer m_renderer;
        private Rigidbody2D m_rigidbody2D;
        private Animator m_girlAnimator;
        private Animator m_animator;
 
        private Vector3 m_destPos;
        private int IsWalking;
        private int IsRightPos;


        // Start is called before the first frame update
        void Start()
        {
            GameObject girl = GameObject.Find("Girl");
            m_particleSystem = GetComponent<ParticleSystem>();
            m_character = GameObject.Find("Character");
            m_renderer = girl.GetComponent<SpriteRenderer>();
            m_rigidbody2D = girl.GetComponent<Rigidbody2D>();
            m_girlAnimator = girl.GetComponent<Animator>();
            m_animator = m_character.GetComponent<Animator>();            
            m_destPos = new Vector3(9.98f, -3.26f, 0f);
            IsWalking = Animator.StringToHash("isWalking");
            IsRightPos = Animator.StringToHash("IsRightPos");
            this.enabled = false;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (Mathf.Abs(m_rigidbody2D.position.x - m_destPos.x) < 0.01f)
            {
                m_girlAnimator.SetBool(IsWalking, false);
                m_destPos = m_rigidbody2D.position;
                GameObject.FindGameObjectWithTag("Girl").transform.SetParent(m_character.transform);
                m_renderer.flipX = true;
                m_animator.SetBool(IsRightPos, true);
                this.enabled = false;
            }
            //人物不设重力，手动控制人物与地形之间的关系
            Vector2 nextPos = Vector2.MoveTowards(m_rigidbody2D.position, m_destPos, speed * Time.deltaTime);
            m_girlAnimator.SetBool(IsWalking, true);
            m_rigidbody2D.MovePosition(nextPos);
        }

        private void OnMouseDown()
        {
            m_particleSystem.Stop();
            m_particleSystem.Clear();            
            InputController.Instance.SetPlayerCanMove(false);
            GameObject.FindGameObjectWithTag("Girl").GetComponent<PlayerController>().enabled = false;
            if (m_destPos.x < m_rigidbody2D.position.x)
                m_renderer.flipX = false;
            this.enabled = true;
        }
    }
}
