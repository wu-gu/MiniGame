using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame {

    public class LoadNextStageQuest : MonoBehaviour
    {
        public string nextStageName;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Boy"))
            {
                Debug.Log("人物走到检查点，可以加载下一个画面");
                if (nextStageName != null)
                {
                    GameController.Instance.LoadNextStageGameObjects(nextStageName);
                    //调整摄像机边界限定
                    //Camera.main.gameObject.GetComponent<CameraController>().UpdateBackgounrdLeft(GameObject.Find(nextStageName+"-L"));
                    //加载时调整右边，卸载时调整左边
                    Camera.main.gameObject.GetComponent<CameraController>().UpdateBackgounrdRight(GameObject.Find(nextStageName + "-R"));
                    Destroy(this);
                }
            }
        }
    }
}
