using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject m_camera;//人物自己以及相机的对象
    ColorAdjustEffect m_colorAdjustEffect;
    // Start is called before the first frame update
    void Start()
    {
        m_camera = GameObject.FindGameObjectWithTag("Main Camera");
        m_colorAdjustEffect = m_camera.GetComponent<ColorAdjustEffect>();
        //m_colorAdjustEffect = m_camera.GetComponent("Color Adjust Effect") as ColorAdjustEffect;
        m_colorAdjustEffect.brightness = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
