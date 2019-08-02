using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiteQuest : MonoBehaviour,QuestBehavior
{
    private GameObject m_camera;

    public float animationTime = 3;
    [Tooltip("摄像机目标Size")]
    public float targetOrthoSize= 0.95f;
    private float m_currTime = 0;

    private Vector2 m_cameraOriginPos;
    private float m_cameraOrthoSize;
    public int cameraMode = 0;//0为默认，1为放大过程，2中间态，3为返回过程

    // Start is called before the first frame update
    void Start()
    {
        m_camera = Camera.main.gameObject;
        QuestController.Instance.RegisterQuest(gameObject.name, this);
        this.enabled = false;
    }

    public void OnUpdate()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.touches[0];
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (touch.phase == TouchPhase.Began)
            {

            }
        }
        this.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_currTime > animationTime)
        {
            switch (cameraMode)
            {
                case 1:
                    cameraMode = 2;
                    this.enabled = false;
                    break;
                case 3:
                    cameraMode = 0;
                    this.enabled = false;
                    break;
            }
            m_currTime = 0;
            return;
        }
        if (cameraMode == 1)
        {
            m_currTime += Time.deltaTime;
            //Vector2 nextPos = Vector2.MoveTowards(m_camera.transform.position, transform.position, 0.2f);
            //float nextSize = Mathf.MoveTowards(m_camera.GetComponent<Camera>().orthographicSize,targetOrthoSize, 0.2f);
            Vector2 nextPos = Vector2.Lerp(m_cameraOriginPos, transform.position, m_currTime / animationTime);
            float nextSize = Mathf.Lerp(m_cameraOrthoSize, targetOrthoSize, m_currTime / animationTime);
            m_camera.transform.position = new Vector3(nextPos.x, nextPos.y, m_camera.transform.position.z);
            m_camera.GetComponent<Camera>().orthographicSize = nextSize;
        }
        else if(cameraMode == 3)
        {
            m_currTime += Time.deltaTime;
            //Vector2 nextPos = Vector2.MoveTowards(m_camera.transform.position, transform.position, 0.2f);
            //float nextSize = Mathf.MoveTowards(m_camera.GetComponent<Camera>().orthographicSize,targetOrthoSize, 0.2f);
            Vector2 nextPos = Vector2.Lerp(transform.position, m_cameraOriginPos, m_currTime / animationTime);
            float nextSize = Mathf.Lerp(targetOrthoSize, m_cameraOrthoSize, m_currTime / animationTime);
            m_camera.transform.position = new Vector3(nextPos.x, nextPos.y, m_camera.transform.position.z);
            m_camera.GetComponent<Camera>().orthographicSize = nextSize;
        }

    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            m_currTime = 0;
            switch (cameraMode)
            {
                case 0:
                    cameraMode = 1;
                    m_cameraOriginPos = m_camera.transform.position;
                    m_cameraOrthoSize = m_camera.GetComponent<Camera>().orthographicSize;
                    break;
                case 2:
                    cameraMode = 3;
                    break;
            }
            this.enabled = true;
        }
    }

}
