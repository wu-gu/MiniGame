using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Ecin
{
    // Only player simple movement is contained in this script
    public class SimpleMove : MonoBehaviour
    {
        private bool needToMove;
        private float playerSpeed;
        private Vector3 oriPos;
        private Vector3 dest;

        // Start is called before the first frame update
        void Start()
        {
            needToMove = false;
            playerSpeed = 3f;
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

                if (dest.x != this.transform.position.x)
                    needToMove = true;
                else
                    needToMove = false;
            }

            if (!needToMove)
            {
                oriPos = this.transform.position;
                return;
            }

            Vector3 direction = (dest - this.transform.position).normalized;
            if (needToMove)
                this.transform.Translate(direction * playerSpeed * Time.deltaTime);

            // Player reaches destination
            if ((dest.x > oriPos.x && this.transform.position.x >= dest.x) || (dest.x < oriPos.x && this.transform.position.x <= dest.x))
            {
                needToMove = false;
                dest = this.transform.position;
            }
        }
    }
}
