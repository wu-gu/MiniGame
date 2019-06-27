using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagpieQuestFiredAnimations : MonoBehaviour
{
    GameObject m_willow;
    GameObject m_flyingMagpie;
    public IEnumerator MagpieQuestFired()
    {
        WillowShake();
        yield return new WaitForSeconds(2f);
        FlyingMagpieAppear();
    }

    void WillowShake()
    {
        m_willow = GameObject.Find("Willow");
        m_willow.GetComponent<Animator>().SetBool("MagpieQuestFired", true);
    }

    void FlyingMagpieAppear()
    {
        m_flyingMagpie = GameObject.Find("FlyingMagpie");
        m_flyingMagpie.GetComponent<Animator>().SetBool("MagpieQuestFired", true);
    }
}
