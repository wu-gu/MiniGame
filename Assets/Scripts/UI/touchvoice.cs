using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class touchvoice : MonoBehaviour
{
    public AudioSource source;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            source.Play();    // 按下Q键播放玩家攻击音效
        }

    }

    public void voicecontrol()
    {
        source.volume = slider.value;
    }
}
