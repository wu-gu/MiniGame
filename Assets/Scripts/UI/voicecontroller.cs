using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class voicecontroller : MonoBehaviour
{
    public AudioSource source;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void voicecontrol()
    {
        source.volume = slider.value;
    }
}
