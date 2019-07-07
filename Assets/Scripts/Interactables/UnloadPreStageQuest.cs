using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MiniGame
{
    public class UnloadPreStageQuest : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Boy"))
            {
                Debug.Log("人物走到检查点，可以卸载一个画面");
                GameController.Instance.UnloadPreStageGameobjects();
                Destroy(this);
            }
        }
    }
}
