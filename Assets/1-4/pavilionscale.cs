using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MiniGame
{

    public class pavilionscale : MonoBehaviour,QuestBehavior
    {
        private Touch pre1;
        private Touch pre2;
        private Vector3 originalScale;
        private Renderer windowRenderer;
        public LayerMask questLayerMask;
        public float questRaycastDistance = 0.1f;
        private Vector2 m_originPos;//原来位置
        private Vector2 m_offset;//触摸位置与物体中心点的偏移
        public GameObject destGameobject;//关联机关，此处为树枝
        private bool m_isSuccess = false;//此处机关是否成功破解
        private float OLDOFFSET;
        private float NEWOFFSET;
        public float destscale;
        public float destx;
        public float desty;
        Watertest mytest;
        private bool next=false;



        // Start is called before the first frame update
        void Awake()
        {
            m_originPos = transform.position;
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
            windowRenderer = this.GetComponent<Renderer>();
            originalScale = this.transform.localScale;
            mytest = GameObject.Find("Main Camera").GetComponent<Watertest>();

        }

        // Update is called once per frame
        
        void Update()
        {
            
            if (Input.GetMouseButton(0))
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero, questRaycastDistance, questLayerMask.value);
                if (hit.collider != null)
                {
                    this.gameObject.layer = LayerMask.NameToLayer("Outline");
                }
                else
                {
                    this.gameObject.layer = LayerMask.NameToLayer("Quest");
                }

            }
            if (m_isSuccess)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(destx, desty, 0), Time.deltaTime*5);
                this.transform.localScale = Vector3.Lerp(this.transform.localScale, new Vector3(destscale, destscale, destscale), Time.deltaTime*5);
                if (transform.position == new Vector3(destx, desty, 0) && this.transform.localScale == new Vector3(destscale, destscale, destscale))
                {
                    this.enabled = false;
                }
            }

            /*
            Vector3 scaleOffset = new Vector3(mouseScrollWheel, mouseScrollWheel, mouseScrollWheel);
            Vector3 currentScale = this.transform.localScale;


            if (scaleOffset.x + currentScale.x <= originalScale.x || scaleOffset.y + currentScale.y <= originalScale.y || scaleOffset.z + currentScale.z <= originalScale.z)
            {
                this.transform.localScale = originalScale;
                return;
            }
            else
            {
                this.transform.localScale += scaleOffset;
            }
            */
        }
        public void settrue()
        {
            next = true;
        }

        public void OnUpdate()
        {
            if (next == true)
            {
                this.enabled = true;
            }
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
                NEWOFFSET = (m_originPos.y - currPos.y)/100;
                //Debug.Log(m_originPos.y - currPos.y);
                //Debug.Log(currPos.y);
                if (NEWOFFSET != OLDOFFSET&& currPos.y< m_originPos.y)
                {
                    
                    if (NEWOFFSET > OLDOFFSET)
                    {
                        if (this.transform.localScale.x < 0.6f)
                        {
                            Vector3 scaleOffset = new Vector3(NEWOFFSET, NEWOFFSET, NEWOFFSET);
                            this.transform.localScale += scaleOffset;
                            OLDOFFSET = NEWOFFSET;
                        }
                    }
                    else if(NEWOFFSET<OLDOFFSET)
                    {
                        if (this.transform.localScale.x > 0.05f)
                        {
                            Vector3 scaleOffset = new Vector3(OLDOFFSET, OLDOFFSET, OLDOFFSET);
                            this.transform.localScale -= scaleOffset;
                            OLDOFFSET = NEWOFFSET;
                        }

                    }
                }

                

            }
        }

 

        private void OnMouseUp()
        {
            //if (Input.GetMouseButton(0))
            //{
            //先做机关破解成功判断
            if (m_isSuccess)
            {

                this.gameObject.layer = LayerMask.NameToLayer("Quest");
                QuestController.Instance.UnRegisterQuest(gameObject.ToString());


            }
            else
            {
                //瞬时回到原来位置
                transform.position = new Vector3(m_originPos.x, m_originPos.y, transform.position.z);
                this.gameObject.layer = LayerMask.NameToLayer("Quest");
                this.transform.localScale = originalScale;
                NEWOFFSET = 0;
                OLDOFFSET = 0;
                //this.enabled = false;
                //过渡回到原来位置
            }
            //}
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //可以采取距离判断或者物体碰撞的方式判断机关是否已经破解。此处采用物体碰撞
            if (collision.gameObject.Equals(destGameobject))
            {
                mytest.OnEnable();
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
                    NEWOFFSET = (m_originPos.y - currPos.y) / 100;
                    //Debug.Log(m_originPos.y - currPos.y);
                    //Debug.Log(currPos.y);
                    if (NEWOFFSET != OLDOFFSET && currPos.y < m_originPos.y)
                    {

                        if (NEWOFFSET > OLDOFFSET)
                        {
                            if (this.transform.localScale.x < 0.6f)
                            {
                                Vector3 scaleOffset = new Vector3(NEWOFFSET, NEWOFFSET, NEWOFFSET);
                                this.transform.localScale += scaleOffset;
                                OLDOFFSET = NEWOFFSET;
                            }
                        }
                        else if (NEWOFFSET < OLDOFFSET)
                        {
                            if (this.transform.localScale.x > 0.05f)
                            {
                                Vector3 scaleOffset = new Vector3(OLDOFFSET, OLDOFFSET, OLDOFFSET);
                                this.transform.localScale -= scaleOffset;
                                OLDOFFSET = NEWOFFSET;
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
                        //瞬时回到原来位置
                        transform.position = new Vector3(m_originPos.x, m_originPos.y, transform.position.z);
                        this.gameObject.layer = LayerMask.NameToLayer("Quest");
                        this.transform.localScale = originalScale;
                        NEWOFFSET = 0;
                        OLDOFFSET = 0;
                        //this.enabled = false;
                        //过渡回到原来位置
                    }
                }
            }
        }*/
    }

}