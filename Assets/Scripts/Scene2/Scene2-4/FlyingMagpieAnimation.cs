using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMagpieAnimation : MonoBehaviour
{
    private GameObject m_standingMagpie;
    private GameObject m_buildingBridgeMagpies;
    private GameObject m_fireParticle;
    private void Start()
    {
        m_standingMagpie = GameObject.Find("StandingMagpie");
        m_buildingBridgeMagpies = GameObject.Find("BuildingBridgeMagpies");
        m_fireParticle = GameObject.Find("FireParticle");
    }

    void FlyingMagpieArrivalEventFired()
    {
        m_standingMagpie.GetComponent<Animator>().SetBool("FlyingMagpieArrivalEventFired", true);
    }

    void BuildingBridgeMagpies()
    {
        m_buildingBridgeMagpies.GetComponent<Animator>().enabled = true;
    }

    void FireParticleDestory()
    {
        GameObject.Destroy(m_fireParticle);
    }

    void FireParticleFade()
    {
        m_fireParticle.GetComponent<Animator>().enabled = true;
    }
}
