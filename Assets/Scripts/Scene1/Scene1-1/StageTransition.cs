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
            if (stateInfo.normalizedTime > 1.0f && stateInfo.IsName("Stage1To2")&&isFirst)
            {
                //isFirst需最开始改变
                isFirst = false;
                Debug.Log("启用额外相机");
                Camera.main.clearFlags = CameraClearFlags.Depth;
                mainCamera.transform.GetChild(0).gameObject.SetActive(true);

                //加载Stage1-2
                GameController.Instance.LoadNextStageGameObjects("stage1-2");
                //卸载Stage1-1
                GameController.Instance.UnloadPreStageGameobjects();

                //播放动画
                mainCamera.GetComponent<Animator>().SetBool("transitionToStage2", true);
                GameObject.FindGameObjectWithTag("Player").transform.parent.gameObject.GetComponent<Animator>().SetTrigger("MakeBoyBigger");


            }
            if (stateInfo.normalizedTime > 1.0f && stateInfo.IsName("Stage2ToOrigin"))
            {
                GameObject boy = GameObject.FindGameObjectWithTag("Player");
                GameObject character = boy.transform.parent.gameObject;
                boy.transform.SetParent(boy.transform.parent.parent);
                Destroy(character);

                PlayerController.Instance.enabled = true;
                GetComponent<Animator>().enabled = false;
                Destroy(this);
            }

        }
    }
}
