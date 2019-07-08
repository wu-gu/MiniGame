using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private GameObject m_girl;
    private SpriteRenderer m_doorRenderer;
    public float alpha;
    // Start is called before the first frame update
    void Start()
    {
        m_girl = GameObject.Find("Girl");
        m_doorRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(m_girl))
        {
            m_doorRenderer.color = new Color(m_doorRenderer.material.color.r, m_doorRenderer.material.color.b, m_doorRenderer.material.color.g, alpha);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(m_girl))
        {
            m_doorRenderer.color = new Color(m_doorRenderer.color.r, m_doorRenderer.color.b, m_doorRenderer.color.g, 1);
        }
    }
}
