using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterAnimation : MonoBehaviour
{
    private GameObject m_shield;
    private GameObject m_girl;
    private GameObject m_boy;
    // Start is called before the first frame update
    void Start()
    {
        m_girl = GameObject.FindGameObjectWithTag("Girl");
        m_boy = GameObject.Find("Couple");
        m_shield = GameObject.Find("Shield");
    }

    void DestoryShield()
    {
        GameObject.Destroy(m_shield);
    }

    void MeetingTriggerFired()
    {
        m_boy.GetComponent<Animator>().SetBool("MeetingTriggerFired", true);
        m_girl.GetComponent<Animator>().SetBool("MeetingTriggerFired", true);
    }
}
