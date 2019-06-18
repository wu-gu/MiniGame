using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//音乐管理类，继承自单例
public class AudioMgr : MonoSingleton<AudioMgr>
{
    //用于播放背景音乐的音乐源
    private AudioSource m_bgMusic;

    //用于播放音效的音乐源
    private AudioSource m_effectMusic;

    //用于控制背景音乐音量大小
    public float BgVolume
    {
        get
        {
            return m_bgMusic.volume;
        }
        set
        {
            m_bgMusic.volume = value;
        }
    }

    //重写Awake虚方法
    protected override void Awake()
    {
        base.Awake();
        //实例化音乐源
        m_bgMusic = gameObject.AddComponent<AudioSource>();
        m_bgMusic.loop = true; //循环
        m_bgMusic.playOnAwake = false; //开始时播放

        //实例化音乐源
        m_effectMusic = gameObject.AddComponent<AudioSource>();
        m_effectMusic.loop = true; //循环
        m_effectMusic.playOnAwake = false; //开始时播放
    }

    //播放背景音乐，传进一个音频名
    private void PlayBgBase(object bgName, bool restart = false)
    {
        //定义一个空的字符串
        string curBgName = string.Empty;
        //如果这个音乐源的音频剪辑不为空
        if(m_bgMusic.clip != null)
        {
            //得到这个音频剪辑名
            curBgName = m_bgMusic.clip.name;
        }

        //根据用户的音频片段名称找到AudioClip，然后播放
        //ResourcesMagr是提前定义好的用于查找音频剪辑对应路径的单例脚本，动态加载资源
        AudioClip clip = ResourcesMgr.Instance.Load<AudioClip>(bgName);
        //如果赵傲了对应音频剪辑
        if(clip != null)
        {
            //如果这个音频剪辑已经赋值给了音频源，且正在播放，那么跳出
            if(clip.name == curBgName && !restart)
            {
                return;
            }
            //否则，把该音频剪辑赋值给音频源，然后播放
            m_bgMusic.clip = clip;
            m_bgMusic.Play();
        }
        else
        {
            //没有找到就报错，debug用
            UnityEngine.Debug.Log("没有对应音频片段");
        }
    }

    //播放各种饮片剪辑的调用方法，AudioType是已经写好的枚举类，调用接口
    public void PlayBg(AudioType.Background bgName, bool restart = false)
    {
        PlayBgBase(bgName, restart);
    }

    public void PlayBg(AudioType.Events bgName, bool restart = false)
    {
        PlayBgBase(bgName, restart);
    }

    public void PlayBg(AudioType.Items bgName, bool restart = false)
    {
        PlayBgBase(bgName, restart);
    }

    //播放音效
    private void PlayEffectBase(object effectName, bool defAudio = true, float volume = 1f)
    {
        AudioClip clip = ResourcesMgr.Instance.Load<AudioClip>(effectName);
        //如果为空，报错并跳出
        if(clip == null)
        {
            UnityEngine.Debug.Log("没有找到音效片段");
            return;
        }

        //否则，如果defAudio = true，播放
        if(defAudio == true)
        {
            //PlayOnShot参数1为音频剪辑，参数2为音量大小
            m_effectMusic.PlayOneShot(clip, volume);
        }
        else
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
        }
    }

    //播放各种音效的调用方法，调用接口
    public void PlayEffect(AudioType.Background effectName, bool defAudio = true, float volume = 1f)
    {
        PlayEffectBase(effectName, defAudio, volume);
    }

    public void PlayEffect(AudioType.Items effectName, bool defAudio = true, float volume = 1f)
    {
        PlayEffectBase(effectName, defAudio, volume);
    }

    public void PlayEffect(AudioType.Events effectName, bool defAudio = true, float volume = 1f)
    {
        PlayEffectBase(effectName, defAudio, volume);
    }
}
