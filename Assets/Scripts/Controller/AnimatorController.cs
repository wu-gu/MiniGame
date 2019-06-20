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
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 创建动画控制委托，后面创建播放动画方法，然后调用就可以直接用委托的对象来调用
    /// 例如:animationHandler = xxx
    /// </summary>
    public delegate void AnimatorHandler();

    /// <summary>
    /// 创建动画对象
    /// </summary>
    Animator animator;

    /// <summary>
    /// 创建各个动画
    /// </summary>
    public List<RuntimeAnimatorController> animatorClips;

    /// <summary>
    /// 实例化委托
    /// </summary>
    public AnimatorHandler animatorHandler;

    /// <summary>
    /// 添加动画，这个给开发者用
    /// </summary>
    public void AddAnimator(RuntimeAnimatorController controller)
    {
        animatorClips.Add(controller);
    }

    /// <summary>
    /// 判断是否存在指定动画
    /// </summary>
    public bool ContainAnimatorClip(RuntimeAnimatorController controller)
    {
        return animatorClips.Contains(controller);
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    public void PlayAnimator(string animatorName)
    {
        animator.Play(animatorName);
    }

    /// <summary>
    /// 播放
    /// </summary>
    private void Update()
    {
        if (animatorHandler != null)
        {
            animatorHandler();
        }
    }
}
