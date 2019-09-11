using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitesController : MonoBehaviour
{
    // Start is called before the first frame update
    public int targetSuccess = 2;
    private int m_currSuccess = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddSuccess(GameObject kite)
    {
        m_currSuccess++;
        // 风筝飞走
        kite.GetComponent<Animator>().SetTrigger("FlyOut");
        if (m_currSuccess == targetSuccess)
        {
            Debug.Log("两个谜语都已经猜出来");
            //摄像机跟随风筝，可以做摄像机动画或者使用CameraController
            CameraController cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
            cameraController.updateFollowed(kite);
            cameraController.enabled = true;
            //人物位置变动
        }
    }

}
