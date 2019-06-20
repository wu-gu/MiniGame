using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Ecin
{
    // Only player simple(horizontal) movement is contained in this script
    public class PlayerMovement : MonoBehaviour
    {
        public LayerMask groundedLayerMask;
        public float playerSpeed;
        public float groundedRaycastDistance;
        private Vector2 oriPos;
        private Vector2 dest;
        private Rigidbody2D m_rigidbody;
        private CapsuleCollider2D player;
        private ContactFilter2D m_contactFilter;

        // Only set once when awake
        void Awake()
        {
            playerSpeed = 4f;
            groundedRaycastDistance = 10f;
            m_rigidbody = this.GetComponent<Rigidbody2D>();
            oriPos = m_rigidbody.position;
            player = GetComponent<CapsuleCollider2D>();
            m_contactFilter.layerMask = groundedLayerMask;
            m_contactFilter.useLayerMask = true;
            m_contactFilter.useTriggers = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void FixedUpdate()
        {
            Vector2 nextPos = Vector2.MoveTowards(oriPos, dest, playerSpeed * Time.deltaTime);

            RaycastHit2D[] hitBuffer = new RaycastHit2D[1];
            if (Physics2D.Raycast(nextPos, Vector2.down, m_contactFilter, hitBuffer, groundedRaycastDistance) > 0)
            {
                Vector2 hitPos = hitBuffer[0].point;
                nextPos.y = hitPos.y + player.bounds.extents.y + 0.2f;
                m_rigidbody.MovePosition(nextPos);
            }

            oriPos = m_rigidbody.position;

            if (oriPos.x == dest.x)
                this.enabled = false;
        }

        public void MoveTo(Vector2 dest)
        {
            this.dest = dest;
            this.enabled = true;
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Quest"))
            {
                Debug.Log("blocked");
                dest = m_rigidbody.position;
            }
        }
    }
}
