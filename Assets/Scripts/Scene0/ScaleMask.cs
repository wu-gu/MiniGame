using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MiniGame
{
    // This script controls the scale of window-shape sprite mask
    // This script is enabled only after player reached blocked point(control by PlayerController, should be seperate into TriggerController in later devlopment)
    public class ScaleMask : MonoBehaviour,QuestBehavior
    {
        // pre1, and pre2 are used to record previous touch points, used to calculate and judge scale downwards/upwards
        // Continuously update per frame
        private Touch pre1;
        private Touch pre2;
        private Vector3 originalScale;
        private GameObject door;
        private Renderer doorRenderer;
        private Renderer windowRenderer;
        private CircleCollider2D circleCollider2D;

        void Awake()
        {
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);

            // Make sure OpenDoor script of door-shape sprite mask is disabled at start(Should be seperated in later development)
            door = GameObject.Find("Door");
            doorRenderer = door.GetComponent<Renderer>();
            windowRenderer = this.GetComponent<Renderer>();
            doorRenderer.enabled = false;

            // Ensure window and door are centered at the same point(They are centered at the same point in design)
            if (windowRenderer.bounds.center.x != doorRenderer.bounds.center.x || windowRenderer.bounds.center.y != doorRenderer.bounds.center.y)
            {
                //Debug.Log("Center mis-position");
                this.transform.Translate(doorRenderer.bounds.center - windowRenderer.bounds.center);
            }
            else
                //Debug.Log("Center on point");

            originalScale = this.transform.localScale;
            circleCollider2D = GetComponent<CircleCollider2D>();
            
        }

        // Update is called once per frame
        void Update()
        {
            //Test scaling in pc platform using mouse scroll wheel
            float mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");

            if (Input.GetMouseButton(0))
                this.gameObject.layer = LayerMask.NameToLayer("Outline");
            else
                this.gameObject.layer = LayerMask.NameToLayer("Quest");

            Vector3 scaleOffset = new Vector3(mouseScrollWheel, mouseScrollWheel, mouseScrollWheel);
            Vector3 currentScale = this.transform.localScale;

            if (scaleOffset.magnitude > 0)
                circleCollider2D.enabled = false;

            if (scaleOffset.x + currentScale.x <= originalScale.x || scaleOffset.y + currentScale.y <= originalScale.y || scaleOffset.z + currentScale.z <= originalScale.z)
            {
                this.transform.localScale = originalScale;
                return;
            }
            else
            {
                this.transform.localScale += scaleOffset;
            }



            // Real usage in Android platform

            // Nothing to process when one or less finger is detected
            //if (Input.touchCount <= 1)
            //{
            //    // Trick is scale back, once the player loses control of the trick and has not reached end point
            //    this.transform.localScale = originalScale;
            //    circleCollider2D.enabled = true;
            //    this.gameObject.layer = LayerMask.NameToLayer("Quest");
            //    return;
            //}
            //else
            //{
            //    this.gameObject.layer = LayerMask.NameToLayer("Outline");
            //    // Process mask scaling when two fingers are detected, more than two: process as two

            //    Touch curr1 = Input.GetTouch(0);
            //    Touch curr2 = Input.GetTouch(1);

            //    // Ignore the process when the second finger is just right on touch, in case of mis-process
            //    if (curr2.phase == TouchPhase.Began)
            //    {
            //        pre2 = curr2;
            //        pre1 = curr1;
            //        return;
            //    }

            //    // Process when the user input is clearly the act of scaling
            //    else
            //    {
            //        // Disable collider component when scale starts, or player will be pushed out 
            //        circleCollider2D.enabled = false;

            //        float preDist = Vector2.Distance(pre1.position, pre2.position);
            //        float currDist = Vector2.Distance(curr1.position, curr2.position);

            //        // Scale distance
            //        float offset = currDist - preDist;

            //        // Calculate scale offset and new scale vector per frame
            //        float scaleFactor = offset / 100f;
            //        Vector3 localScale = this.transform.localScale;
            //        Vector3 scale = new Vector3(localScale.x + scaleFactor, localScale.y + scaleFactor, localScale.z + scaleFactor);

            //        // Certain limitation of smallest scale size
            //        if (scale.x >= originalScale.x && scale.y >= originalScale.y && scale.z >= originalScale.z)
            //            transform.localScale = scale;
            //        pre1 = curr1;
            //        pre2 = curr2;

            //    }
            //}

            float frameRadius = doorRenderer.bounds.extents.x;
            float windowRadius = windowRenderer.bounds.extents.x; // extents.x == extents.y == window radius

            // Detect whether window mask covers the whole area of frame_
            if (frameRadius <= windowRadius)
            {
                //Debug.Log("Reach point");
                this.gameObject.layer = LayerMask.NameToLayer("Quest");
                QuestController.Instance.FireQuestBehavior(door.ToString());
                QuestController.Instance.UnRegisterQuest(gameObject.ToString());
                this.enabled = false;
                // Current effect of reaching finish point, should be replaced or refined in later development
                //AudioSource music = GetComponent<AudioSource>();
                //music.Play();
            }
        }

        public void OnUpdate()
        {
            this.enabled = true;
        }
    }
}
