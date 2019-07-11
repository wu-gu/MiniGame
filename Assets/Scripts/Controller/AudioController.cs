using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace MiniGame
{
    public class AudioController : MonoSingleton<AudioController>
    {
        //用于播放音效的音乐源
        [Header("Sound Effect Settings")]
        public AudioClip soundEffectAudioClip;
        //public AudioMixerGroup musicOutput;
        public bool soundEffectPlayOnAwake = true;
        [Range(0f, 1f)]
        public float soundEffectVolume = 0.8f;

        //用于播放背景音乐的音乐源
        [Header("Background Settings")]
        public AudioClip backgroundAudioClip;
        //public AudioMixerGroup BackgroundOutput;
        public bool backgroundPlayOnAwake = true;
        [Range(0f, 1f)]
        public float backgroundVolume = 0.5f;

        //用于播放环境场景音的音乐源
        [Header("Envirnment Settings")]
        public AudioClip environmentAudioClip;
        //public AudioMixerGroup environmentOutput;
        public bool environmentPlayOnAwake = true;
        [Range(0f, 1f)]
        public float environmentVolume = 0.8f;

        protected AudioSource m_SoundEffectAudioSource;
        protected AudioSource m_BackgroundAudioSource;
        protected AudioSource m_EnvironmentAudioSource;

        protected bool m_TransferSoundEffectTime, m_TransferBackgroundTime, m_TransferEnvironmentTime;
        //protected AudioController m_OldInstanceToDestroy = null;

        //每一个栈中音频剪辑都会被播放，并在播放后弹出
        //音频剪辑通过PushClip方法压入栈，这个方法也是提供给开发者调用的接口
        protected Stack<AudioClip> m_MusicStack = new Stack<AudioClip>();

        void Awake ()
        {
            //// If there's already a player...
            //if (Instance != null && Instance != this)
            //{
            //    //...if it use the same music clip, we set the audio source to be at the same position, so music don't restart
            //    if(Instance.soundEffectAudioClip == soundEffectAudioClip)
            //    {
            //        m_TransferSoundEffectTime = true;
            //    }

            //    //...if it use the same Background clip, we set the audio source to be at the same position, so Background don't restart
            //    if (Instance.backgroundAudioClip == backgroundAudioClip)
            //    {
            //        m_TransferBackgroundTime = true;
            //    }

            //    // 销毁之前的管理器
            //    m_OldInstanceToDestroy = Instance;
            //}
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            //DontDestroyOnLoad (gameObject);



            //实例化音乐源
            m_SoundEffectAudioSource = gameObject.AddComponent<AudioSource> ();
            m_SoundEffectAudioSource.clip = soundEffectAudioClip;
            //m_SoundEffectAudioSource.outputAudioMixerGroup = musicOutput;
            m_SoundEffectAudioSource.loop = false;
            m_SoundEffectAudioSource.volume = soundEffectVolume; //音量大小

            m_BackgroundAudioSource = gameObject.AddComponent<AudioSource>();
            m_BackgroundAudioSource.clip = backgroundAudioClip;
            //m_BackgroundAudioSource.outputAudioMixerGroup = BackgroundOutput;
            m_BackgroundAudioSource.loop = true;
            m_BackgroundAudioSource.volume = backgroundVolume;

            m_EnvironmentAudioSource = gameObject.AddComponent<AudioSource>();
            m_EnvironmentAudioSource.clip = environmentAudioClip;
            //m_EnvironmentAudioSource.outputAudioMixerGroup = environmentOutput;
            m_EnvironmentAudioSource.loop = true;
            m_EnvironmentAudioSource.volume = environmentVolume;


            if (soundEffectPlayOnAwake)
            {
                m_SoundEffectAudioSource.time = 0f;
                m_SoundEffectAudioSource.Play();
            }

            if (backgroundPlayOnAwake)
            {
                m_BackgroundAudioSource.time = 0f;
                m_BackgroundAudioSource.Play();
            }

            if (environmentPlayOnAwake)
            {
                m_EnvironmentAudioSource.time = 0f;
                m_EnvironmentAudioSource.Play();
            }
        }

        //private void Start()
        //{
        //    //if delete & trasnfer time only in Start so we avoid the small gap that doing everything at the same time in Awake would create 
        //    if (m_OldInstanceToDestroy != null)
        //    {
        //        if (m_TransferBackgroundTime) m_BackgroundAudioSource.timeSamples = m_OldInstanceToDestroy.m_BackgroundAudioSource.timeSamples;
        //        if (m_TransferSoundEffectTime) m_SoundEffectAudioSource.timeSamples = m_OldInstanceToDestroy.m_SoundEffectAudioSource.timeSamples;
        //        m_OldInstanceToDestroy.Stop();
        //        Destroy(m_OldInstanceToDestroy.gameObject);
        //    }
        //}

        private void Update()
        {
            if(m_MusicStack.Count > 0)
            {
                //isPlaying will be false once the current clip end up playing
                if(!m_SoundEffectAudioSource.isPlaying)
                {
                    //把播完的弹出来
                    m_MusicStack.Pop();
                    if(m_MusicStack.Count > 0)
                    {
                        m_SoundEffectAudioSource.clip = m_MusicStack.Peek();
                        m_SoundEffectAudioSource.Play();
                    }
                    else
                    {//Back to looping music clip
                        m_SoundEffectAudioSource.clip = soundEffectAudioClip;
                        m_SoundEffectAudioSource.loop = true;
                        m_SoundEffectAudioSource.Play();
                    }
                }
            }
        }

        //音效入栈方法
        public void PushClip(AudioClip clip)
        {
            if(m_MusicStack.Count>0)
                m_MusicStack.Pop();
            m_MusicStack.Push(clip);
            m_SoundEffectAudioSource.Stop();
            m_SoundEffectAudioSource.loop = false;
            m_SoundEffectAudioSource.time = 0;
            m_SoundEffectAudioSource.clip = clip;
            m_SoundEffectAudioSource.Play();
        }

        public void ChangeSoundEffect(AudioClip clip)
        {
            soundEffectAudioClip = clip;
            m_SoundEffectAudioSource.clip = clip;
        }

        public void ChangeBackground(AudioClip clip)
        {
            backgroundAudioClip = clip;
            m_BackgroundAudioSource.clip = clip;
        }

        public void ChangeEnviroment(AudioClip clip)
        {
            environmentAudioClip = clip;
            m_EnvironmentAudioSource.clip = clip;
        }

        public void Play ()
        {
            PlayJustBackground ();
            PlayJustSoundEffect ();
            PlayJustEnvironment();
        }

        public void PlayJustSoundEffect ()
        {
            m_SoundEffectAudioSource.time = 0f;
            m_SoundEffectAudioSource.Play();
        }

        public void PlayJustBackground ()
        {
            m_BackgroundAudioSource.Play();
        }

        public void PlayJustEnvironment()
        {
            m_EnvironmentAudioSource.Play();
        }

        public void Stop ()
        {
            StopJustBackground ();
            StopJustSoundEffect();
            StopJustEnvironment();
        }

        public void StopJustSoundEffect()
        {
            m_SoundEffectAudioSource.Stop ();
        }

        public void StopJustBackground ()
        {
            m_BackgroundAudioSource.Stop ();
        }

        public void StopJustEnvironment()
        {
            m_EnvironmentAudioSource.Stop();
        }

        //静音
        public void Mute ()
        {
            MuteJustBackground ();
            MuteJustSoundEffect();
            MuteJustEnvironment();
        }

        public void MuteJustSoundEffect()
        {
            m_SoundEffectAudioSource.volume = 0f;
        }

        public void MuteJustBackground ()
        {
            m_BackgroundAudioSource.volume = 0f;
        }

        public void MuteJustEnvironment()
        {
            m_EnvironmentAudioSource.volume = 0f;
        }

        //取消静音
        public void Unmute ()
        {
            UnmuteJustBackground ();
            UnmuteJustSoundEffect();
            UnmuteJustEnvironment();
        }

        public void UnmuteJustSoundEffect()
        {
            m_SoundEffectAudioSource.volume = soundEffectVolume;
        }

        public void UnmuteJustBackground ()
        {
            m_BackgroundAudioSource.volume = backgroundVolume;
        }

        public void UnmuteJustEnvironment()
        {
            m_EnvironmentAudioSource.volume = environmentVolume;
        }

        //渐渐消音
        public void Mute (float fadeTime)
        {
            MuteJustBackground(fadeTime);
            MuteJustSoundEffect(fadeTime);
            MuteJustEnvironment(fadeTime);
        }

        public void MuteJustSoundEffect(float fadeTime)
        {
            StartCoroutine(VolumeFade(m_SoundEffectAudioSource, 0f, fadeTime));
        }

        public void MuteJustBackground (float fadeTime)
        {
            StartCoroutine(VolumeFade(m_BackgroundAudioSource, 0f, fadeTime));
        }

        public void MuteJustEnvironment(float fadeTime)
        {
            StartCoroutine(VolumeFade(m_EnvironmentAudioSource, 0f, fadeTime));
        }

        //渐渐取消静音
        public void Unmute (float fadeTime)
        {
            UnmuteJustBackground(fadeTime);
            UnmuteJustSoundEffect(fadeTime);
            UnmuteJustEnvironment(fadeTime);
        }

        public void UnmuteJustSoundEffect(float fadeTime)
        {
            StartCoroutine(VolumeFade(m_SoundEffectAudioSource, soundEffectVolume, fadeTime));
        }

        public void UnmuteJustBackground (float fadeTime)
        {
            StartCoroutine(VolumeFade(m_BackgroundAudioSource, backgroundVolume, fadeTime));
        }

        public void UnmuteJustEnvironment(float fadeTime)
        {
            StartCoroutine(VolumeFade(m_EnvironmentAudioSource, environmentVolume, fadeTime));
        }

        public void SetsoundEffectVolume(float volume)
        {
            soundEffectVolume = volume;
        }

        public void SetbackgroundVolume(float volume)
        {
            backgroundVolume = volume;
        }

        public void SetEnvironmentVolume(float volume)
        {
            environmentVolume = volume;
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
            if (m_BackgroundAudioSource.clip != null)
            {
                //得到这个音频剪辑名
                curBgName = m_BackgroundAudioSource.clip.name;
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
                m_BackgroundAudioSource.clip = clip;
                m_BackgroundAudioSource.Play();
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
            PlayBgBase(backgroundAudioClip.name);
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
                m_SoundEffectAudioSource.PlayOneShot(clip, volume);
            }
            else
            {
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
            }
        }

        //播放各种音效的调用方法，调用接口
        public void PlayEffect()
        {
            PlayEffectBase(soundEffectAudioClip.name);
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

        //调节背景音乐音量
        public void BackgroundMusicVolumeChanged(Slider slider)
        {
            backgroundVolume = slider.value;
            UnmuteJustBackground();
        }

        //调节音效音乐音量
        public void SoundEffectVolumeChanged(Slider slider)
        {
           //PlayerController.Instance.ChangeWalkigVolume(slider.value);
           soundEffectVolume = slider.value;
            environmentVolume = slider.value;
            UnmuteJustEnvironment();
            UnmuteJustSoundEffect();
        }
    }
}