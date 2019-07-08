using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MiniGame
{

    public class PavilionScale : MonoBehaviour,QuestBehavior
    {
        private Touch pre1;
        private Touch pre2;
        private Vector3 m_originalScale;
        private SpriteRenderer m_sprite;
        public LayerMask questLayerMask;
        public float questRaycastDistance = 0.1f;
        private Vector2 m_originPos;//原来位置
        private Vector2 m_offset;//触摸位置与物体中心点的偏移
        private GameObject m_base;//关联机关
        

        //private float m_oldOffset;
        private float m_newOffset;
        public float destscale;
        public float origiscale;
        public float scalespeed;
        public float scaleOffset;
        public float xOffset;
        public float yOffset;
        public float destx;
        public float desty;
        public float alphaspeed;

        private SpriteRenderer m_door;
        private WaterTest m_waterTest;

        private bool next=true;//猫动画结束前不可以移动阁楼
        private bool m_isSuccess = false;//此处机关是否成功破解
        private bool m_lerpToRightPos = false;//松手后如果机关已经破解，逐渐把阁楼放到正确位置去

        private float m_originalpha;
        private float m_newalpha;
        private Collider2D m_collider2D;
        private Collider2D door_collider2D;
        private float m_collider2DHeight;
        private GameObject m_shield;

        //室内场景脱离阁楼外壳
        private GameObject m_stage1_5;
        private GameObject m_stage1_4;

        void Start()
        {
            m_collider2D = this.GetComponent<Collider2D>();
            m_collider2DHeight = (m_collider2D.bounds.max.y - m_collider2D.bounds.min.y) / 2;
            m_sprite = this.GetComponent<SpriteRenderer>();
            m_originPos = transform.position;
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);

            m_originalScale = this.transform.localScale;
            m_waterTest = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<WaterTest>();
            m_door = GameObject.Find("Door").GetComponent<SpriteRenderer>();
            m_base = GameObject.Find("Base");
            m_originalpha = m_sprite.color.a;
            door_collider2D = GameObject.Find("Door").GetComponent<Collider2D>();
            m_stage1_5 = GameObject.Find("Stage1-5");
            m_shield = GameObject.Find("Shield");

            this.enabled = false;
        }

        // Update is called once per frame
        
        void Update()
        {

            if (Input.GetMouseButton(0))
            {
                //描边
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero, questRaycastDistance, questLayerMask.value);
                //if (hit.collider != null)
                //{
                //    this.gameObject.layer = LayerMask.NameToLayer("Outline");
                //}
                //else
                //{
                //    this.gameObject.layer = LayerMask.NameToLayer("Quest");
                //}

            }

            ////Android端
            //if (!m_lerpToRightPos && Input.touchCount == 1)
            //{

            //    Touch touch = Input.touches[0];
            //    Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            //    //描边
            //    RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero, questRaycastDistance, questLayerMask.value);
            //    if (hit.collider != null)
            //    {
            //        this.gameObject.layer = LayerMask.NameToLayer("Outline");
            //    }
            //    else
            //    {
            //        this.gameObject.layer = LayerMask.NameToLayer("Quest");
            //    }





            //    if (touch.phase == TouchPhase.Began)
            //    {
            //        m_offset = new Vector2(transform.position.x, transform.position.y) - touchPos;
            //    }
            //    if (touch.phase == TouchPhase.Moved)
            //    {
            //        Vector2 currPos = touchPos + m_offset;
            //        if (currPos.y > m_base.transform.position.y)
            //        {
            //            transform.position = new Vector3(currPos.x, currPos.y, transform.position.z);
            //        }

            //        m_newOffset = m_originalScale.y + scalespeed * (destscale - m_originalScale.y) * Mathf.Abs((m_originPos.y - currPos.y)) / Mathf.Abs(((m_originPos.y - desty)));
            //        m_newalpha = m_originalpha + alphaspeed * (1 - m_originalpha) * (m_originPos.y - currPos.y) / (m_originPos.y - desty);
            //        Debug.Log("new alpha" + m_newalpha);
            //        Debug.Log("new scale" + m_newOffset);
            //        //Debug.Log(currPos.y);
            //        if (m_originalScale.y <= m_newOffset && m_newOffset <= destscale)
            //        {
            //            if (m_newalpha >= 1)
            //            {
            //                m_sprite.color = new Color(m_sprite.material.color.r, m_sprite.material.color.g, m_sprite.material.color.b, 1.0f);
            //                m_door.color = m_sprite.color;
            //            }
            //            else
            //            {
            //                m_sprite.color = new Color(m_sprite.material.color.r, m_sprite.material.color.g, m_sprite.material.color.b, m_newalpha);
            //                m_door.color = m_sprite.color;
            //            }
            //            if (m_newOffset >= destscale)
            //            {
            //                this.transform.localScale = new Vector3(destscale, destscale, 0);

            //            }
            //            else
            //            {
            //                this.transform.localScale = new Vector3(m_newOffset, m_newOffset, 0);
            //            }
            //        }
            //    }
            //    if (touch.phase == TouchPhase.Ended)
            //    {
            //          PlayerController.Instance.enabled = false;
            //        if (m_isSuccess)
            //        {
            //            m_lerpToRightPos = true;
            //            this.gameObject.layer = LayerMask.NameToLayer("Default");
            //            QuestController.Instance.UnRegisterQuest(gameObject.ToString());
            //        }
            //        else
            //        {
            //            Debug.Log("回去");
            //            //瞬时回到原来位置
            //            transform.position = new Vector3(m_originPos.x, m_originPos.y, transform.position.z);
            //            this.gameObject.layer = LayerMask.NameToLayer("Quest");
            //            this.transform.localScale = m_originalScale;
            //            m_sprite.color = new Color(m_sprite.material.color.r, m_sprite.material.color.g, m_sprite.material.color.b, m_originalpha);
            //            m_door.color = m_sprite.color;
            //            m_newOffset = 0;
            //            m_oldOffset = 0;
            //            //this.enabled = false;
            //            //过渡回到原来位置
            //        }
            //    }
            //}



            if (m_isSuccess&& m_lerpToRightPos)
            {
                Debug.Log("成功了");
                transform.position = Vector3.Lerp(transform.position, new Vector3(destx, desty, 0), Time.deltaTime * 5);
                this.transform.localScale = Vector3.Lerp(this.transform.localScale, new Vector3(destscale , destscale, 0), Time.deltaTime * 5);
                if (Mathf.Abs(this.transform.localScale.x-destscale)<scaleOffset && Mathf.Abs( transform.position .x-destx)<xOffset&& Mathf.Abs(transform.position.y - desty) < yOffset)
                {
                    transform.position = new Vector3(destx, desty, 0);
                    this.transform.localScale = new Vector3(destscale, destscale, 0);

                    
                    m_collider2D.enabled = false;
                    door_collider2D.enabled = true;
                    GameObject.Find("Door").GetComponent<OpenPavilion>().enabled= true;
                    GameObject.Find("Light").transform.GetChild(0).gameObject.SetActive(true);
                    GameObject.Destroy(m_shield);
                    //将第1-5画面脱离出来
                    m_stage1_5.transform.SetParent(transform.parent.parent);
                    m_stage1_5.transform.position = new Vector3(m_stage1_5.transform.position.x, m_stage1_5.transform.position.y, 10.0f);
                    this.enabled = false;

                }
                //if (destscale-this.transform.localScale.x <0.01)
                //{
                //transform.position = new Vector3(destx, desty, 0);
                //this.transform.localScale = new Vector3(destscale, destscale, 0);
                // this.enabled = false;
                //}
            }

            /*
            Vector3 scaleOffset = new Vector3(mouseScrollWheel, mouseScrollWheel, mouseScrollWheel);
            Vector3 currentScale = this.transform.localScale;


            if (scaleOffset.x + currentScale.x <= m_originalScale.x || scaleOffset.y + currentScale.y <= m_originalScale.y || scaleOffset.z + currentScale.z <= m_originalScale.z)
            {
                this.transform.localScale = m_originalScale;
                return;
            }
            else
            {
                this.transform.localScale += scaleOffset;
            }
            */
        }
        //public void setTrue()
        //{
        //    next = true;
        //}

        public void OnUpdate()
        {
            PlayerController.Instance.enabled = false;
            if (next)
            {
                this.enabled = true;
            }
        }

        private void OnMouseDown()
        {
            if (next && !m_lerpToRightPos && Input.GetMouseButton(0))
            {
                PlayerController.Instance.enabled = false;
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                m_offset = new Vector2(transform.position.x, transform.position.y) - touchPos;
            }
        }

        private void OnMouseDrag()
        {
            //Debug.Log("拖拽物体");
            if (next && !m_lerpToRightPos && Input.GetMouseButton(0))
            {

                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 currPos = touchPos + m_offset;

                if (currPos.y > m_base.transform.position.y)
                {
                    transform.position = new Vector3(currPos.x, currPos.y, transform.position.z);
                }

                m_newOffset = m_originalScale.y + scalespeed * (destscale - m_originalScale.y) * Mathf.Abs((m_originPos.y - currPos.y)) / Mathf.Abs(((m_originPos.y - desty)));
                m_newalpha = m_originalpha + alphaspeed * (1 - m_originalpha) * (m_originPos.y - currPos.y) / (m_originPos.y - desty);
                Debug.Log("new alpha" + m_newalpha);
                Debug.Log("new scale" + m_newOffset);
                //Debug.Log(currPos.y);
                if (m_originalScale.y <= m_newOffset && m_newOffset <= destscale)
                {
                    if (m_newalpha >= 1)
                    {
                        m_sprite.color = new Color(m_sprite.material.color.r, m_sprite.material.color.g, m_sprite.material.color.b, 1.0f);
                        m_door.color = m_sprite.color;
                    }
                    else
                    {
                        m_sprite.color = new Color(m_sprite.material.color.r, m_sprite.material.color.g, m_sprite.material.color.b, m_newalpha);
                        m_door.color = m_sprite.color;
                    }
                    if (m_newOffset >= destscale)
                    {
                        this.transform.localScale = new Vector3(destscale, destscale, 0);

                    }
                    else
                    {
                        this.transform.localScale = new Vector3(m_newOffset, m_newOffset, 0);
                    }
                }
            }
        }



        private void OnMouseUp()
        {
            //if (Input.GetMouseButton(0))
            //{
            //先做机关破解成功判断
            PlayerController.Instance.enabled = true;
            if (next && !m_lerpToRightPos)
            {

                if (m_isSuccess)
                {
                    m_lerpToRightPos = true;
                    GameObject.Destroy(m_shield);
                    QuestController.Instance.UnRegisterQuest(gameObject.ToString());
                }
                else
                {
                    Debug.Log("回去");
                    //瞬时回到原来位置
                    transform.position = new Vector3(m_originPos.x, m_originPos.y, transform.position.z);
                    this.gameObject.layer = LayerMask.NameToLayer("Quest");
                    this.transform.localScale = m_originalScale;
                    m_sprite.color = new Color(m_sprite.material.color.r, m_sprite.material.color.g, m_sprite.material.color.b, m_originalpha);
                    m_door.color = m_sprite.color;
                    m_newOffset = 0;
                    //m_oldOffset = 0;
                    //this.enabled = false;
                    //过渡回到原来位置
                }
            }

            //}
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //可以采取距离判断或者物体碰撞的方式判断机关是否已经破解。此处采用物体碰撞
            if (collision.gameObject.Equals(m_base))
            {
                Debug.Log("碰到地面了");
                m_waterTest.OnEnable();
                //可以让目的物体发白光
                m_isSuccess = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!m_lerpToRightPos && collision.gameObject.Equals(m_base))
            {
                Debug.Log("离开地面了");
                m_isSuccess = false;
            }
        }


        //Android
        /*
        void Update()
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
                    m_newOffset = (m_originPos.y - currPos.y) / 100;
                    Debug.Log(m_originPos.y - currPos.y);
                    Debug.Log(currPos.y);
                    if (m_newOffset != m_oldOffset && currPos.y < m_originPos.y)
                    {

                        if (m_newOffset > m_oldOffset)
                        {
                            if (this.transform.localScale.x < 0.6f)
                            {
                                Vector3 scaleOffset = new Vector3(m_newOffset, m_newOffset, 0);
                                this.transform.localScale += scaleOffset;
                                m_oldOffset = m_newOffset;
                            }
                        }
                        else if (m_newOffset < m_oldOffset)
                        {
                            if (this.transform.localScale.x > 0.05f)
                            {
                                Vector3 scaleOffset = new Vector3(m_oldOffset, m_oldOffset, 0);
                                this.transform.localScale -= scaleOffset;
                                m_oldOffset = m_newOffset;
                            }

                        }
                    }
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    if (m_isSuccess)
                    {
                        this.gameObject.layer = LayerMask.NameToLayer("Quest");
                        QuestController.Instance.UnRegisterQuest(gameObject.ToString());
                    }
                    else
                    {
                        瞬时回到原来位置
                        transform.position = new Vector3(m_originPos.x, m_originPos.y, transform.position.z);
                        this.gameObject.layer = LayerMask.NameToLayer("Quest");
                        this.transform.localScale = m_originalScale;
                        m_newOffset = 0;
                        m_oldOffset = 0;
                        this.enabled = false;
                        过渡回到原来位置
                    }
                }
            }
        }*/
    }

}