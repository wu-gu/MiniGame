using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class InputController : MonoSingleton<InputController>
    {
        public LayerMask questLayerMask;

        public float questRaycastDistance = 0.1f;

        private bool m_canMovePlayer = true;//是否可以手动控制人物


        void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }

        public void OnUpdate()
        {

            if (Input.GetMouseButtonDown(0))
            {
                //先判断是否触摸某个机关
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero, questRaycastDistance, questLayerMask.value);
                if (hit.collider != null)
                {
                    Debug.Log("点击机关");
                    Debug.Log("点击机关: " + hit.collider.gameObject.ToString());
                    QuestController.Instance.FireQuestBehavior(hit.collider.gameObject.ToString());
                }
                else
                {
                    Debug.Log("没有点击机关");
                    //没有点击机关，则进入人物行走判断
                    if (m_canMovePlayer)
                    {
                        PlayerController.Instance.OnUpdate();
                    }
                }
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                //Debug.Log("点击");
                //先判断是否触摸某个机关
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
                RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero, questRaycastDistance, questLayerMask.value);
                if (hit.collider != null)
                {
                    //Debug.Log("点击机关");
                    QuestController.Instance.FireQuestBehavior(hit.collider.gameObject.ToString());
                }
                else
                {
                    //Debug.Log("没有点击机关");
                    //没有点击机关，则进入人物行走判断
                    if (m_canMovePlayer)
                    {
                        PlayerController.Instance.OnUpdate();
                    }
                }
            }
        }

        public void SetPlayerCanMove(bool canMove)
        {
            m_canMovePlayer = canMove;
        }
    }
}

