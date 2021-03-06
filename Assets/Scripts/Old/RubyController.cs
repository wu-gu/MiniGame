﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{


    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class RubyController : MonoBehaviour
    {
        [Tooltip("The Layers which represent gameobjects that the Character Controller can be grounded on.")]
        public LayerMask groundedLayerMask;

        public float speed = 3.0f;//x方向移动速率
        public float groundedRaycastDistance = 50f;

        //private CharacterController m_characterController;

        private Rigidbody2D m_rigidbody2D;
        private CapsuleCollider2D m_capsule;
        private ContactFilter2D m_contactFilter;
        private Vector2 m_destPos;

        private void Start()
        {
            m_capsule = GetComponent<CapsuleCollider2D>();
            m_rigidbody2D = GetComponent<Rigidbody2D>();
            m_contactFilter.layerMask = groundedLayerMask;
            m_contactFilter.useLayerMask = true;
            m_contactFilter.useTriggers = false;

            m_destPos = m_rigidbody2D.position;
        }

        void Update()
        {
            //PC端鼠标左键点击
            if (Input.GetMouseButtonDown(0))
            {
                m_destPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            //Android端单击
            if (Input.touchCount == 1)
            {
                Touch touch = Input.touches[0];
                if (touch.phase == TouchPhase.Ended)
                {
                    m_destPos = touch.position;
                }
            }
        }

        void FixedUpdate()
        {

            //人物不设重力，手动控制人物与地形之间的关系
            Vector2 nextPos = Vector2.MoveTowards(m_rigidbody2D.position, m_destPos, speed * Time.deltaTime);
            RaycastHit2D[] hitBuffer = new RaycastHit2D[1];
            //地面检测
            if (Physics2D.Raycast(nextPos, Vector2.down, m_contactFilter, hitBuffer, groundedRaycastDistance)>0)
            {
                //修正y方向位置
                Debug.Log(hitBuffer[0].collider.gameObject.name);

                Vector2 hitPos = hitBuffer[0].point;
                Debug.DrawLine(nextPos, hitPos, Color.red);
                Debug.Log("hitPos:" + hitPos);
                nextPos.y = hitPos.y + m_capsule.size.y / 2;
                m_rigidbody2D.MovePosition(nextPos);
            }
        }
    }
}
