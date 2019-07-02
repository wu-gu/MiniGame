using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{

    public class StageTransition : MonoBehaviour
    {
        GameObject mainCamera;

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
            if (stateInfo.normalizedTime > 1.0f && stateInfo.IsName("Stage1To2"))
            {

                //加载Stage1-2
                GameController.Instance.LoadNextStageGameObjects("stage1-2");
                //卸载Stage1-1
                GameController.Instance.UnloadPreStageGameobjects();
                GameObject boy = GameObject.FindGameObjectWithTag("Player");
                boy.transform.SetParent(boy.transform.parent.parent);
                Destroy(boy.transform.parent.gameObject);
                mainCamera.GetComponent<Animator>().SetBool("transitionToStage2", true);

            }
            if (stateInfo.normalizedTime > 1.0f && stateInfo.IsName("Stage2ToOrigin"))
            {

                Destroy(this);
            }

        }
    }
}
