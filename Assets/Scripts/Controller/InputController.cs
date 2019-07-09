using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
            //此处PC端输入在Android端会干扰输入，导出Android的时候需要注释掉
//#if UNITY_STANDALONE_WIN
                if (Input.GetMouseButtonDown(0)&& !EventSystem.current.IsPointerOverGameObject())
                {
                    GameController.Instance.HideGamingUI();
                    //先判断是否触摸某个机关
                    Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero, questRaycastDistance, questLayerMask.value);
                    if (hit.collider != null)
                    {
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
//#endif

#if UNITY_ANDROID
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began&& !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                GameController.Instance.HideGamingUI();
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
#endif
        }


        //public bool IsPointerOverUIObject(Vector2 screenPosition)
        //{
        //    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        //    eventDataCurrentPosition.position = new Vector2(screenPosition.x, screenPosition.y);

        //    List<RaycastResult> results = new List<RaycastResult>();
        //    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        //    return results.Count > 0;
        //}

        public void SetPlayerCanMove(bool canMove)
        {
            m_canMovePlayer = canMove;
            PlayerController.Instance.enabled = canMove;
        }
    }
}

