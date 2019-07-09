using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiniGame
{

    public class GamingSetButton : MonoBehaviour
    {
        public CanvasGroup settingImage;
        public CanvasGroup exitImage;

        public Slider ambientMusicSlider;
        public Slider effectMusicSlider;

        private float settingAlpha = 0.0f;
        private float alphaSpeed = 1.0f;

        // Start is called before the first frame update
        //void Start()
        //{
        //    settingImage = GameObject.Find("SettingImage").GetComponent<CanvasGroup>();
        //    exitImage = GameObject.Find("ExitImage").GetComponent<CanvasGroup>();
        //}

        private void Update()
        {
            if (Application.platform == RuntimePlatform.Android && (Input.GetKeyDown(KeyCode.Escape)))
            {
                ShowExitWindow();
            }

            if (settingAlpha != settingImage.alpha)
            {
                settingImage.alpha = Mathf.Lerp(settingImage.alpha, settingAlpha, alphaSpeed * Time.deltaTime);
                if (Mathf.Abs(settingAlpha - settingImage.alpha) <= 0.01)
                {
                    settingImage.alpha = settingAlpha;
                }
            }
        }

        public void ShowSettingWindow()
        {
            ambientMusicSlider.value = AudioController.Instance.ambientVolume;
            effectMusicSlider.value = AudioController.Instance.musicVolume;
            Debug.Log("打开设置窗口");
            settingAlpha = 1;
            settingImage.blocksRaycasts = true;
            settingImage.interactable = true;
        }

        /// <summary>
        /// 隐藏设置界面
        /// </summary>
        public void HideSettingWindow()
        {
            Debug.Log("关闭设置窗口");
            settingAlpha = 0;
            settingImage.blocksRaycasts = false;
            settingImage.interactable = false;
        }

        /// <summary>
        /// 回到开始界面
        /// </summary>
        public void BackToStartLevel()
        {
            Debug.Log("回到主界面");
            GameController.Instance.TransitionToNewLevel("Start");
        }

        public void HideExitWindow()
        {
            Debug.Log("关闭退出窗口");
            exitImage.alpha = 0;
            exitImage.blocksRaycasts = false;//
            exitImage.interactable = false;
        }

        public void ShowExitWindow()
        {
            Debug.Log("打开退出窗口");
            exitImage.alpha = 1;
            exitImage.blocksRaycasts = true;//
            exitImage.interactable = true;
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void AmbientMusicVolumeChanged(Slider slider)
        {
            AudioController.Instance.AmbientMusicVolumeChanged(slider);
        }

        public void EffectMusicVolumeChanged(Slider slider)
        {
            AudioController.Instance.EffectMusicVolumeChanged(slider);
        }
    }

}
