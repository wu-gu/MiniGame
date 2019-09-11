using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public GameObject backgroundL, backgroundR;
    private float minX, maxX;//摄像机的左边框与右边框的限定
    float orthoHorizontal;
    private Camera m_mainCam;
    private GameObject m_followed;

    public float followSpeed = 2.5f;

    void Awake()
    {
        //Camera.main.aspect = 16.0f / 9;
        // camera viewport: aspect = widht/height
        //minX = backgroundL.GetComponent<Renderer>().bounds.min.x + orthoHorizontal;
        //maxX = backgroundR.GetComponent<Renderer>().bounds.max.x - orthoHorizontal;

        if (backgroundL != null && backgroundR != null)
        {
            //minX = backgroundL.transform.position.x + orthoHorizontal;
            //maxX = backgroundR.transform.position.x - orthoHorizontal;
            minX = backgroundL.transform.position.x;
            maxX = backgroundR.transform.position.x;
        }
        m_mainCam = Camera.main;
        orthoHorizontal = m_mainCam.aspect * m_mainCam.orthographicSize;
        m_followed = GameObject.FindGameObjectWithTag("Boy");
        if (m_followed == null)
            m_followed = GameObject.FindGameObjectWithTag("Girl");
    }

    /*
     * 以下被注释掉的方法为旧版判断摄像机中心左右边界范围
     * 现在为判断摄像机左右边界范围
     */


    // Update is called once per frame
    void LateUpdate()
    {
        if (backgroundL != null && backgroundR != null)
        {
            orthoHorizontal = m_mainCam.aspect * m_mainCam.orthographicSize;
            Debug.DrawRay(new Vector3(minX, m_mainCam.transform.position.y, m_mainCam.transform.position.z), Vector3.down, Color.blue);
            Debug.DrawRay(new Vector3(maxX, m_mainCam.transform.position.y, m_mainCam.transform.position.z), Vector3.down, Color.green);

            //镜头随着人慢慢移动，看人的速度与followSpeed
            float nextX = Mathf.MoveTowards(m_mainCam.transform.position.x, m_followed.transform.position.x, followSpeed*Time.deltaTime);
            //重新计算相机左右边界
            //float destMinX = nextX - orthoHorizontal;
            //float destMaxX = nextX + orthoHorizontal;
            //Debug.DrawRay(new Vector3(destMinX, m_mainCam.transform.position.y, m_mainCam.transform.position.z), Vector3.down, Color.black);
            //Debug.DrawRay(new Vector3(destMaxX, m_mainCam.transform.position.y, m_mainCam.transform.position.z), Vector3.down, Color.cyan);
            //if (destMinX > minX && destMaxX < maxX)
            if (m_followed.transform.position.x - orthoHorizontal > minX && m_followed.transform.position.x + orthoHorizontal < maxX)
            {
                m_mainCam.transform.position = new Vector3(nextX, m_mainCam.transform.position.y, m_mainCam.transform.position.z);
            }
        }
        else
        {
            float nextX = Mathf.MoveTowards(m_mainCam.transform.position.x, m_followed.transform.position.x, followSpeed*Time.deltaTime);
            m_mainCam.transform.position = new Vector3(nextX, m_mainCam.transform.position.y, m_mainCam.transform.position.z);
        }
    }
    //void LateUpdate()
    //{
    //    if (backgroundL != null && backgroundR != null)
    //    {
    //        Debug.DrawRay(new Vector3(minX, m_mainCam.transform.position.y, m_mainCam.transform.position.z), Vector3.down,Color.blue);
    //        Debug.DrawRay(new Vector3(maxX, m_mainCam.transform.position.y, m_mainCam.transform.position.z), Vector3.down,Color.green);
    //        //重新计算相机目标位置
    //        float destX = Mathf.Clamp(m_followed.transform.position.x, minX, maxX);
    //        //镜头随着人慢慢移动，看人的速度与followSpeed
    //        float nextX = Mathf.MoveTowards(m_mainCam.transform.position.x, destX, followSpeed);
    //        m_mainCam.transform.position = new Vector3(nextX, m_mainCam.transform.position.y, m_mainCam.transform.position.z);
    //    }
    //}

    /// <summary>
    /// 更新相机位置左边界
    /// </summary>
    /// <param name="left"></param>
    public void UpdateBackgounrdLeft(GameObject left)
    {
        orthoHorizontal = m_mainCam.aspect* m_mainCam.orthographicSize;
        backgroundL = left;
        minX = backgroundL.transform.position.x;
        Debug.Log("更新相机左边界:" + minX);
    }
    //public void UpdateBackgounrdLeft(GameObject left)
    //{
    //    orthoHorizontal = m_mainCam.aspect * m_mainCam.orthographicSize;
    //    backgroundL = left;
    //    minX = backgroundL.transform.position.x + orthoHorizontal;
    //    Debug.Log("更新相机左边界:" + minX);
    //}


    /// <summary>
    /// 更新相机位置右边界
    /// </summary>
    /// <param name="right"></param>
    public void UpdateBackgounrdRight(GameObject right)
    {
        orthoHorizontal = m_mainCam.aspect * m_mainCam.orthographicSize;
        backgroundR = right;
        maxX = backgroundR.transform.position.x;
        Debug.Log("更新相机右边界:" + maxX);
    }

    //public void UpdateBackgounrdRight(GameObject right)
    //{
    //        orthoHorizontal = m_mainCam.aspect * m_mainCam.orthographicSize;
    //        backgroundR = right;
    //        maxX = backgroundR.transform.position.x - orthoHorizontal;
    //        Debug.Log("更新相机右边界:" + maxX);
    //}

    public void updateFollowed(GameObject followed)
    {
        m_followed = followed;
    }
}
