using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private Vector2 m_firstDirection;
    private Vector2 m_preDirection;
    public float animSpeed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            //m_firstDirection = transform.ro
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_preDirection = m_firstDirection= touchPos - (Vector2)(transform.position);
        }

        if (Input.touchCount == 1)
        {
            //m_firstDirection = transform.ro
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
            m_preDirection = m_firstDirection = touchPos - (Vector2)(transform.position);
        }
    }

    private void OnMouseDrag()
    {
        
        bool canUsed = false;
        Vector2 touchPos = (Vector2)(transform.position);
        //PC端输入
        if (Input.GetMouseButton(0))
        {
            touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            canUsed = true;
        }
        //Android端输入
        if (Input.touchCount == 1)
        {
            touchPos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
            canUsed = true;
        }

        if(canUsed)
        {
            Vector2 currDirection = touchPos - (Vector2)(transform.position);

            Vector3 preDirectionVec3 = new Vector3(m_preDirection.x, m_preDirection.y, transform.position.z).normalized;
            Vector3 currDirectionVec3 = new Vector3(currDirection.x, currDirection.y, transform.position.z).normalized;

            float angle = Vector3.Angle(preDirectionVec3, currDirectionVec3);
            Vector3 normal = Vector3.Cross(preDirectionVec3, currDirectionVec3);
            //计算顺时针还是逆时针
            angle *= Mathf.Sign(Vector3.Dot(normal, transform.forward));
            transform.Rotate(new Vector3(0, 0, angle));
            m_preDirection = currDirection;
        }
    }

    /// <summary>
    /// PC端与Android端通用
    /// </summary>
    private void OnMouseUp()
    {
        Vector3 firstDirectionVec3 = new Vector3(m_firstDirection.x, m_firstDirection.y, transform.position.z).normalized;
        Vector3 lastDirectionVec3 = new Vector3(m_preDirection.x, m_preDirection.y, transform.position.z).normalized;

        float angle = Vector3.Angle(lastDirectionVec3, firstDirectionVec3);
        Vector3 normal = Vector3.Cross(lastDirectionVec3, firstDirectionVec3);
        //计算顺时针还是逆时针
        angle *= Mathf.Sign(Vector3.Dot(normal, transform.forward));
        StartCoroutine("RotateToOrigin", angle);
        //Debug.Log(angle);
        //transform.Rotate(new Vector3(0, 0, angle));
    }

    IEnumerator RotateToOrigin(float angle)
    {
        float sign = Mathf.Sign(angle);
        angle = Mathf.Abs(angle);
        while (angle - animSpeed > 0)
        {
            transform.Rotate(new Vector3(0, 0, sign*animSpeed));
            angle -=animSpeed;
            yield return 0;
        }
        StopCoroutine("RotateToOrigin");
    }
}
