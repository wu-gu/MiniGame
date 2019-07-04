using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPavilion : MonoBehaviour, QuestBehavior
{
    private Collider2D m_collider2D;
    private Animator m_animator;
    private GameObject destGameobject;
    private Vector3 boypos;
    private Vector3 doorpos;
    private bool canopen=true;
    private bool isFirst = true;
    public float distance;


    public void OnUpdate()
    {
        if (canopen&& isFirst)
        {
            isFirst = false;
            m_animator.enabled = true;
        }


    }

    // Start is called before the first frame update
    void Awake()
    {
        QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
        m_animator = GameObject.Find("Pavilion").GetComponent<Animator>();
        m_collider2D = GetComponent<Collider2D>();
        doorpos = this.transform.position;
    }

    void Start()
    {
        destGameobject = GameObject.Find("Boy");
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo animatorInfo;
        animatorInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        boypos = destGameobject.transform.position;
        Debug.Log("distance"+Mathf.Abs(doorpos.x - boypos.x));
        if (Mathf.Abs(doorpos.x - boypos.x) < distance)
        {
            canopen = true;
        }
        else
        {
            canopen = false;
        }
        if ((animatorInfo.normalizedTime > 1.0f))
        {
            QuestController.Instance.UnRegisterQuest(gameObject.ToString());
            m_animator.enabled = false;   
            m_collider2D.enabled = false;
            this.enabled = false;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.Equals(destGameobject))
    //    {
    //        canopen = true;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.Equals(destGameobject))
    //    {
    //        canopen = false;
    //    }
    //}
}
