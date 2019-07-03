using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotate : MonoBehaviour
{
    private WaterWaveEffect m_setWave;
    private ParticleSystem m_waterL, m_waterR, m_waterM;
    private float m_time;
    private bool m_stop;

    void Start()
    {
        m_setWave = GameObject.Find("Additional Camera").GetComponent<WaterWaveEffect>();
        m_waterL = GameObject.Find("Water_Left").GetComponent<ParticleSystem>();
        m_waterR = GameObject.Find("Water_Right").GetComponent<ParticleSystem>();
        m_waterM = GameObject.Find("Water_Mid").GetComponent<ParticleSystem>();
        m_time = 0f;
        m_stop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_stop)
        {           
            this.transform.Rotate(new Vector3(0, 0, Time.deltaTime * -10));
            if (m_time == 0f)
                m_setWave.SetWave();
            m_time += Time.deltaTime;
            if (m_time > 3.0f)
                m_time = 0f;
        }
    }

    public void StopSelfRotate()
    {
        m_stop = true;
        m_waterL.Stop();
        m_waterR.Stop();
        m_waterM.Stop();
    }

    public void Restart()
    {
        m_stop = false;
        m_waterL.Play();
        m_waterM.Play();
        m_waterR.Play();
    }
}
