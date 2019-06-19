using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Ecin
{
    // Only player simple(horizontal) movement is contained in this script
    public class PlayerMovement : MonoBehaviour
    {
        private bool needToMove;
        private float playerSpeed;
        private Vector3 direction;
        private Vector3 dest;

        // Only set once when awake
        void Awake()
        {
            playerSpeed = 2f;
            needToMove = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (needToMove)
            {
                this.transform.Translate(direction * playerSpeed * Time.deltaTime);

                if (direction.x > 0 && this.transform.position.x >= dest.x)
                    needToMove = false;
                if (direction.x < 0 && this.transform.position.x <= dest.x)
                    needToMove = false;
            }
            else
            {
                this.enabled = false;
            }
        }

        public void MoveTo(Vector3 dest)
        {
            needToMove = true;
            this.dest = dest;
            if (dest.x > this.transform.position.x)
                direction = new Vector3(1, 0, 0);
            else
                direction = new Vector3(-1, 0, 0);
            this.enabled = true;
        }
    }
}
