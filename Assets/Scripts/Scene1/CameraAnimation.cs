using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{


    public class CameraAnimation : MonoBehaviour
    {
        //private GameObject m_stage1_5;
        //private GameObject m_stage1_4;

        private void Start()
        {
            //m_stage1_5 = GameObject.Find("--- Level1-5 ---");
        }

        //void Stage1_5Scale()
        //{
        //    m_stage1_5.GetComponent<Animator>().enabled = true;
        //}

        /// <summary>
        /// 关闭主相机深度设置
        /// </summary>
        public void CloseDepthOnly()
        {
            PlayerController.Instance.enabled = false;
            GetComponent<CameraController>().enabled = false;
            gameObject.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }

        /// <summary>
        /// 卸载画面4
        /// </summary>
        public void UnloadStage4()
        {
            Destroy(GameObject.Find("Stage1-4"));
            //启用人物移动与摄像机跟随
            PlayerController.Instance.enabled = true;
            CameraController cameraController = GetComponent<CameraController>();
            cameraController.enabled = true;
            cameraController.UpdateBackgounrdLeft(GameObject.Find("Stage1-5-L"));
            cameraController.UpdateBackgounrdRight(GameObject.Find("Stage1-5-R"));
            GetComponent<Animator>().enabled = false;
        }

        public void ChangeBoyTransform()
        {
            GameObject boy = GameObject.FindGameObjectWithTag("Boy");
            boy.transform.position = new Vector3(boy.transform.position.x, 1.59f, 10);
            boy.transform.localScale = new Vector3(0.07f, 0.07f, 1);
        }
    }
}
