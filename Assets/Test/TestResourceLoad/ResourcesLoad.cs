using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] gameObjects =  Resources.LoadAll<GameObject>("Test");
        foreach(var obj in gameObjects)
        {
            //Resources.Load();
            Debug.Log("加载资源"+obj.name);
            //obj.SetActive(true);
            Instantiate(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
