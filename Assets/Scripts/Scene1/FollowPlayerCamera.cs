using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerCamera : MonoBehaviour
{
    public Transform destTransform;
    private Vector3 m_offset;
    private Vector3 m_nowPosition;
    private Vector3 m_oldPosition;
    // Start is called before the first frame update
    void Start()
    {
        m_offset = destTransform.position - this.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        m_oldPosition = this.transform.position;
        m_nowPosition = destTransform.position - m_offset;
        this.transform.position = new Vector3(m_nowPosition.x, m_oldPosition.y, m_oldPosition.z);
    }
}
