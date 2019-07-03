using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public GameObject backgroundL, backgroundR;
    private float minX, maxX;
    float orthoHorizontal;
    private Camera m_mainCam;
    private GameObject m_player;

    public float followSpeed = 0.1f;

    void Awake()
    {
        //Camera.main.aspect = 16.0f / 9;
        // camera viewport: aspect = widht/height
        //minX = backgroundL.GetComponent<Renderer>().bounds.min.x + orthoHorizontal;
        //maxX = backgroundR.GetComponent<Renderer>().bounds.max.x - orthoHorizontal;

        if (backgroundL != null && backgroundR != null)
        {
            minX = backgroundL.transform.position.x + orthoHorizontal;
            maxX = backgroundR.transform.position.x - orthoHorizontal;
        }
        m_mainCam = Camera.main;
        orthoHorizontal = m_mainCam.aspect * m_mainCam.orthographicSize;
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (backgroundL != null && backgroundR != null)
        {
            Debug.DrawRay(new Vector3(minX, m_mainCam.transform.position.y, m_mainCam.transform.position.z), Vector3.down, Color.blue);
            Debug.DrawRay(new Vector3(maxX, m_mainCam.transform.position.y, m_mainCam.transform.position.z), Vector3.down, Color.green);
            float destX = Mathf.Clamp(m_player.transform.position.x, minX, maxX);
            float nextX = Mathf.MoveTowards(m_mainCam.transform.position.x, destX, followSpeed);
            m_mainCam.transform.position = new Vector3(nextX, m_mainCam.transform.position.y, m_mainCam.transform.position.z);
        }
    }

    public void UpdateBackgounrdLeft(GameObject left)
    {
        backgroundL = left;
        minX = backgroundL.transform.position.x + orthoHorizontal;
    }

    public void UpdateBackgounrdRight(GameObject right)
    {
        backgroundR = right;
        maxX = backgroundR.transform.position.x - orthoHorizontal;
    }
}
