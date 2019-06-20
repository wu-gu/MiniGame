using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Ecin
{
    // This script is used to detect transition need: collider triggered/external called(currently used)
    [RequireComponent(typeof(Collider2D))]
    public class TransitionPoint : MonoBehaviour
    {
        public enum TransitionType
        {
            DifferentZone, DifferentNonGameplayScene, SameScene,
        }

        public enum TransitionWhen
        {
            ExternallCall, InteractPressed, OnTriggerEnter,
        }

        [Tooltip("This is the game object that will transition. For example, the player. ")]
        public GameObject transitioningGameObject;
        [Tooltip("Whether the transition will be within this scene, to a different zone or a non-gameplay scene. ")]
        public TransitionType transitionType;
        [SceneName]
        public string newSceneName;
        [Tooltip("The tag of the TransitionDestination script in the scene being transitioned to. ")]
        public TransitionDestination.DestinationTag transitionDestinationTag;
        [Tooltip("The destination in this scene that the transitioning game object will be teleported. ")]
        public TransitionPoint destinationTransform;
        [Tooltip("What should trigger the transition to start. ")]
        public TransitionWhen transitionWhen;
        [Tooltip("The player will lose control when the transition happens but should the axis and button values reset to the default when control is lost. ")]
        public bool resetInputValuesOnTransition = false; // The only player input is to touch the screen, so no need to clear for now
        [Tooltip("Is this transition only possible with specific items in the inventory. ")]
        public bool requiresInventoryCheck = false; // No inventory settings for now
        // [Tooltip("The inventory to be checked. ")]
        // public InventoryController inventoryController;
        // [Tooltip("The required items. ")]
        // public InventoryController.InventoryChecker inventoryCheck;

        bool m_TransitioningGameObjectPresent;

        // Start is called before the first frame update
        void Start()
        {
            if (transitionWhen == TransitionWhen.ExternallCall)
                m_TransitioningGameObjectPresent = true;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject == transitioningGameObject)
            {
                m_TransitioningGameObjectPresent = true;

                // No ScreenFader for now
                // if (ScreenFader.IsFading || SceneController.Transitioning)
                // return;

                if (SceneController.Transitioning)
                    return;

                if (transitionWhen == TransitionWhen.OnTriggerEnter)
                    TransitionInternal();
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject == transitioningGameObject)
                m_TransitioningGameObjectPresent = false;
        }

        // Update is called once per frame
        void Update()
        {
            // No ScreenFader and SceneController for now
            // if(ScreenFader.IsFading || SceneController.Transitioning)
            // return;
            if (SceneController.Transitioning)
                return;

            if (!m_TransitioningGameObjectPresent)
                return;

            // No InteractPressed settings for now 
            // if(transitionWhen == TransitionWhen.InteractPressed)
            // {
                // if (PlayerInput.Instance.Interact.Down)
                    // TransitionInternal();
            // }
        }

        void TransitionInternal()
        {
            // No InventroyCheck settings for now
            // if(requiresInventoryCheck)
            // {
                // if (!inventoryCheck.CheckInventory(inventoryController))
                    // return;
            // }

            // No same scene transition settings for now
            // if(transitionType == TransitionType.SameScene)
            // {
                // GameObjectTeleporter.Teleport(transitioningGameObject, destinationTransform.transform);
            // }
            // else
            // {
                // SceneController.TransitionToScene(this);
            // }
            SceneController.TransitionToScene(this);
        }

        public void Transition()
        {
            if (!m_TransitioningGameObjectPresent)
                return;

            if (transitionWhen == TransitionWhen.ExternallCall)
                TransitionInternal();
        }
    }
}
