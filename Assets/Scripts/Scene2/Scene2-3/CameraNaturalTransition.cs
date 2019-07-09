using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNaturalTransition : MonoBehaviour
{
    private bool m_transitionNeed1;
    private bool m_transitionNeed2;
    private Vector3 m_destPos1;
    private Vector3 m_destPos2;
    private float speed;
    private Animator m_animator;
    private int IsColorChangeTime;

    // Start is called before the first frame update
    void Start()
    {
        this.enabled = false;
        m_transitionNeed1 = false;
        m_transitionNeed2 = false;
        speed = 2.5f;
        m_animator = this.GetComponent<Animator>();
        m_destPos1 = new Vector3(20.4f, -2.03f, -10f);
        m_destPos2 = new Vector3(27.19f, -2.03f, -10f);
        IsColorChangeTime = Animator.StringToHash("IsColorChangeTime");
    }        

    // Update is called once per frame
    void Update()
    {
        if(m_transitionNeed1)
        {
            if (Mathf.Abs(transform.position.x - m_destPos1.x) < 0.01f)
            {
                this.transform.position = m_destPos1;
                m_transitionNeed1 = false;
                m_animator.enabled = true;
                m_animator.SetTrigger(IsColorChangeTime);
                this.enabled = false;
                return;
            }
            
            Vector3 nextPos = Vector3.MoveTowards(transform.position, m_destPos1, speed * Time.deltaTime);
            transform.position = nextPos;     
        }
        else if(m_transitionNeed2)
        {
            if (Mathf.Abs(transform.position.x - m_destPos2.x) < 0.01f)
            {
                this.transform.position = m_destPos2;
                m_transitionNeed2 = false;
                this.enabled = false;
                return;
            }

            Vector3 nextPos = Vector3.MoveTowards(transform.position, m_destPos2, speed * Time.deltaTime);
            transform.position = nextPos;
        }
    }

    public void NaturalTransition1()
    {
        m_transitionNeed1 = true;
        this.enabled = true;
    }

    public void NaturalTransition2()
    {
        m_transitionNeed2 = true;
        this.enabled = true;
    }
}
