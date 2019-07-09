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

    /// <summary>
    /// 机关注册
    /// </summary>
    /// <param name="gameobjectName"></param>
    /// <param name="questBehavior"></param>
    public void RegisterQuest(string gameobjectName ,QuestBehavior questBehavior)
    {
        if (!m_questBehaviorDict.ContainsKey(gameobjectName))
        {
            Debug.Log("机关注册，机关名字为:"+gameobjectName);
            m_questBehaviorDict.Add(gameobjectName, questBehavior);
        }
    }

    /// <summary>
    /// 机关注销
    /// </summary>
    /// <param name="gameobjectName"></param>
    public void UnRegisterQuest(string gameobjectName)
    {
        if (m_questBehaviorDict.ContainsKey(gameobjectName))
        {
            m_questBehaviorDict.Remove(gameobjectName);
        }
    }

    /// <summary>
    /// 注销所有机关，用于关卡变换的时候，避免有些剩下的机关忘记注销
    /// </summary>
    public void UnRegisterAllQuest()
    {
        m_questBehaviorDict.Clear();
    }

    /// <summary>
    /// 触发机关行为
    /// </summary>
    /// <param name="gameobjectName"></param>
    public void FireQuestBehavior(string gameobjectName)
    {
        if (m_questBehaviorDict.ContainsKey(gameobjectName))
        {
            m_questBehaviorDict[gameobjectName].OnUpdate();
        }

    }


}
