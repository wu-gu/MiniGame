using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour
{
    public float animSpeed;

    private Vector2 originPos;
    private Vector2 m_offset;


    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        Debug.Log("点击物体");
        //Android端
        if (Input.touchCount == 1)
        {
            Touch touch = Input.touches[0];
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (touch.phase == TouchPhase.Began)
            {
                m_offset = new Vector2(transform.position.x, transform.position.y) - touchPos;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 currPos = touchPos + m_offset;
                transform.position = new Vector3(currPos.x, currPos.y, transform.position.z);
            }
            if(touch.phase== TouchPhase.Ended)
            {
                //先做机关判断

                //瞬时回到原来位置
                transform.position = new Vector3(originPos.x, originPos.y, transform.position.z);
                //过渡回到原来位置
            }
        }

        //PC端
        if (Input.GetMouseButton(0))
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_offset = new Vector2(transform.position.x, transform.position.y) - touchPos;
        }
    }

    private void OnMouseDrag()
    {
        Debug.Log("拖拽物体");
        if (Input.GetMouseButton(0))
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 currPos = touchPos + m_offset;
            transform.position = new Vector3(currPos.x, currPos.y, transform.position.z);
        }
    }

    /// <summary>
    /// 在此处做机关触发判断
    /// </summary>
    private void OnMouseUp()
    {
        transform.position = new Vector3(originPos.x, originPos.y, transform.position.z);
    }
}
