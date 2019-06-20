using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoSingleton<AnimationController>
{
    /// <summary>
    /// 继承单例
    /// </summary>
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 创建动画控制委托，后面创建播放动画方法，然后调用就可以直接用委托的对象来调用
    /// 例如:animationHandler = xxx
    /// </summary>
    public delegate void AnimationHandler();

    /// <summary>
    /// 创建动画对象
    /// </summary>
    Animation animation;

    /// <summary>
    /// 创建各个动画
    /// </summary>
    public List<AnimationClip> animationClips;

    /// <summary>
    /// 实例化委托
    /// </summary>
    public AnimationHandler animationHandler;

    /// <summary>
    /// 添加动画，这个给开发者用
    /// </summary>
    public void AddAnimation(AnimationClip clip)
    {
        animationClips.Add(clip);
    }

    /// <summary>
    /// 判断是否存在指定动画
    /// </summary>
    public bool ContainAnimationClip(AnimationClip clip)
    {
        return animationClips.Contains(clip);
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    public void PlayAnimation(string animationName)
    {
        animation.Play(animationName);
    }

    private void Start()
    {
        animation = GetComponent<Animation>();
    }

    /// <summary>
    /// 播放
    /// </summary>
    private void Update()
    {
        if (animationHandler != null)
        {
            animationHandler();
        }
    }
}
