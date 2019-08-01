using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerQuest : MonoBehaviour,QuestBehavior
{

    public GameObject destGameobject;//关联机关

    /*平移基础操作所需变量*/
    private Vector2 m_originPos;//原来位置
    private Vector2 m_offset;//触摸位置与物体中心点的偏移
    private bool m_isSuccess = false;//此处机关是否成功破解

    public string destGameObjectName;

    public void OnUpdate()
    {
        this.enabled = true;
    }

    void Start()
    {
        m_originPos = transform.position;
        destGameobject = GameObject.Find(destGameObjectName);
        QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_offset = new Vector2(transform.position.x, transform.position.y) - touchPos;
        }
    }

    private void OnMouseDrag()
    {
        //Debug.Log("拖拽物体");
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
        if (m_isSuccess)
        {
            Debug.Log("机关成功");
            destGameobject.GetComponent<Animator>().enabled = true;
            GameObject.Find("Kites").GetComponent<KitesController>().AddSuccess();
            QuestController.Instance.UnRegisterQuest(gameObject.ToString());
            this.enabled = false;
        }
        else
        {
            //瞬时回到原来位置
            transform.position = new Vector3(m_originPos.x, m_originPos.y, transform.position.z);
            this.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //可以采取距离判断或者物体碰撞的方式判断机关是否已经破解。此处采用物体碰撞
        if (collision.gameObject.Equals(destGameobject))
        {
            Debug.Log("谜底碰到风筝了");
            //可以让目的物体发白光
            m_isSuccess = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(destGameobject))
        {
            Debug.Log("谜底离开风筝了");
            m_isSuccess = false;
        }
    }
}
