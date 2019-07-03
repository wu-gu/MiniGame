using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLargerQuest : MonoBehaviour,QuestBehavior
{
    private float m_originDistance;
    private bool m_isSuccess = false;

    public void OnUpdate()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Android端
        if (!m_isSuccess && Input.touchCount == 2) 
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch2.phase == TouchPhase.Began)
            {
                m_originDistance = Vector2.Distance(touch1.position, touch2.position);
                return;
            }

            if (touch1.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved)
            {
                float newDistance = Vector2.Distance(touch1.position, touch2.position);
                if (newDistance < m_originDistance)
                {
                    m_isSuccess = true;
                }
            }
        }
    }

}
