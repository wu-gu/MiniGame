using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{

}

public class RubyController : MonoBehaviour
{
    private Vector2 m_destPos;
    public float speed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_destPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //PC端鼠标左键点击
        if(Input.GetMouseButtonDown(0))
        {
            m_destPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        //Android端单击
        if(Input.touchCount == 1)
        {
            Touch touch = Input.touches[0];
            if(touch.phase == TouchPhase.Ended)
            {
                m_destPos = touch.position;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, m_destPos, speed * Time.deltaTime);

    }
}
