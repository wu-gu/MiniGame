using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Ecin
{
    // Control player movement and trick trigger component control in tutorial(trick trigger control is supposed to be seperate in later development) 
    public class PlayerController : MonoBehaviour
    {
        private CapsuleCollider2D playerBB;
        private bool needToMove;
        private float playerSpeed;
        private Vector3 oriPos;
        private Vector3 dest;
        private bool forwardable;
        private CircleCollider2D windowBB;

        // Start is called before the first frame update
        void Start()
        {
            playerBB = GetComponent<CapsuleCollider2D>();
            needToMove = false;
            playerSpeed = 2f;
            forwardable = true;
            windowBB = GameObject.Find("Window").GetComponent<CircleCollider2D>();
            GameObject.Find("Window").GetComponent<ScaleMask>().enabled = false;
            GameObject.Find("Door").GetComponent<OpenDoor>().enabled = false;
            oriPos = this.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            // Detect mouse input
            if (Input.GetMouseButtonDown(0))
            {
                // Complete movement settings
                //dest = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //if (dest.x < playerBB.bounds.min.x || dest.x > playerBB.bounds.max.x || dest.y < playerBB.bounds.min.y || dest.y > playerBB.bounds.max.y)
                //    needToMove = true;
                //else
                //    needToMove = false;

                // Horizontal movement settings
                Vector3 tmp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                dest = new Vector3(tmp.x, this.transform.position.y, this.transform.position.z);

                // Detect the need of movement
                if (dest.x != this.transform.position.x)
                    needToMove = true;
                else
                    needToMove = false;
            }

            // If player is not forwardable: blocked by un-triggered tricks, player is able to move backward, but not forward
            // Moving forward == no need to move
            if (forwardable == false)
            {
                Debug.Log("Blocked");
                
                // The player is not allowed to interact with tricks until getting blocked
                GameObject.Find("Window").GetComponent<ScaleMask>().enabled = true;
                if (dest.x > this.transform.position.x)
                {
                    dest = this.transform.position;
                    needToMove = false;
                }
            }

            // If this statement is reached, either there is really no need to move, or player is blocked and wants to move forward
            if (!needToMove)
            {
                oriPos = this.transform.position;
                return;
            }

            // If this statement is reached, either the player has not been blocked, or the player is blocked and wants to move backward
            Vector3 direction = (dest - this.transform.position).normalized;
            if (needToMove)
            {
                // Player stops before collision hanppens, collision only triggered with tricks(current assumption, only tricks and a few special game objects have collider component)
                if (playerBB.bounds.max.x + direction.x * playerSpeed * Time.deltaTime >= windowBB.bounds.min.x)
                {
                    forwardable = false;
                    needToMove = false;
                    return;
                }
                else
                    this.transform.Translate(direction * playerSpeed * Time.deltaTime);
            }

            // Detect if the player has reached movement destination
            if ((dest.x > oriPos.x && this.transform.position.x >= dest.x) || (dest.x < oriPos.x && this.transform.position.x <= dest.x))
            {
                needToMove = false;
                dest = this.transform.position;
                oriPos = this.transform.position;
            }


            // If this statement is reached, either the player has not reach blocked point or the player had reached blocked point and went backward
            forwardable = true;
        }
    }
}
