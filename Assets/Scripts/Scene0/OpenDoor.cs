using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    // This script controls the effect of door open: in fact is the scale of door-shape sprite mask
    // This script is only enabled when ScaleMask script end trigger detected
    public class OpenDoor : MonoBehaviour, QuestBehavior
    {
        private Camera cam;
        private Renderer doorRenderer;
        float scaleFactor = 0.15f;

        // Start is called before the first frame update
        void Start()
        {
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
            cam = Camera.main;
            doorRenderer = GetComponent<Renderer>();
            this.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            // Door open action, execute with no start trigger: controlled by dynamic script enable and disable
            Vector3 localScale = transform.localScale;
            transform.localScale = new Vector3(localScale.x + scaleFactor * 2, localScale.y + scaleFactor, localScale.z + scaleFactor);

            // Detect the need of scene transition
            // doorMax, doorMin, doorRadius, doorCentral: the maximal point, minimal point, mask radius, and central position of the door-shape sprite mask
            Vector3 doorMax = doorRenderer.bounds.max;
            Vector3 doorMin = doorRenderer.bounds.min;
            Vector3 doorCentral = transform.position;
            float doorRadius = doorRenderer.bounds.extents.x; // extents.x == extents.y == max.x - center.x == max.y - center.y


            // orthoVertical, half height of camera viewport, orthoHorizontal, half width of camera viewport
            // camCentral, camMax, camMin: the central position, maximal point, and minimal point of the camera viewport
            float orthoVertical = cam.orthographicSize;
            float orthoHorizontal = cam.aspect * orthoVertical; // camera viewport: aspect = widht/height
            Vector3 camCentral = cam.transform.position;
            Vector3 camMax = new Vector3(camCentral.x + orthoHorizontal, camCentral.y + orthoVertical, camCentral.z);
            Vector3 camMin = new Vector3(camCentral.x - orthoHorizontal, camCentral.y - orthoVertical, camCentral.z);

            // Transition trigger: door mask outreach camera viewport, transition start
            if (doorRadius >= Vector3.Distance(camMax, doorCentral) && doorRadius >= Vector3.Distance(camMin, doorCentral))
            {
                //Debug.Log("Transition timing reached");
                scaleFactor = 0f;
                QuestController.Instance.UnRegisterQuest(gameObject.ToString());
                this.enabled = false;
                GameObject.Find("TransitionStart").GetComponent<TransitionPoint>().Transition(); // External call usage of  transition
            }

        }

        public void OnUpdate()
        {
            this.enabled = true;
            this.transform.localScale = Vector3.one;
            doorRenderer.enabled = true;
        }
    }
}
