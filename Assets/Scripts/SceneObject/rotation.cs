using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour
{
    private bool isClick = false;
    private Vector2 position;
    private Vector2 oldPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        position = Input.mousePosition;
        if (isClick)
        {
            transform.Rotate(Vector3.forward * (position.x - oldPosition.x + position.y - oldPosition.y), Space.World);
            //transform.Rotate(Vector3.right * position.y, Space.World);
        }
        oldPosition = Input.mousePosition;
    }

    void OnMouseUp()
    { //鼠标抬起
        isClick = false;
    }

    void OnMouseDown()
    { //鼠标按下

        isClick = true;
    }
}
