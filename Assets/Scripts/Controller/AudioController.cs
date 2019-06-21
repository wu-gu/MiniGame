using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.Audio;

namespace MiniGame
{
    public class AudioController : MonoSingleton<AudioController>
    {
        //用于播放音效的音乐源
        [Header("Music Settings")]
        public AudioClip musicAudioClip;
        public AudioMixerGroup musicOutput;
        public bool musicPlayOnAwake = true;
        [Range(0f, 1f)]
        public float musicVolume = 1f;
        //用于播放背景音乐的音乐源
        [Header("Ambient Settings")]
        public AudioClip ambientAudioClip;
        public AudioMixerGroup ambientOutput;
        public bool ambientPlayOnAwake = true;
        [Range(0f, 1f)]
        public float ambientVolume = 1f;

        protected AudioSource m_MusicAudioSource;
        protected AudioSource m_AmbientAudioSource;

        protected bool m_TransferMusicTime, m_TransferAmbientTime;
        protected AudioController m_OldInstanceToDestroy = null;

        //每一个栈中音频剪辑都会被播放，并在播放后弹出
        //音频剪辑通过PushClip方法压入栈，这个方法也是提供给开发者调用的接口
        protected Stack<AudioClip> m_MusicStack = new Stack<AudioClip>();

        void Awake ()
        {
            // If there's already a player...
            if (Instance != null && Instance != this)
            {
                //...if it use the same music clip, we set the audio source to be at the same position, so music don't restart
                if(Instance.musicAudioClip == musicAudioClip)
                {
                    m_TransferMusicTime = true;
                }

                //...if it use the same ambient clip, we set the audio source to be at the same position, so ambient don't restart
                if (Instance.ambientAudioClip == ambientAudioClip)
                {
                    m_TransferAmbientTime = true;
                }

                // 销毁之前的管理器
                m_OldInstanceToDestroy = Instance;
            }

            DontDestroyOnLoad (gameObject);
            //实例化音乐源
            m_MusicAudioSource = gameObject.AddComponent<AudioSource> ();
            m_MusicAudioSource.clip = musicAudioClip;
            m_MusicAudioSource.outputAudioMixerGroup = musicOutput;
            m_MusicAudioSource.loop = true;
            m_MusicAudioSource.volume = musicVolume; //音量大小

            if (musicPlayOnAwake)
            {
                m_MusicAudioSource.time = 0f;
                m_MusicAudioSource.Play();
            }

            m_AmbientAudioSource = gameObject.AddComponent<AudioSource>();
            m_AmbientAudioSource.clip = ambientAudioClip;
            m_AmbientAudioSource.outputAudioMixerGroup = ambientOutput;
            m_AmbientAudioSource.loop = true;
            m_AmbientAudioSource.volume = ambientVolume;

            if (ambientPlayOnAwake)
            {
                m_AmbientAudioSource.time = 0f;
                m_AmbientAudioSource.Play();
            }
        }

        private void Start()
        {
            //if delete & trasnfer time only in Start so we avoid the small gap that doing everything at the same time in Awake would create 
            if (m_OldInstanceToDestroy != null)
            {
                if (m_TransferAmbientTime) m_AmbientAudioSource.timeSamples = m_OldInstanceToDestroy.m_AmbientAudioSource.timeSamples;
                if (m_TransferMusicTime) m_MusicAudioSource.timeSamples = m_OldInstanceToDestroy.m_MusicAudioSource.timeSamples;
                m_OldInstanceToDestroy.Stop();
                Destroy(m_OldInstanceToDestroy.gameObject);
            }
        }

        private void Update()
        {
            if(m_MusicStack.Count > 0)
            {
                //isPlaying will be false once the current clip end up playing
                if(!m_MusicAudioSource.isPlaying)
                {
                    m_MusicStack.Pop();
                    if(m_MusicStack.Count > 0)
                    {
                        m_MusicAudioSource.clip = m_MusicStack.Peek();
                        m_MusicAudioSource.Play();
                    }
                    else
                    {//Back to looping music clip
                        m_MusicAudioSource.clip = musicAudioClip;
                        m_MusicAudioSource.loop = true;
                        m_MusicAudioSource.Play();
                    }
                }
            }
        }

        //音效入栈方法
        public void PushClip(AudioClip clip)
        {
            m_MusicStack.Push(clip);
            m_MusicAudioSource.Stop();
            m_MusicAudioSource.loop = false;
            m_MusicAudioSource.time = 0;
            m_MusicAudioSource.clip = clip;
            m_MusicAudioSource.Play();
        }

        public void ChangeMusic(AudioClip clip)
        {
            musicAudioClip = clip;
            m_MusicAudioSource.clip = clip;
        }

        public void ChangeAmbient(AudioClip clip)
        {
            ambientAudioClip = clip;
            m_AmbientAudioSource.clip = clip;
        }


        public void Play ()
        {
            PlayJustAmbient ();
            PlayJustMusic ();
        }

        public void PlayJustMusic ()
        {
            m_MusicAudioSource.time = 0f;
            m_MusicAudioSource.Play();
        }

        public void PlayJustAmbient ()
        {
            m_AmbientAudioSource.Play();
        }

        public void Stop ()
        {
            StopJustAmbient ();
            StopJustMusic ();
        }

        public void StopJustMusic ()
        {
            m_MusicAudioSource.Stop ();
        }

        public void StopJustAmbient ()
        {
            m_AmbientAudioSource.Stop ();
        }

        //静音
        public void Mute ()
        {
            MuteJustAmbient ();
            MuteJustMusic ();
        }

        public void MuteJustMusic ()
        {
            m_MusicAudioSource.volume = 0f;
        }

        public void MuteJustAmbient ()
        {
            m_AmbientAudioSource.volume = 0f;
        }

        //取消静音
        public void Unmute ()
        {
            UnmuteJustAmbient ();
            UnmuteJustMustic ();
        }

        public void UnmuteJustMustic ()
        {
            m_MusicAudioSource.volume = musicVolume;
        }

        public void UnmuteJustAmbient ()
        {
            m_AmbientAudioSource.volume = ambientVolume;
        }

        //渐渐消音
        public void Mute (float fadeTime)
        {
            MuteJustAmbient(fadeTime);
            MuteJustMusic(fadeTime);
        }

        public void MuteJustMusic (float fadeTime)
        {
            StartCoroutine(VolumeFade(m_MusicAudioSource, 0f, fadeTime));
        }

        public void MuteJustAmbient (float fadeTime)
        {
            StartCoroutine(VolumeFade(m_AmbientAudioSource, 0f, fadeTime));
        }

        //渐渐取消静音
        public void Unmute (float fadeTime)
        {
            UnmuteJustAmbient(fadeTime);
            UnmuteJustMusic(fadeTime);
        }

        public void UnmuteJustMusic (float fadeTime)
        {
            StartCoroutine(VolumeFade(m_MusicAudioSource, musicVolume, fadeTime));
        }

        public void UnmuteJustAmbient (float fadeTime)
        {
            StartCoroutine(VolumeFade(m_AmbientAudioSource, ambientVolume, fadeTime));
        }

        //声音渐变
        protected IEnumerator VolumeFade (AudioSource source, float finalVolume, float fadeTime)
        {
            float volumeDifference = Mathf.Abs(source.volume - finalVolume);
            float inverseFadeTime = 1f / fadeTime;

            while (!Mathf.Approximately(source.volume, finalVolume))
            {
                float delta = Time.deltaTime * volumeDifference * inverseFadeTime;
                source.volume = Mathf.MoveTowards(source.volume, finalVolume, delta);
                yield return null;
            }
            source.volume = finalVolume;
        }

        //播放背景音乐，传进一个音频名
        private void PlayBgBase(object bgName, bool restart = false)
        {
            //定义一个空的字符串
            string curBgName = string.Empty;
            //如果这个音乐源的音频剪辑不为空
            if (m_AmbientAudioSource.clip != null)
            {
                //得到这个音频剪辑名
                curBgName = m_AmbientAudioSource.clip.name;
            }

            //根据用户的音频片段名称找到AudioClip，然后播放
            //ResourcesMagr是提前定义好的用于查找音频剪辑对应路径的单例脚本，动态加载资源
            AudioClip clip = ResourcesMgr.Instance.Load<AudioClip>(bgName);
            //如果加载了对应音频剪辑
            if (clip != null)
            {
                //如果这个音频剪辑已经赋值给了音频源，且正在播放，那么跳出
                if (clip.name == curBgName && !restart)
                {
                    return;
                }
                //否则，把该音频剪辑赋值给音频源，然后播放
                m_AmbientAudioSource.clip = clip;
                m_AmbientAudioSource.Play();
            }
            else
            {
                //没有找到就报错，debug用
                UnityEngine.Debug.Log("没有对应音频片段");
            }
        }

        //播放各种饮片剪辑的调用方法，AudioType是已经写好的枚举类，调用接口
        public void PlayBg()
        {
            PlayBgBase(ambientAudioClip.name);
        }

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
            if (clip == null)
            {
                UnityEngine.Debug.Log("没有找到音效片段");
                return;
            }

            //否则，如果defAudio = true，播放
            if (defAudio == true)
            {
                //PlayOnShot参数1为音频剪辑，参数2为音量大小
                m_MusicAudioSource.PlayOneShot(clip, volume);
            }
            else
            {
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
            }
        }

        //播放各种音效的调用方法，调用接口
        public void PlayEffect()
        {
            PlayEffectBase(musicAudioClip.name);
        }

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
}