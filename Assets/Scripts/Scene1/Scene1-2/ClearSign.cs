using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSign : MonoBehaviour
{
    private ParticleSystem m_particleSystem;
    private CircleCollider2D m_circleCollider2D;

    void Awake()
    {
        m_particleSystem = GetComponent<ParticleSystem>();
        m_circleCollider2D = GetComponent<CircleCollider2D>();
    }

    void OnMouseDown()
    {
        m_particleSystem.Stop();
        m_particleSystem.Clear();
        m_circleCollider2D.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_particleSystem.Stop();
        m_particleSystem.Clear();
        m_circleCollider2D.enabled = false;
    }
}
