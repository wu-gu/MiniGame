using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudQuest : MonoBehaviour, QuestBehavior
{
    //业务变量

    public GameObject destGameobject;
    private bool m_isSuccess = false;

    //平移基础操作所需变量
    private Vector2 m_originPos;//原来位置
    private Vector2 m_offset;//触摸位置与物体中心点的偏移


    // Start is called before the first frame update
    void Start()
    {
        m_originPos = transform.position;
    }

    public void OnUpdate()
    {
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
            if (touch.phase == TouchPhase.Ended)
            {
                //先做机关破解成功判断
                if (m_isSuccess)
                {

                }
                else {
                    //瞬时回到原来位置
                    transform.position = new Vector3(m_originPos.x, m_originPos.y, transform.position.z);
                    //过渡回到原来位置
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //可以采取距离判断或者物体判断的方式判断机关是否已经破解。此处采用物体判断
        if (collision.gameObject.Equals(destGameobject))
        {
            //可以让目的物体发白光
            m_isSuccess = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(destGameobject))
        {
            m_isSuccess = false;
        }
    }

}
