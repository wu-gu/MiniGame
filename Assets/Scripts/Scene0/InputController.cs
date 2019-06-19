using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Ecin
{
    // Control player movement and trick trigger component control in tutorial(trick trigger control is supposed to be seperate in later development) -> trickControl and movement control has been seperated
    public class InputController : MonoBehaviour
    {
        private CapsuleCollider2D playerBB;
        private bool movable;
        private Vector3 oriPos;
        private Vector3 dest;
        private PlayerMovement playerMovement;
        private WalkSakura sakura;

        // Start is called before the first frame update
        void Start()
        {
            GameObject girl = GameObject.Find("Girl");
            playerBB = girl.GetComponent<CapsuleCollider2D>();
            movable = false;
            oriPos = playerBB.transform.position;
            playerMovement = girl.GetComponent<PlayerMovement>();
            sakura = GameObject.Find("OnClickSakura").GetComponent<WalkSakura>();
        }

        void Update()
        {
            // Touch count is only able in Android platform
            if (Input.GetMouseButtonDown(0)/* && Input.touchCount == 1*/)
            {
                Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                dest = new Vector3(mousePoint.x, mousePoint.y, oriPos.z);
                Debug.Log(dest);
                float threshold = playerBB.bounds.max.y - playerBB.bounds.min.y + 0.8f;
                Debug.Log(threshold);
                // Movable: (y-axis value of mouse position is within player's bounds || layer.Ground) && unblocked

                // Check if y-axis value is within threshold
                RaycastHit2D hit;
                hit = Physics2D.Raycast(new Vector2(dest.x, dest.y), Vector2.down, threshold);
                if (hit.collider != null && hit.collider.gameObject.layer != LayerMask.NameToLayer("Quest"))
                {
                    Debug.Log("Within threshold without quest in between, movable");
                    movable = true;
                }
                else
                    return;

                // Check if blocked

                if (movable)
                {
                    sakura.SetSakura(dest);
                    playerMovement.MoveTo(dest);
                }
            }
            else
                return;
        }
    }
}
