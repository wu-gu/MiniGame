using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWaterWheelQuest : MonoBehaviour, QuestBehavior
{
    Vector2 pre;
    Vector2 curr;
    private Vector2 m_firstDirection;
    private Vector2 m_preDirection;
    private GameObject shadow1, shadow2, shadow3;

    void Awake()
    {
        QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
        shadow1 = GameObject.Find("Shadow1");
        shadow2 = GameObject.Find("Shadow2");
        shadow3 = GameObject.Find("Shadow3");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 touchPos = (Vector2)(transform.position);
            touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 currDirection = touchPos - (Vector2)(transform.position);

            Vector3 preDirectionVec3 = new Vector3(m_preDirection.x, m_preDirection.y, transform.position.z).normalized;
            Vector3 currDirectionVec3 = new Vector3(currDirection.x, currDirection.y, transform.position.z).normalized;

            float angle = Vector3.Angle(preDirectionVec3, currDirectionVec3);
            Vector3 normal = Vector3.Cross(preDirectionVec3, currDirectionVec3);
            //计算顺时针还是逆时针
            angle *= Mathf.Sign(Vector3.Dot(normal, transform.forward));
            transform.Rotate(new Vector3(0, 0, angle));
            m_preDirection = currDirection;     
        }
        else
        {
            this.enabled = false;
        }
    }

    public void OnUpdate()
    {
        this.enabled = true;
    }

}
