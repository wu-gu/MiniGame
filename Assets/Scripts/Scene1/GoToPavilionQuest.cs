using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{

    public class GoToPavilionQuest : MonoBehaviour, QuestBehavior
    {
        public GameObject character;

        private bool m_canWork = false;

        // Start is called before the first frame update
        private void Awake()
        {
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
        }

        public void Update()
        {
            AnimatorStateInfo stateInfo = character.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime > 1.0f && stateInfo.IsName("MoveAlongRoad"))
            {
                Debug.Log("第1-1关结束");
                //人物停止行走动画
                character.transform.Find("Boy").GetComponent<Animator>().SetBool("isWalking", false);
                QuestController.Instance.UnRegisterQuest(gameObject.ToString());
                //转场
                //GameObject.Find("TransitionStart").GetComponent<TransitionPoint>().Transition();
                this.enabled = false;
            }
        }

        public void OnUpdate()
        {
            if (m_canWork)
            {
                this.enabled = true;
                character.GetComponent<Animator>().enabled = true;
                Debug.Log(character.transform.Find("Boy").ToString());
                character.transform.Find("Boy").GetComponent<Animator>().SetBool("isWalking", true);
            }
        }

        public void DoWorking()
        {
            GetComponent<SpriteRenderer>().enabled = true;
            m_canWork = true;
        }




    }
}
