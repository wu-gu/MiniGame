using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectStageContainer
{
    private Dictionary<string, GameObject> m_stageGameobjects = new Dictionary<string, GameObject>();

    public string StageName // This is your property
    { get; set; }

    public void AddGameobject(GameObject obj)
    {
        if (!m_stageGameobjects.ContainsValue(obj))
        {
            m_stageGameobjects.Add(obj.name, obj);
        }
    }

    public GameObject GetGameObjectByName(string name)
    {
        if (!m_stageGameobjects.ContainsKey(name)){
            return m_stageGameobjects[name];
        }
        return null;
    }

    public void DestoryAll()
    {
        foreach(var obj in m_stageGameobjects)
        {            
            //此处如果之前已经销毁了游戏对象，然后在这里又销毁会有问题
            if (obj.Value == null)
                Debug.Log(obj.Key+" already destroyed");
            else GameObject.Destroy(obj.Value);
        }
    }
}
