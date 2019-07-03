using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame {

    public class LoadNextStageQuest : MonoBehaviour
    {
        public string nextStageName;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("人物走到检查点，可以加载下一个画面");
                if (nextStageName != null)
                {
                    GameController.Instance.LoadNextStageGameObjects(nextStageName);
                    Destroy(this);
                }
            }
        }
    }
}
