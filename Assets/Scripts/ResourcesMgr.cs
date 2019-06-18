using UnityEngine;
using System.Collections;

// 通过用户传递进来的枚举值, 自动的分析出该枚举值对应的资源在哪个路径下.
public class ResourcesMgr : MonoSingleton<ResourcesMgr>
{
    // 面向用户的方法:  方法名<T>指定类型
    /// <summary>
    /// 返回用户需要的资源.
    /// </summary>
    /// <param name="enumName">Enum name.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public T Load<T>(object enumName) where T : Object
    {
        // 获取枚举类型的字符串形式
        string enumType = enumName.GetType().Name;

        //空的字符串
        string filePath = string.Empty;

        switch (enumType)
        {
            case "Items":
                {
                    filePath = "Audios/Items/" + enumName.ToString();
                    break;
                }
            case "Background":
                {
                    filePath = "Audios/Main/" + enumName.ToString();
                    break;
                }
            case "Events":
                {
                    filePath = "Audios/Events/" + enumName.ToString();
                    break;
                }
            default:
                {
                    break;
                }
        }
        return Resources.Load<T>(filePath);
    }
}
