using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class doormask : MonoBehaviour, QuestBehavior
    {
        private GameObject dooropenmask;
        private GameObject dooropen;
        private Renderer dooropenmask_Renderer;
        private Renderer dooropen_Renderer;
        private Vector3 mask_originalScale;
        private Vector3 open_originalScale;
        void Awake()
        {
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
            dooropenmask = GameObject.Find("dooropenmask");
            dooropen = GameObject.Find("dooropen");
            dooropenmask_Renderer = dooropenmask.GetComponent<Renderer>();
            dooropen_Renderer = dooropen.GetComponent<Renderer>();
            dooropen_Renderer.enabled = false;
            mask_originalScale = dooropenmask.transform.localScale;
            open_originalScale = dooropen.transform.localScale;
        }
        void Update()
        {
            float mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
            Vector3 scaleOffset = new Vector3(mouseScrollWheel, 0, 0);
            Vector3 currentScale = dooropenmask.transform.localScale;
            if (scaleOffset.x + currentScale.x <= mask_originalScale.x )
            {
                dooropenmask.transform.localScale = mask_originalScale;
                return;
            }
            else
            {
                dooropenmask.transform.localScale += scaleOffset;
            }
            if(dooropenmask.transform.localScale.x>= open_originalScale.x)
            {
                QuestController.Instance.FireQuestBehavior(dooropen.ToString());
                //dooropenmask_Renderer.enabled = false;
                QuestController.Instance.UnRegisterQuest(gameObject.ToString());
                this.enabled = false;
            }
        }
        // Start is called before the first frame update
        public void OnUpdate()
        {
            this.enabled = true;
        }
    }
}