//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[RequireComponent(typeof(LineRenderer))]
//public class FloatLineManager : MonoBehaviour
//{
//    LineRenderer lr;

//    List<GameObject> objList = new List<GameObject>();

//    //顶点数量
//    [SerializeField, Range(1, 100)] public int pointcount = 20;
//    //顶点距离
//    [SerializeField, Range(0.01f, 1f)] public static float pointdis = 0.3f;

//    public static Vector3 avgStep;

//    public GameObject endPoint;

//    // Use this for initialization
//    void Start()
//    {
//        lr = gameObject.GetComponent<LineRenderer>();

//        avgStep = (transform.position - endPoint.transform.position) / pointcount;
//        avgStep.z = 0;

//        lr.positionCount = pointcount;
//        //生成顶点
//        for (int i = 0; i < pointcount; i++)
//        {
//            GameObject obj = new GameObject(i + "");
//            //obj.transform.position = new Vector3(transform.position.x + (float)i * pointdis, transform.position.y, 0);
//            obj.transform.position = transform.position - i * avgStep;
//            objList.Add(obj);

//            FloatLinePoint point = obj.AddComponent<FloatLinePoint>();
//            Debug.Log(obj.transform.position);
//            if (i == 0)
//            {
//                point.parentObj = gameObject;
//            }
//            else
//            {
//                point.parentObj = objList[i - 1];
//            }

//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        Vector3[] vs = new Vector3[objList.Count];
//        for (int i = 0; i < objList.Count; i++)
//        {
//            vs[i] = objList[i].transform.position;
//        }
//        lr.SetPositions(vs);
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FloatLineManager : MonoBehaviour
{
    LineRenderer lr;

    List<Vector3> pointList = new List<Vector3>();

    //顶点数量
    [SerializeField, Range(1, 100)] public int pointcount = 40;
    //顶点距离
    [SerializeField, Range(0.01f, 1f)] public static float pointdis = 0.3f;

    public Vector3 avgStep;
    public float speed = 0.8f;

    public GameObject endPoint;

    // Use this for initialization
    void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();

        avgStep = (transform.position - endPoint.transform.position) / pointcount;
        avgStep.z = 0;

        lr.positionCount = pointcount;
        //生成顶点
        for (int i = 0; i < pointcount; i++)
        {
            Vector3 floatPoint = transform.position - i * avgStep;
            pointList.Add(floatPoint);
            Debug.Log(floatPoint);
        }
    }

    // Update is called once per frame
    void Update()
    {
        pointList[0] = transform.position;
        for (int i = 1; i < pointList.Count; i++)
        {
            pointList[i] = Vector3.Lerp(pointList[i-1] - avgStep, pointList[i], speed);
        }
        lr.SetPositions(pointList.ToArray());
    }
}

