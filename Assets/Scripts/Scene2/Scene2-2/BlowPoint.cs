using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowPoint : MonoBehaviour
{
    private Animator m_animator;
    private int isBlowing;

    void Start()
    {
        m_animator = GameObject.Find("Lanterns").GetComponent<Animator>();
        isBlowing = Animator.StringToHash("IsBlowing");
    }

    void OnCollisionEnter2D()
    {
        m_animator.SetBool(isBlowing, true);
    }
}
