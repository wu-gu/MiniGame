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

    public void AddSuccess()
    {
        m_currSuccess++;
        if (m_currSuccess == targetSuccess)
        {
            //机关成功了
        }
    }

}
