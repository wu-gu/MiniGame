//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class FloatLinePoint : MonoBehaviour
//{

//    public GameObject parentObj;

//    public static float speed = 0.7f;
//    // Use this for initialization
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        //transform.position = transform.position +
//        //    (parentObj.transform.position - new Vector3(FloatLineManager.avgStep.x, FloatLineManager.avgStep.y, 0) - transform.position) * speed;
//        transform.position = Vector3.Lerp(parentObj.transform.position - FloatLineManager.avgStep, transform.position, speed);
//    }
//}