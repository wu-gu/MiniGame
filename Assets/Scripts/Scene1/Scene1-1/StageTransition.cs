using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{

    public class StageTransition : MonoBehaviour
    {
        GameObject mainCamera;

        bool isFirst = true;

        // Start is called before the first frame update
        void Start()
        {
            //mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            mainCamera = gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            AnimatorStateInfo stateInfo = GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0);
            //if (stateInfo.normalizedTime > 1.0f && stateInfo.IsName("Stage1To2")&&isFirst)
            if (stateInfo.normalizedTime > 1.0f && stateInfo.IsName("Stage1To2_b") && isFirst)
            {
                //isFirst需最开始改变
                isFirst = false;
                Debug.Log("启用额外相机");
                Camera.main.clearFlags = CameraClearFlags.Depth;
                mainCamera.transform.GetChild(0).gameObject.SetActive(true);

                //加载Stage1-2
                GameController.Instance.LoadNextStageGameObjects("Stage1-2");
                //卸载Stage1-1
                GameController.Instance.UnloadPreStageGameobjects();

                ////播放镜头拉远动画
                //mainCamera.GetComponent<Animator>().SetBool("transitionToStage2", true);
                //GameObject.FindGameObjectWithTag("Player").transform.parent.gameObject.GetComponent<Animator>().SetTrigger("MakeBoyBigger");


            }
            //if (stateInfo.normalizedTime > 1.0f && stateInfo.IsName("Stage2ToOrigin"))
            if (stateInfo.normalizedTime > 1.0f && stateInfo.IsName("Stage2ToOriScale"))
            {
                //让Boy脱离Character
                GameObject boy = GameObject.FindGameObjectWithTag("Boy");
                GameObject character = boy.transform.parent.gameObject;
                boy.transform.SetParent(boy.transform.parent.parent);
                Destroy(character);

                PlayerController.Instance.enabled = true;
                GetComponent<Animator>().enabled = false;

                //播放水流环境音
                AudioController.Instance.PlayJustEnvironment();
                AudioController.Instance.UnmuteJustEnvironment(1.0f);

                //调整摄像机边界限定，要注意和摄像机动画的冲突
                Camera.main.gameObject.GetComponent<CameraController>().UpdateBackgounrdLeft(GameObject.Find("Stage1-2-L"));
                Camera.main.gameObject.GetComponent<CameraController>().UpdateBackgounrdRight(GameObject.Find("Stage1-2-R"));
                Destroy(this);
            }
        }

        // 启用影子动画状态机
        public void SetUpShadows()
        {
            Animator shadows = GameObject.Find("Shadows").GetComponent<Animator>();
            shadows.enabled = true;
        }
    }
}
