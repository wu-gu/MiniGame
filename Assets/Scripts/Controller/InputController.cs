using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class InputController
    {
        public LayerMask questLayerMask;

        public float questRaycastDistance = 0.1f;


        public void onUpdate()
        {
            if (Input.touchCount>0)
            {
                //先判断是否触摸某个机关
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
                RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero, questRaycastDistance, questLayerMask.value);
                if (hit.collider != null)
                {
                    QuestController.Instance.FireQuestBehavior(hit.collider.gameObject.ToString());
                }
                else
                {
                    //没有点击机关，则进入人物行走判断

                }
            }
        }
    }
}

