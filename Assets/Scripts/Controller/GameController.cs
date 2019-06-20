using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniGame
{

    public class GameController : MonoSingleton<GameController>
    {
        public Camera mainCamera;

        protected Scene m_CurrentZoneScene;
        //private List<AnimationClip> animationClips;
        public AnimationClip[] scenesAnimationClips = new AnimationClip[5];
        public RuntimeAnimatorController[] scenesAnimatorControllers = new RuntimeAnimatorController[5];
        private SelfMonoBehaviour[] selfMonoBehaviours;

        public TransitionDestination initialSceneTransitionDestination;
        protected TransitionDestination.DestinationTag m_ZoneRestartDestinationTag;
        protected bool m_Transitioning;
        public static bool Transitioning
        {
            get { return Instance.m_Transitioning; }
        }

        void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            m_CurrentZoneScene = SceneManager.GetActiveScene();

            //switch (m_CurrentZoneScene.name)
            //{
            //    case "Scene0":
            //    {
            //        AnimationController.Instance.AddAnimation(scenesAnimationClips[0]);
            //        AnimationController.Instance.PlayAnimation(scenesAnimationClips[0].name);
            //            //AnimatorController.Instance.AddAnimator(scenesAnimatorControllers[0]);
            //            //AnimatorController.Instance.PlayAnimator()
            //        Debug.Log(scenesAnimationClips[0].name);
            //        break;
            //    }
            //    case "Scene1":
            //    {
            //        AnimationController.Instance.AddAnimation(scenesAnimationClips[1]);
            //        AnimationController.Instance.PlayAnimation(scenesAnimationClips[1].name);
            //        Debug.Log(scenesAnimationClips[1].name);
            //        break;
            //    }
            //    default:
            //        break;
            //}
            //可以在此初始化输入控制等其他控制器

            mainCamera = Camera.main;

        }

        //void Start()
        //{
        //    selfMonoBehaviours = gameObject.GetComponents<SelfMonoBehaviour>();
        //}


        void Update()
        {
            //update时序控制
            InputController.Instance.OnUpdate();

      //      var count = selfMonoBehaviours.Length;
		    //for (var i = 0; i < count; i++)
		    //{
			   // selfMonoBehaviours[i].DoUpdate();
		    //}
      //      Debug.Log("拥有组件" + selfMonoBehaviours[0].name);
        }

        /// <summary>
        /// 设置场景为活动场景
        /// 必须要保证：目标场景被加载后，才可以正确设置活动状态
        /// </summary>
        IEnumerator Transition(string newSceneName, bool resetInputValues, TransitionDestination.DestinationTag destinationTag, TransitionPoint.TransitionType transitionType = TransitionPoint.TransitionType.DifferentZone)
        {
            m_Transitioning = true;

            yield return SceneManager.LoadSceneAsync(newSceneName); //等待场景加载完毕后，再向下执行;异步加载，方式：附加
            TransitionDestination entrance = GetDestination(destinationTag);
            SetEnteringGameObjectLocation(entrance);
            SetupNewScene(transitionType, entrance); //设置场景为活动场景
            if (entrance != null)
                entrance.onReachDestination.Invoke();

            m_Transitioning = false;           
        }


        public static void RestartZone()
        {
            Instance.StartCoroutine(Instance.Transition(Instance.m_CurrentZoneScene.name, false, Instance.m_ZoneRestartDestinationTag, TransitionPoint.TransitionType.DifferentZone));
        }

        public static void TransitionToScene(TransitionPoint transitionPoint)
        {
            Instance.StartCoroutine(Instance.Transition(transitionPoint.newSceneName, transitionPoint.resetInputValuesOnTransition, transitionPoint.transitionDestinationTag, transitionPoint.transitionType));
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
            if (entrance == null)
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