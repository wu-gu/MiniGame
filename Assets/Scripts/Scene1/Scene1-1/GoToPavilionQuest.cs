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

        }

        void Start()
        {
            character = GameObject.Find("Character");
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
            this.enabled = false;
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
                //摄像机镜头拉近
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>().enabled = true;
                //转场
                //GameObject.Find("TransitionStart").GetComponent<TransitionPoint>().Transition();
                //this.enabled = false;
                Destroy(this);
            }
        }

        public void OnUpdate()
        {

            if (m_canWork)
            {
                this.enabled = true;
                GetComponent<ParticleSystem>().Stop();
                GetComponent<ParticleSystem>().Clear();
                Debug.Log("男主前进");
                character.GetComponent<Animator>().enabled = true;//男主按照设定路线行进
                //Debug.Log(character.transform.Find("Boy").ToString());
                character.transform.Find("Boy").GetComponent<Animator>().SetBool("isWalking", true);
            }
        }

        public void DoWorking()
        {
            Debug.Log("提示光圈出现");
            GetComponent<ParticleSystem>().Play();
            m_canWork = true;
        }




    }
}
