using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiniGame
{

    public class myimage2 : MonoBehaviour
    {
        public Material grayMaterial;
        public int ownLevel;

        CanvasGroup myimage;
        private float alpha = 0.0f;
        private float alphaSpeed = 0.5f;
        // Start is called before the first frame update
        void Start()
        {
            myimage = this.transform.GetComponent<CanvasGroup>();
            if (GameController.Instance.highestProgress <= ownLevel)
                GetComponent<Image>().material = grayMaterial;
        }

        // Update is called once per frame
        void Update()
        {
            if (alpha != myimage.alpha)
            {
                myimage.alpha = Mathf.Lerp(myimage.alpha, alpha, alphaSpeed * Time.deltaTime);
                if (Mathf.Abs(alpha - myimage.alpha) <= 0.01)
                {
                    myimage.alpha = alpha;
                }
            }
        }

        public void Show()
        {
            alpha = 1;

            myimage.blocksRaycasts = true;//可以和该UI对象交互

            myimage.interactable = true;
        }

        public void Hide()
        {
            alpha = 0;

            myimage.blocksRaycasts = false;//不可以和该UI对象交互

            myimage.interactable = false;
        }
    }
}
