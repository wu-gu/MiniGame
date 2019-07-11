using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{

    /// <summary>
    /// 在Stage1-4到Stage1-5的转场过程中使用
    /// </summary>
    public class CameraAnimation : MonoBehaviour
    {

        [Tooltip("Stage1-4 to 1-5转场音乐")]
        public AudioClip transitionClip;

        /// <summary>
        /// 动画开始播放，关闭主相机深度设置
        /// </summary>
        public void CloseDepthOnly()
        {
            PlayerController.Instance.enabled = false;
            GetComponent<CameraController>().enabled = false;
            gameObject.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            AudioController.Instance.PushClip(transitionClip);
        }

        /// <summary>
        /// 将人物变到屋子里面去
        /// </summary>
        public void ChangeBoyTransform()
        {
            Debug.Log("人物进入屋内");
            //改变人物位置
            GameObject boy = GameObject.FindGameObjectWithTag("Boy");
            boy.transform.localScale = new Vector3(0.07f, 0.07f, 1);
            boy.transform.position = new Vector3(boy.transform.position.x, 0.7f, 10);
            GameObject.Find("GroundInDoor").GetComponent<Collider2D>().enabled = true;
            GameObject.Find("SmokePaticle").transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find("SmokePaticle").transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
            //Rigidbody2D rigidbody2D = boy.GetComponent<Rigidbody2D>();
            //rigidbody2D.position = new Vector3(rigidbody2D.position.x, 0.7f, 10);

        }

        /// <summary>
        /// 卸载画面4
        /// </summary>
        public void UnloadStage4()
        {

            Debug.Log("销毁Stage1-3以及1-4，停止水流声");
            //停止水流环境音
            AudioController.Instance.MuteJustEnvironment(1.0f);

            GetComponent<Animator>().enabled = false;
            //Destroy(GameObject.Find("Stage1-3"));
            GameController.Instance.UnloadPreStageGameobjects();//currStage在Stage1-4(而1-5是在1-4里面的)
            Destroy(GameObject.Find("Stage1-4"));
            //启用人物移动与摄像机跟随
            PlayerController.Instance.enabled = true;

            CameraController cameraController = GetComponent<CameraController>();
            cameraController.enabled = true;
            cameraController.UpdateBackgounrdLeft(GameObject.Find("Stage1-5-L"));
            cameraController.UpdateBackgounrdRight(GameObject.Find("Stage1-5-R"));
            
        }


    }
}
