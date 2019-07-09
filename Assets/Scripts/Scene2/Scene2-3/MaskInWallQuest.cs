using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskInWallQuest : MonoBehaviour,QuestBehavior
{
    /*平移基础操作所需变量*/
    //private Vector2 m_originPos;//原来位置
    private Vector2 m_offset;//触摸位置与物体中心点的偏移

    private float min_y;
    private float max_y;

    private bool m_canMove = false;
    private bool m_isSuccess = false;
    private SpriteRenderer m_sculpture;
    private Animator m_animator;
    private CameraController m_cameraController;
    private int IsSwitchPoint;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject pattern = GameObject.Find("Paint");
        //float maskHeight = GetComponent<SpriteMask>().bounds.max.y - GetComponent<SpriteMask>().bounds.min.y;
        ////Debug.Log("maskHeight:" + maskHeight);
        //min_y = pattern.GetComponent<SpriteRenderer>().bounds.min.y + maskHeight / 2;
        ////Debug.Log("min_y" + min_y);
        //max_y = pattern.GetComponent<SpriteRenderer>().bounds.max.y - maskHeight / 2;
        ////Debug.Log("max_y" + max_y);
        ////m_originPos = transform.position;
        //QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
        //this.enabled = false;
        
        float maskHeight = GetComponent<SpriteMask>().bounds.max.y - GetComponent<SpriteMask>().bounds.min.y;
        max_y = GameObject.Find("UpperBound").GetComponent<EdgeCollider2D>().bounds.center.y - maskHeight / 2;
        min_y = GameObject.Find("LowerBound").GetComponent<EdgeCollider2D>().bounds.center.y + maskHeight / 2;

        //Debug.Log("maskHeight" + maskHeight);
        //Debug.Log("max_y" + max_y);
        //Debug.Log("min_y" + min_y);
        m_sculpture = GameObject.Find("Sculpture").GetComponent<SpriteRenderer>();
        GameObject mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        m_animator = mainCam.GetComponent<Animator>();
        m_cameraController = mainCam.GetComponent<CameraController>();
        IsSwitchPoint = Animator.StringToHash("IsSwitchPoint");
       
        QuestController.Instance.RegisterQuest(this.gameObject.ToString(), this);
        this.enabled = false;

    }

    public void OnUpdate()
    { 
        this.enabled = true;

        // Android -- began
        //if (Input.touchCount == 1)
        //{
        //    Touch touch = Input.touches[0];
        //    Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    m_offset = new Vector2(transform.position.x, transform.position.y) - touchPos;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_canMove)
        {
            Color oriColor = m_sculpture.color;
            m_sculpture.color = new Color(oriColor.r, oriColor.g, oriColor.b, oriColor.a - 0.005f);
            if (m_sculpture.color.a <= 0f)
            {
                SetCanMove();
                Destroy(GameObject.Find("Sculpture"));
            }
        }

        //if(m_canMove&&!m_isSuccess)
        //{
        //    if (Input.touchCount == 1)
        //    {
        //        Touch touch = Input.touches[0];
        //        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
        //        if (touch.phase == TouchPhase.Moved)
        //        {
        //            Vector2 currPos = touchPos + m_offset;
        //            float newY = Mathf.Clamp(currPos.y, min_y, max_y);
        //            //当Mask移动到最下面的时候，机关触发成功
        //            if (newY == min_y)
        //            {
        //                m_isSuccess = true;
        //                m_canMove = false;
        //            }

        //            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        //        }
        //        if (touch.phase == TouchPhase.Ended)
        //        {
        //            if (m_isSuccess)
        //            {
        //                Debug.Log("萧墙机关成功");
        //                QuestController.Instance.UnRegisterQuest(gameObject.ToString());
        //                this.enabled = false;
        //            }
        //            else
        //            {
        //                Debug.Log("萧墙未成功");
        //                this.enabled = false;
        //            }
        //        }
        //    }
        //}
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
        if (m_canMove && !m_isSuccess)
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 currPos = touchPos + m_offset;
                float newY = Mathf.Clamp(currPos.y, min_y, max_y);
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }
        }
        //Debug.Log("拖拽物体");
    }

    /// <summary>
    /// 在此处做机关触发判断
    /// </summary>
    private void OnMouseUp()
    {
        //if (Input.GetMouseButton(0))
        //{
        //先做机关破解成功判断
        if (!m_isSuccess && m_canMove)
        {
            Debug.Log("currPosY" + transform.position.y);
            Debug.Log("minY" + min_y);
            //当Mask移动到最下面的时候，机关触发成功
            if (Mathf.Abs(transform.position.y - min_y) < 0.0001f)
            {
                m_isSuccess = true;
                m_canMove = false;
            }
            if (m_isSuccess)
            {
                Debug.Log("萧墙机关成功");
                GameObject hint = GameObject.Find("Hint");
                hint.GetComponent<ParticleSystem>().Play();
                hint.GetComponent<Collider2D>().enabled = true;
                QuestController.Instance.UnRegisterQuest(gameObject.ToString());
                m_cameraController.enabled = false;
                m_animator.enabled = true;
                m_animator.SetBool(IsSwitchPoint, true);
                this.enabled = false;
            }
            else
            {
                this.enabled = false;
            }
        }
        
    }

    private void SetCanMove()
    {
        m_canMove = true;
    }
}
