using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager: MonoSingleton<LoadManager>
{
    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// 加载某一个画面的物体
    /// </summary>
    /// <param name="stageName">画面名</param>
    /// <returns>画面物体容器</returns>
    public  GameObjectStageContainer LoadStageGameObject(string stageName)
    {
        Debug.Log("Loading stage " + stageName);
        GameObjectStageContainer container = new GameObjectStageContainer();

        GameObject[] gameObjects = Resources.LoadAll<GameObject>(stageName);
        foreach(var obj in gameObjects)
        {
            GameObject instance =  Instantiate(obj);
            //修改实例化物体的名字为预制件的名字
            instance.name = obj.name;
            container.AddGameobject(instance);
        }
        container.StageName = stageName;
        Debug.Log("Loading stage " + stageName + " success");
        return container;
    }
}
