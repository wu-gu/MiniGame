using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichtUp : MonoBehaviour
{
    private Light m_light;

    void Start()
    {
        m_light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        m_light.intensity += Time.deltaTime;
        if (m_light.intensity >= 1.0f)
            this.enabled = false;
    }

    public void OnUpdate()
    {
        this.enabled = true;
    }
}
