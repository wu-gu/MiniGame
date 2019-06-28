using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestResourceDepend : MonoBehaviour
{
    public GameObject destObject;

    void Awake()
    {
        //GameObject.Destroy();
        Debug.Log(gameObject.ToString()+"Awake");
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gameObject.ToString() + "Start");
        if (destObject == null)
            Debug.Log("目标物体没有加载");
        Debug.Log("目标物体"+destObject.ToString());
        //destObject = Instantiate(destObject);
        //Instantiate(destObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
