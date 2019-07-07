using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLargerQuest : MonoBehaviour,QuestBehavior
{
    private float m_originDistance;
    private bool m_isSuccess = false;

    public void OnUpdate()
    {
        this.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Android端
        if (!m_isSuccess && Input.touchCount == 2) 
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch2.phase == TouchPhase.Began)
            {
                m_originDistance = Vector2.Distance(touch1.position, touch2.position);
                return;
            }

            if (touch1.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved)
            {
                float newDistance = Vector2.Distance(touch1.position, touch2.position);
                if (newDistance < m_originDistance)
                {
                    Debug.Log("镜头拉远机关成功");
                    m_isSuccess = true;
                    //播放镜头拉远动画
                    GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>().SetBool("transitionToStage2", true);
                    GameObject.FindGameObjectWithTag("Boy").transform.parent.gameObject.GetComponent<Animator>().SetTrigger("MakeBoyBigger");
                    QuestController.Instance.UnRegisterQuest(gameObject.ToString());
                    Destroy(this);
                }
            }
        }

        //PC端测试
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Debug.Log("镜头拉远机关成功");
            m_isSuccess = true;
            //播放镜头拉远动画
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>().SetBool("transitionToStage2", true);
            GameObject.FindGameObjectWithTag("Boy").transform.parent.gameObject.GetComponent<Animator>().SetTrigger("MakeBoyBigger");
            QuestController.Instance.UnRegisterQuest(gameObject.ToString());
            Destroy(this);
        }

    }

}
