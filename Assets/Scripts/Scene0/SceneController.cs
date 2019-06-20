using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Ecin
{
    // This script is used to control transition between scenes.
    // Stable version, possiblly no need to refine current functions, may have future new needs to add
    public class SceneController : MonoBehaviour
    {
        // Singleton pattern
        public static SceneController Instance
        {
            get
            {
                if (instance != null)
                    return instance;

                instance = FindObjectOfType<SceneController>();

                if (instance != null)
                    return instance;

                Create();

                return instance;
            }
        }

        public static bool Transitioning
        {
            get { return Instance.m_Transitioning; }
        }

        protected static SceneController instance;

        public static SceneController Create()
        {
            GameObject sceneControllerGameObject = new GameObject("SceneController");
            instance = sceneControllerGameObject.AddComponent<SceneController>();

            return instance;
        }

        public TransitionDestination initialSceneTransitionDestination;

        protected Scene m_CurrentZoneScene;
        protected TransitionDestination.DestinationTag m_ZoneRestartDestinationTag;
        // protected PlayerInput m_playerInput; // Current player input == mouse down, no need to setup complex techniques to clear for now
        protected bool m_Transitioning;

        void Awake()
        {
            if(Instance!=this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            if (initialSceneTransitionDestination != null)
            {
                SetEnteringGameObjectLocation(initialSceneTransitionDestination);
                initialSceneTransitionDestination.onReachDestination.Invoke();
            }
            else
            {
                m_CurrentZoneScene = SceneManager.GetActiveScene();
                m_ZoneRestartDestinationTag = TransitionDestination.DestinationTag.A;
            }
        }

        public static void RestartZone()
        {
            Instance.StartCoroutine(Instance.Transition(Instance.m_CurrentZoneScene.name, false, Instance.m_ZoneRestartDestinationTag, TransitionPoint.TransitionType.DifferentZone));
        }

        public static void TransitionToScene(TransitionPoint transitionPoint)
        {
            Instance.StartCoroutine(Instance.Transition(transitionPoint.newSceneName, transitionPoint.resetInputValuesOnTransition, transitionPoint.transitionDestinationTag, transitionPoint.transitionType));
        }

        protected IEnumerator Transition(string newSceneName, bool resetInputValues, TransitionDestination.DestinationTag destinationTag, TransitionPoint.TransitionType transitionType = TransitionPoint.TransitionType.DifferentZone)
        {
            m_Transitioning = true;

            yield return SceneManager.LoadSceneAsync(newSceneName);
            TransitionDestination entrance = GetDestination(destinationTag);
            SetEnteringGameObjectLocation(entrance);
            SetupNewScene(transitionType, entrance);
            if (entrance != null)
                entrance.onReachDestination.Invoke();

            m_Transitioning = false;
        }

        protected TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
        {
            TransitionDestination[] entrances = FindObjectsOfType<TransitionDestination>();
            for (int i = 0; i < entrances.Length; i++)
            {
                if (entrances[i].destinationTag == destinationTag)
                    return entrances[i];
            }
            Debug.LogWarning("No entrance was found with the " + destinationTag + ".tag. ");
            return null;
        }

        protected void SetEnteringGameObjectLocation(TransitionDestination entrance)
        {
            if (entrance == null)
            {
                Debug.LogWarning("Entering Transform's location has not been set yet. ");
                return;
            }
            Transform entranceLocation = entrance.transform;
            Transform enteringTransform = entrance.transitioningGameObject.transform;
            enteringTransform.position = entranceLocation.position;
            enteringTransform.rotation = entranceLocation.rotation;
        }

        protected void SetupNewScene(TransitionPoint.TransitionType transitionType, TransitionDestination entrance)
        {
            if(entrance==null)
            {
                Debug.LogWarning("Restart information has not been set. ");
                return;
            }

            if (transitionType == TransitionPoint.TransitionType.DifferentZone)
                SetZoneStart(entrance);
        }

        protected void SetZoneStart(TransitionDestination entrance)
        {
            m_CurrentZoneScene = entrance.gameObject.scene;
            m_ZoneRestartDestinationTag = entrance.destinationTag;
        }
    }
}
