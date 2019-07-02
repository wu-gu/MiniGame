using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudQuest : MonoBehaviour, QuestBehavior
{
    /*业务变量*/

    public GameObject destGameobject;//关联机关，此处为树枝
    private bool m_isSuccess = false;//此处机关是否成功破解
    private Animator m_animator;

    /*平移基础操作所需变量*/
    private Vector2 m_originPos;//原来位置
    private Vector2 m_offset;//触摸位置与物体中心点的偏移


    void Awake()
    {
        m_originPos = transform.position;
        m_animator = GetComponent<Animator>();

    }

    /// <summary>
    /// 该脚本就算一开始被开启，Start也会在所有同步加载的Gameobjects都创建并调用Awake后调用
    /// </summary>
    void Start()
    {
        destGameobject = GameObject.Find("Branch");
        QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
        this.enabled = false;
    }


    public void OnUpdate()
    {
        this.enabled = true;
    }

    void Update()
    {

        if (Input.touchCount == 1)
        {
            

            Touch touch = Input.touches[0];
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (touch.phase == TouchPhase.Began)
            {
                m_animator.enabled = false;
                m_offset = new Vector2(transform.position.x, transform.position.y) - touchPos;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 currPos = touchPos + m_offset;
                transform.position = new Vector3(currPos.x, currPos.y, transform.position.z);
            }
            if (touch.phase == TouchPhase.Ended)
            {
                if (m_isSuccess)
                {
                    Debug.Log("云朵机关成功");
                    destGameobject.GetComponent<Animator>().SetBool("shouldHide", true);
                    m_animator.enabled = true;
                    m_animator.SetBool("shouldHide", true);
                    QuestController.Instance.UnRegisterQuest(gameObject.ToString());
                    this.enabled = false;
                }
                else
                {
                    Debug.Log("云朵机关未成功");
                    //瞬时回到原来位置
                    transform.position = new Vector3(m_originPos.x, m_originPos.y, transform.position.z);
                    m_animator.enabled = true;
                    this.enabled = false;
                    //过渡回到原来位置
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            m_animator.enabled = false;
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
        //if (Input.GetMouseButton(0))
        //{
            //先做机关破解成功判断
        if (m_isSuccess)
            {
                Debug.Log("机关成功");
                destGameobject.GetComponent<Animator>().SetBool("shouldHide", true);
                m_animator.enabled = true;
                m_animator.SetBool("shouldHide", true);
                QuestController.Instance.UnRegisterQuest(gameObject.ToString());
                this.enabled = false;
        }
            else
            {
                //瞬时回到原来位置
                transform.position = new Vector3(m_originPos.x, m_originPos.y, transform.position.z);
                m_animator.enabled = true;
                this.enabled = false;
                //过渡回到原来位置
            }
        //}
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //可以采取距离判断或者物体碰撞的方式判断机关是否已经破解。此处采用物体碰撞
        if (collision.gameObject.Equals(destGameobject))
        {
            Debug.Log("云碰到树枝了");
            //可以让目的物体发白光
            m_isSuccess = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(destGameobject))
        {
            Debug.Log("云离开树枝了");
            m_isSuccess = false;
        }
    }

}
