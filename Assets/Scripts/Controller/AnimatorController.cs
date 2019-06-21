using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoSingleton<AnimatorController>
{
    /// <summary>
    /// 继承单例
    /// </summary>
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 定义GameObject数组
    /// </summary>
    public GameObject[] gameObjects;

    /// <summary>
    /// 寻找场景中所有的GameObject
    /// </summary>
    private void FindAllGameObjectsInScene()
    {
        //关键代码，获取所有gameObject元素给数组
        gameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        //foreach(GameObject obj in gameObjects)
        //{
        //    Debug.Log(obj.gameObject.name);
        //}
    }

    /// <summary>
    /// 创建动画对象
    /// </summary>
    Animator animator;

    /// <summary>
    /// 创建各个动画
    /// </summary>
    public List<Animator> animators;

    /// <summary>
    /// 添加动画，这个给开发者用
    /// </summary>
    public void AddAnimator()
    {
        foreach (GameObject obj in gameObjects)
        {
            if(obj.GetComponent<Animator>() != null)
            {
                animators.Add(obj.GetComponent<Animator>());
            }
        }
    }

    /// <summary>
    /// 判断是否存在指定动画控制器
    /// </summary>
    public bool ContainAnimatorClip(Animator controller)
    {
        return animators.Contains(controller);
    }

    public void FindAnimator(string animatorName)
    {
        for (int i = 0; i < animators.Count; i++)
        {
            if (animators[i].name == animatorName) animator = animators[i];
        }
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    public void PlayAnimation(string animatorName, string animationClipName)
    {
        FindAnimator(animatorName);
        animator.SetBool(animationClipName, true);
    }

    /// <summary>
    /// 停止动画
    /// </summary>
    public void StopAnimation(string animatorName, string animationClipName)
    {
        FindAnimator(animatorName);
        animator.SetBool(animationClipName, false);
    }

    /// <summary>
    /// 混合动画用
    /// </summary>
    public void SetLayerWeight(int layer, float weight)
    {
        animator.SetLayerWeight(layer, weight);
    }

    /// <summary>
    /// 播放
    /// </summary>
    private void Update()
    {
        //if (animatorHandler != null)
        //{
        //    animatorHandler();
        //}
    }

    private void Start()
    {
        FindAllGameObjectsInScene();
        AddAnimator();
    }
}
