using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public GameObject backgroundL, backgroundR;
    private float minX, maxX;
    private Camera m_mainCam;
    private GameObject m_player;

    void Awake()
    {
        Camera.main.aspect = 16.0f / 9;
        m_mainCam = Camera.main;
        float orthoHorizontal = m_mainCam.aspect * m_mainCam.orthographicSize; // camera viewport: aspect = widht/height
        minX = backgroundL.GetComponent<Renderer>().bounds.min.x + orthoHorizontal;
        maxX = backgroundR.GetComponent<Renderer>().bounds.max.x - orthoHorizontal;

        m_player = GameObject.FindGameObjectWithTag("Player");
         
    }

    // Update is called once per frame
    void Update()
    {
        float nextX = Mathf.Clamp(m_player.transform.position.x, minX, maxX);
        m_mainCam.transform.position = new Vector3(nextX, m_mainCam.transform.position.y, m_mainCam.transform.position.z);
    }
}
