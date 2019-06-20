using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoSingleton<QuestController>
{
    //string为gameobject的name
    private Dictionary<string, QuestBehavior> m_questBehaviorDict = new Dictionary<string, QuestBehavior>();
    //可以考虑机关之间的关系

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 机关注册
    /// </summary>
    /// <param name="gameobjectName"></param>
    /// <param name="questBehavior"></param>
    public void RegisterQuest(string gameobjectName ,QuestBehavior questBehavior)
    {
        if (!m_questBehaviorDict.ContainsKey(gameobjectName))
        {
            m_questBehaviorDict.Add(gameobjectName, questBehavior);
        }
    }

    public void UnRegisterQuest(string gameobjectName)
    {
        if (m_questBehaviorDict.ContainsKey(gameobjectName))
        {
            m_questBehaviorDict.Remove(gameobjectName);
        }
    }

    /// <summary>
    /// 触发机关行为
    /// </summary>
    /// <param name="gameobjectName"></param>
    public void FireQuestBehavior(string gameobjectName)
    {
        m_questBehaviorDict[gameobjectName].OnUpdate();
    }


}
