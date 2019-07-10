using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiniGame
{
    //GamingSetButton
    public class GamingUI : MonoBehaviour
    {
        /*提示UI*/
        public CanvasGroup promptWindow;
        private float m_promptUIAlpha = 0.0f;

        private float m_alphaSpeed = 1.0f;

        //当前是第几关，在有Setm_tipImgsGroup下可以被弃用了，交由GameController控制
        //public int currLevel;
        //提示画组
        private CanvasGroup m_tipImgsGroup;
        //提示画编号组
        private bool[] m_tipIndexes = new bool[5];
        //提示画
        private Dictionary<int, Transform> m_tipImgsDictionary = new Dictionary<int, Transform>();


        /*设置UI*/
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

        private void Start()
        {
            CanvasGroup cg = GameObject.Find("TipImgs").GetComponent<CanvasGroup>();
            cg.alpha = 1.0f;
            SettipImgsGroup(cg);
        }

        private void Update()
        {
            if (Application.platform == RuntimePlatform.Android && (Input.GetKeyDown(KeyCode.Escape)))
            {
                ShowExitWindow();
            }

            //设置窗口透明度渐变
            if (settingAlpha != settingImage.alpha)
            {
                settingImage.alpha = Mathf.Lerp(settingImage.alpha, settingAlpha, alphaSpeed * Time.deltaTime);
                if (Mathf.Abs(settingAlpha - settingImage.alpha) <= 0.01)
                {
                    settingImage.alpha = settingAlpha;
                }
            }

            //提示窗口透明度渐变
            if (m_promptUIAlpha != promptWindow.alpha)
            {
                promptWindow.alpha = Mathf.Lerp(promptWindow.alpha, m_promptUIAlpha, m_alphaSpeed * Time.deltaTime);
                if (Mathf.Abs(m_promptUIAlpha - promptWindow.alpha) <= 0.01)
                {
                    promptWindow.alpha = m_promptUIAlpha;
                }
            }
        }


        /*设置UI接口*/
        public void ShowSettingWindow()
        {
            ambientMusicSlider.value = AudioController.Instance.backgroundVolume;
            effectMusicSlider.value = AudioController.Instance.soundEffectVolume;
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

        /// <summary>
        /// 当点击显示窗口以外地方的时候，关闭当前所有窗口
        /// </summary>
        public void HideShowingWindow()
        {
            HideExitWindow();
            HideSettingWindow();
            HidePromptWindow();
        }

        public void ExitGame()
        {
            GameController.Instance.EndGame();
        }

        public void AmbientMusicVolumeChanged(Slider slider)
        {
            AudioController.Instance.BackgroundMusicVolumeChanged(slider);
        }

        public void EffectMusicVolumeChanged(Slider slider)
        {
            AudioController.Instance.SoundEffectVolumeChanged(slider);
        }


        /*提示UI接口*/
        /// <summary>
        /// 显示提示界面，这个不仅在点击“提示”按钮时触发，还应该在每一个有碎片补充的机关节点成功后触发
        /// </summary>
        public void ShowPromptWindow()
        {
            Debug.Log("打开提示窗口");
            m_promptUIAlpha = 1;
            promptWindow.blocksRaycasts = true;
            promptWindow.interactable = true;
            ShowPromptTipImgs();
        }

        /// <summary>
        /// 隐藏提示界面，需要再机关节点成功后动画等触发前调用，故机关节点调用show和hide应该用协程，等待几秒
        /// </summary>
        public void HidePromptWindow()
        {
            Debug.Log("关闭提示窗口");
            m_promptUIAlpha = 0;
            promptWindow.blocksRaycasts = false;
            promptWindow.interactable = false;
        }

        /// <summary>
        /// 延时指定秒数，执行代码，用于机关成功时调用
        /// </summary>
        /// <param name="t">延时秒数</param>
        /// <returns></returns>
        public void ShowForTime(float t)
        {
            StartCoroutine(ShowForTimeCoroutine(t));
        }
        IEnumerator ShowForTimeCoroutine(float t)
        {
            ShowPromptWindow();
            yield return new WaitForSeconds(t);//运行到这，暂停t秒

            //t秒后，继续运行下面代码
            HidePromptWindow();
        }

        /// <summary>
        /// 更新当前可显示画作
        /// </summary>
        public void ShowPromptTipImgs()
        {
            for (int i = 0; i < m_tipIndexes.Length;)
            {
                //展示画，通过设置透明度实现
                if (m_tipIndexes[i])
                {
                    Transform outTemp;
                    m_tipImgsDictionary.TryGetValue(i, out outTemp);
                    outTemp.gameObject.GetComponent<Image>().color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
                }
                ++i;
            }
        }

        /// <summary>
        /// 根据当前是第几关加载不同的画组预制件
        /// </summary>
        /// <param name="tipImgs">画组预制件</param>
        public void SettipImgsGroup(CanvasGroup tipImgs)
        {
            this.m_tipImgsGroup = tipImgs;
            this.m_tipImgsGroup.transform.SetParent(promptWindow.transform);
            Debug.Log("画组预制件" + tipImgs.name);
            AddTipImgs();
        }

        /// <summary>
        /// 根据当前该关卡已解谜机关编号设置提示画编号
        /// </summary>
        /// <param name="index">已解谜机关编号</param>
        public void SettipIndexes(int index)
        {
            for (int i = 0; i < index;)
            {
                this.m_tipIndexes[i] = true;
                ++i;
            }
        }

        /// <summary>
        /// 根据画组预制件加载画
        /// </summary>
        private void AddTipImgs()
        {
            //只遍历所有的子物体,没有孙物体，遍历不包含本身
            int i = 0;
            foreach (Transform child in m_tipImgsGroup.transform)
            {
                m_tipImgsDictionary.Add(i, child);
                ++i;
            }
        }
    }

}
