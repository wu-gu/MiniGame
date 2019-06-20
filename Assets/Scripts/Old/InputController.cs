//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace _Ecin
//{
//    // Control player movement and trick trigger component control in tutorial(trick trigger control is supposed to be seperate in later development) -> trickControl and movement control has been seperated
//    public class InputController : MonoBehaviour
//    {
//        public LayerMask groundedLayerMask;
//        private ContactFilter2D m_contactFilter;
//        private CapsuleCollider2D playerBB;
//        private bool movable;
//        private Vector2 dest;
//        private PlayerMovement playerMovement;
//        private WalkSakura sakura;

//        // Start is called before the first frame update
//        void Start()
//        {
//            GameObject girl = GameObject.Find("Girl");
//            playerBB = girl.GetComponent<CapsuleCollider2D>();
//            movable = false;
//            playerMovement = girl.GetComponent<PlayerMovement>();
//            sakura = GameObject.Find("OnClickSakura").GetComponent<WalkSakura>();
//            m_contactFilter.layerMask = groundedLayerMask;
//            m_contactFilter.useLayerMask = true;
//            m_contactFilter.useTriggers = false;
//        }

//        void Update()
//        {
//            // Touch count is only able in Android platform
//            if (Input.GetMouseButtonDown(0)/* && Input.touchCount == 1*/)
//            {
//                Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//                dest = new Vector2(mousePoint.x, mousePoint.y);
//                Debug.Log(dest);
//                float threshold = playerBB.bounds.max.y - playerBB.bounds.min.y + 0.8f;
//                Debug.Log(threshold);
//                // Movable: (y-axis value of mouse position is within player's bounds || layer.Ground) && unblocked

//                // Check if y-axis value is within threshold
//                RaycastHit2D[] hitBuffer = new RaycastHit2D[1];
//                int hitCount = Physics2D.Raycast(dest, Vector2.down, m_contactFilter, hitBuffer, threshold);
//                RaycastHit2D hit;
//                hit = Physics2D.Raycast(dest, Vector2.down, threshold);
//                if (hitCount > 0 && hit.collider.gameObject.layer != LayerMask.NameToLayer("Quest"))
//                {
//                    Debug.Log("Within threshold without quest in between, movable");
//                    movable = true;
//                }
//                else
//                    return;

//                // Check if blocked

//                if (movable)
//                {
//                    sakura.SetSakura(dest);
//                    playerMovement.MoveTo(dest);
//                }
//            }
//            else
//                return;
//        }
//    }
//}
