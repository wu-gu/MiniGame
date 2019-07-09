using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiniGame
{
    public class PromptGUIController : MonoBehaviour
    {
        public CanvasGroup promptWindow;
        private float m_promptUIAlpha = 0.0f;
        
        private float m_alphaSpeed = 2.0f;

        //当前是第几关，在有Setm_tipImgsGroup下可以被弃用了，交由GameController控制
        //public int currLevel;
        //提示画组
        private CanvasGroup m_tipImgsGroup;
        //提示画编号组
        private bool[] m_tipIndexes = new bool[5];
        //提示画
        private Dictionary<int,Transform> m_tipImgsDictionary = new Dictionary<int, Transform>();

        private void Start()
        {
            SettipImgsGroup(GameObject.Find("TipImgs").GetComponent<CanvasGroup>());
            SettipIndexes(3);
        }

        private void Update()
        {
            if (m_promptUIAlpha != promptWindow.alpha)
            {
                promptWindow.alpha = Mathf.Lerp(promptWindow.alpha, m_promptUIAlpha, m_alphaSpeed * Time.deltaTime);
                if (Mathf.Abs(m_promptUIAlpha - promptWindow.alpha) <= 0.01)
                {
                    promptWindow.alpha = m_promptUIAlpha;
                }
            }
        }

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
            for(int i = 0; i < m_tipIndexes.Length;)
            {
                //展示画，通过设置透明度实现
                if(m_tipIndexes[i])
                {
                    Transform outTemp;
                    m_tipImgsDictionary.TryGetValue(i ,out outTemp);
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
            for(int i = 0; i < index;)
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

