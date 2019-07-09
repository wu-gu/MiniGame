using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class BlockPoint : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject.Find("Window").GetComponent<ScaleMask>().SetBlocked();
        }
    }
}
