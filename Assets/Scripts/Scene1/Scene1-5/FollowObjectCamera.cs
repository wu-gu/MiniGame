using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class FollowObjectCamera : MonoBehaviour
    {
        private GameObject m_destTransform;
        private Vector3 m_offset;
        private Vector3 m_nowPosition;
        private Vector3 m_oldPosition;
        private float minY = 0.0f;
        private float maxY = 16.0f;
        private float m_speed = 0.1f;
        // Start is called before the first frame update
        void Start()
        {
            m_destTransform = GameObject.Find("Plate");
            m_offset = m_destTransform.transform.position - this.transform.position;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            m_oldPosition = this.transform.position;
            m_nowPosition = m_destTransform.transform.position - m_offset;
            m_nowPosition.y = Mathf.Clamp(m_destTransform.transform.position.y, minY, maxY);
            m_nowPosition.y = Mathf.MoveTowards(transform.position.y, m_nowPosition.y, m_speed);
            this.transform.position = new Vector3(m_oldPosition.x, m_nowPosition.y, m_oldPosition.z);
        }
    }
}

