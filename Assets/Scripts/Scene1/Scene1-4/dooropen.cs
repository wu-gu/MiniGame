using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour, QuestBehavior
{
    private Renderer dooropen_Renderer;
    float scaleFactor = 0.15f;
    private Camera cam;
    private Vector3 open_originalScale;


    public void OnUpdate()
    {
        dooropen_Renderer.enabled = true;
        this.enabled = true;
    }
    void Awake()
    {
        QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
        cam = Camera.main;
        dooropen_Renderer = GetComponent<Renderer>();
        open_originalScale = this.transform.localScale;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localScale = transform.localScale;
        transform.localScale = new Vector3(localScale.x + scaleFactor, localScale.y + scaleFactor, localScale.z + scaleFactor);

        Vector3 doorCentral = transform.position;
        float doorx = dooropen_Renderer.bounds.extents.x;
        float doory = dooropen_Renderer.bounds.extents.y;

        float orthoVertical = cam.orthographicSize;
        float orthoHorizontal = cam.aspect * orthoVertical; // camera viewport: aspect = widht/height
        Vector3 camCentral = cam.transform.position;
        Vector3 camMax = new Vector3(camCentral.x + orthoHorizontal, camCentral.y + orthoVertical, camCentral.z);
        Vector3 camMin = new Vector3(camCentral.x - orthoHorizontal, camCentral.y - orthoVertical, camCentral.z);

        if (doory >= Vector3.Distance(camMax, doorCentral) && doory >= Vector3.Distance(camMin, doorCentral))
        {
            //Debug.Log("Transition timing reached");
            scaleFactor = 0f;
            QuestController.Instance.UnRegisterQuest(gameObject.ToString());
            this.enabled = false;
        }
    }
}
