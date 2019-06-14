using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    private Vector3 _vec3TargetScreenSpace;// 目标物体的屏幕空间坐标
    private Vector3 _vec3TargetWorldSpace;// 目标物体的世界空间坐标
    private Rigidbody2D _rigidbody;// 目标物体的空间变换组件
    private Vector3 _vec3MouseScreenSpace;// 鼠标的屏幕空间坐标
    private Vector3 _vec3Offset;// 偏移

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake() { _rigidbody = GetComponent<Rigidbody2D>(); }

    IEnumerator OnMouseDown()

    {

        // 把目标物体的世界空间坐标转换到它自身的屏幕空间坐标 

        _vec3TargetScreenSpace = Camera.main.WorldToScreenPoint(_rigidbody.position);

        // 存储鼠标的屏幕空间坐标（Z值使用目标物体的屏幕空间坐标） 

        _vec3MouseScreenSpace = new Vector3(Input.mousePosition.x, _vec3TargetScreenSpace.y, _vec3TargetScreenSpace.z);

        // 计算目标物体与鼠标物体在世界空间中的偏移量 
        Vector3 currentPos = new Vector3(_rigidbody.position.x, _rigidbody.position.y, 0);
        _vec3Offset = currentPos - Camera.main.ScreenToWorldPoint(_vec3MouseScreenSpace);

        // 鼠标左键按下 

        while (Input.GetMouseButton(0))
        {

            // 存储鼠标的屏幕空间坐标（Z值使用目标物体的屏幕空间坐标）

            _vec3MouseScreenSpace = new Vector3(Input.mousePosition.x, _vec3TargetScreenSpace.y, _vec3TargetScreenSpace.z);

            // 把鼠标的屏幕空间坐标转换到世界空间坐标（Z值使用目标物体的屏幕空间坐标），加上偏移量，以此作为目标物体的世界空间坐标
            _vec3TargetWorldSpace = Camera.main.ScreenToWorldPoint(_vec3MouseScreenSpace) + _vec3Offset;
            _rigidbody.MovePosition(_vec3TargetWorldSpace);
            // 更新目标物体的世界空间坐标 

            //_rigidbody.position = _vec3TargetWorldSpace;

            // 等待固定更新 

            yield return new WaitForFixedUpdate();
        }
    }
}
