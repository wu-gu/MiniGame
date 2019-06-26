using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class CameraController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Camera.main.aspect = 16.0f/9;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}