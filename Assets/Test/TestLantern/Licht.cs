using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Licht : MonoBehaviour
{
    private Animator m_animator;
    private GameObject m_blowPoint;
    private int m_lichtingRight, m_lichtingLeft, m_lichtingMid;
    private bool m_reset;
    private bool m_revert;
    //private LichtUp m_lichtUp;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = this.GetComponent<Animator>();
        m_blowPoint = GameObject.Find("Blow Point");
        m_lichtingRight = Animator.StringToHash("LichtingRight");
        m_lichtingLeft = Animator.StringToHash("LichtingLeft");       
        m_lichtingMid = Animator.StringToHash("LichtingMid");
        m_revert = true;
        m_reset = false;
        //m_lichtUp = GameObject.Find("Light Ambient").GetComponent<LichtUp>();
    }

    void Update()
    {
        AnimatorStateInfo animatorStateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        if(animatorStateInfo.IsName("Licht_idle"))
        {
            GameObject.Find("Glowworm_1").GetComponent<Glowworm>().UnRegisterQuest();
            GameObject.Find("Glowworm_2").GetComponent<Glowworm>().UnRegisterQuest();
            GameObject.Find("Glowworm_3").GetComponent<Glowworm>().UnRegisterQuest();
            Destroy(m_blowPoint);
            //m_lichtUp.OnUpdate();
            this.enabled = false;
        }
    }

    public void LichtRight()
    {
        m_revert = false;
        m_reset = false;
        m_animator.SetBool(m_lichtingRight, false);
        m_animator.SetBool(m_lichtingLeft, false);
        m_animator.SetBool(m_lichtingMid, false);
        AnimatorStateInfo animatorStateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        if(animatorStateInfo.IsName("WindBlowed"))
        {
            m_animator.SetBool(m_lichtingRight, true);
            //m_animator.SetBool(m_lichtingRight, false);
        }
        if(animatorStateInfo.IsName("Licht_right"))
        {
            m_revert = true;
        }
        if(animatorStateInfo.IsName("Licht_left"))
        {
            m_revert = true;
        }
    }

    public void LichtLeft()
    {
        m_revert = false;
        m_reset = false;
        m_animator.SetBool(m_lichtingRight, false);
        m_animator.SetBool(m_lichtingLeft, false);
        m_animator.SetBool(m_lichtingMid, false);
        AnimatorStateInfo animatorStateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        if(animatorStateInfo.IsName("WindBlowed"))
        {
            m_revert = true;
        }
        if(animatorStateInfo.IsName("Licht_right"))
        {
            m_animator.SetBool(m_lichtingLeft, true);
            //m_animator.SetBool(m_lichtingLeft, false);
        }
        if(animatorStateInfo.IsName("Licht_left"))
        {
            m_revert = true;
        }
    }

    public void LichtMid()
    {
        m_revert = false;
        m_reset = false;
        m_animator.SetBool(m_lichtingRight, false);
        m_animator.SetBool(m_lichtingLeft, false);
        m_animator.SetBool(m_lichtingMid, false);
        AnimatorStateInfo animatorStateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        if (animatorStateInfo.IsName("WindBlowed"))
        {
            m_revert = true;
        }
        if (animatorStateInfo.IsName("Licht_right"))
        {
            m_animator.SetBool(m_lichtingMid, true);
            //m_animator.SetBool(m_lichtingMid, false);
            m_reset = true;
        }
        if (animatorStateInfo.IsName("Licht_left"))
        {
            m_animator.SetBool(m_lichtingMid, true);
            //m_animator.SetBool(m_lichtingMid, false);
        }
    }

    public bool AcceptLicht()
    {
        AnimatorStateInfo animatorStateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        if (!animatorStateInfo.IsName("Idle") && !animatorStateInfo.IsName("WindBlow") && !animatorStateInfo.IsName("Licht_idle"))
        {
            return true;
        }
        else
            return false;
    }

    public bool Revert()
    {
        return m_revert;
    }

    public bool Reset()
    {
        return m_reset;
    }

}
