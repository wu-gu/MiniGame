using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour
{
    public float scaleUnit = 1.0f;
    private float m_oldDistance;
    private Vector2 m_originScale;

    // Start is called before the old frame update
    void Start()
    {
        m_originScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel")!=0)
        {

        }
        

        //Android端
        if(Input.touchCount==2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch2.phase == TouchPhase.Began)
            {
                m_oldDistance = Vector2.Distance(touch1.position,touch2.position);
                return;
            }

            if(touch1.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved)
            {
                float newDistance = Vector2.Distance(touch1.position, touch2.position);

                float newScale = (newDistance - m_oldDistance) * scaleUnit;
                transform.localScale += new Vector3(newScale, newScale, 1);
                m_oldDistance = newDistance;
            }


            //机关触发判断，如果不触发，恢复原来大小
            if (touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended)
            {
                //float originScale = m_oldDistance / newDistance * scaleUnit;
                //transform.localScale.Scale(new Vector3(originScale, originScale, 1));
                transform.localScale = m_originScale;
            }
        }
    }

    private void OnMouseDown()
    {
        //Android端
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch2.phase == TouchPhase.Began)
            {
                m_oldDistance = Vector2.Distance(touch1.position, touch2.position);
                return;
            }
        }
    }

    private void OnMouseDrag()
    {
        //Android端
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved)
            {
                float newDistance = Vector2.Distance(touch1.position, touch2.position);

                float newScale = (newDistance - m_oldDistance) * scaleUnit;
                transform.localScale += new Vector3(newScale, newScale, 1);
                m_oldDistance = newDistance;
            }
        }
    }

    private void OnMouseUp()
    {
        //Android端
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            //机关触发判断，如果不触发，恢复原来大小
            if (touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended)
            {
                //float originScale = m_oldDistance / newDistance * scaleUnit;
                //transform.localScale.Scale(new Vector3(originScale, originScale, 1));
                transform.localScale = m_originScale;
            }
        }
    }


}
