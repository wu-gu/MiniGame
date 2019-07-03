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

    public  GameObjectStageContainer LoadStageGameObject(string stageName)
    {
        Debug.Log("Loading stage " + stageName);
        GameObjectStageContainer container = new GameObjectStageContainer();

        GameObject[] gameObjects = Resources.LoadAll<GameObject>(stageName);
        foreach(var obj in gameObjects)
        {
            GameObject instance =  GameObject.Instantiate(obj);
            instance.name = obj.name;
            container.AddGameobject(instance);
            //m_container = container;
        }
        container.StageName = stageName;
        Debug.Log("Loading stage " + stageName + " success");
        return container;
    }
}
