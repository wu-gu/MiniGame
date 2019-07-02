using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeShadows : MonoBehaviour
{
    private Animator anim;
    private int shadowState;

    void Awake()
    {
        anim = GetComponent<Animator>();
        shadowState = Animator.StringToHash("ShadowState");
        anim.speed = 1.0f;
    }

    public void SetUpNewShadowState(int state)
    {
        AnimatorStateInfo animatorInfo;
        animatorInfo = anim.GetCurrentAnimatorStateInfo(0);
        anim.SetInteger(shadowState, state);
    }
}
