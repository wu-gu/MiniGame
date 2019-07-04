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
        //用于选择关卡时
        public List<string> allLevelName = new List<string>();//一关对应一个名字
        public int highestProgress;//玩家的最大进度，为allLevelName中的索引，即第几关

        //private List<AnimationClip> animationClips;
        //public AnimationClip[] scenesAnimationClips = new AnimationClip[5];
        //public RuntimeAnimatorController[] scenesAnimatorControllers = new RuntimeAnimatorController[5];
        //private SelfMonoBehaviour[] selfMonoBehaviours;

        //public TransitionDestination initialSceneTransitionDestination;
        protected TransitionDestination.DestinationTag m_ZoneRestartDestinationTag;
        protected bool m_Transitioning;
        public static bool Transitioning
        {
            get { return Instance.m_Transitioning; }
        }


        //与画面相关
        [Tooltip("是否动态加载场景")]
        //主要用于判断一开始的画面是否是需要动态加载的
        public bool m_IsSetupDynamicLoad = false;


        private LinkedList<GameObjectStageContainer> m_stageGameObjectsContainerList = new LinkedList<GameObjectStageContainer>();
        private LinkedListNode<GameObjectStageContainer> m_currStageContainer;
        //private GameObjectStageContainer m_currStageContainer;
        //private GameObjectStageContainer m_nextStageContainer;


        void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            allLevelName.Add("Level0");
            highestProgress = 0;

            m_CurrentZoneScene = SceneManager.GetActiveScene();

            if(!m_CurrentZoneScene.name.Equals("Start"))
                //加载该关卡的开始画面
                LoadFirstStageGameObjects();


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
            if (!m_CurrentZoneScene.name.Equals("Start"))
            {
                InputController.Instance.OnUpdate();

                ////不在开始界面时按返回键弹出返回按钮
                //if (Application.platform == RuntimePlatform.Android && (Input.GetKeyDown(KeyCode.Escape)))
                //{
                //    GameObject.Find("GameUI").GetComponent<GamingSetButton>().ShowExitWindow();
                //}
            }




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

            yield return SceneManager.LoadSceneAsync(newSceneName); //异步加载新场景
            TransitionDestination entrance = GetDestination(destinationTag);//获取新场景中人物传送目的地
            SetEnteringGameObjectLocation(entrance);//设置新场景人物初始位置
            SetupNewScene(transitionType, entrance); //设置场景为活动场景
            if (entrance != null)
                entrance.onReachDestination.Invoke();

            //加载该关卡的开始画面
            LoadFirstStageGameObjects();
            Debug.Log(newSceneName + "已经加载完成");

            m_Transitioning = false;           
        }

        //重新加载当前场景
        public static void RestartZone()
        {
            Instance.StartCoroutine(Instance.Transition(Instance.m_CurrentZoneScene.name, false, Instance.m_ZoneRestartDestinationTag, TransitionPoint.TransitionType.DifferentZone));
        }

        //根据传送点信息传送到下个场景中
        public static void TransitionToScene(TransitionPoint transitionPoint)
        {
            Instance.StartCoroutine(Instance.Transition(transitionPoint.newSceneName, transitionPoint.resetInputValuesOnTransition, transitionPoint.transitionDestinationTag, transitionPoint.transitionType));
        }

        //获取场景中的传送目的地
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

        //设置人物的初始化位置
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

        /// <summary>
        /// 加载Scene初始画面的GameObjects
        /// </summary>
        public void LoadFirstStageGameObjects()
        {
            if (m_IsSetupDynamicLoad)
            {
                //加载该关卡的开始画面

                GameObject startStagePoint = GameObject.Find("StartStagePoint");
                if (startStagePoint != null)
                {
                    string initStageName = startStagePoint.GetComponent<LoadNextStageQuest>().nextStageName;
                    Debug.Log("加载该关卡开始界面: " + initStageName);
                    if (initStageName != null)
                    {
                        //m_currStageContainer = LoadManager.Instance.LoadStageGameObject(initStageName);
                        m_currStageContainer = m_stageGameObjectsContainerList.AddFirst(LoadManager.Instance.LoadStageGameObject(initStageName));
                    }
                    else
                    {
                        Debug.Log("不存在初始画面的checkPoint或者stageName不正确");
                    }
                }
                
            }
        }

        /// <summary>
        /// 加载场景中下一个stage的物体
        /// </summary>
        /// <param name="stageName"></param>
        public void LoadNextStageGameObjects(string stageName)
        {
            GameObjectStageContainer newStageContainer = LoadManager.Instance.LoadStageGameObject(stageName);
            m_stageGameObjectsContainerList.AddLast(newStageContainer);
            //m_nextStageContainer = newStageContainer;
        }

        /// <summary>
        /// 卸载上一个Stage的物体
        /// </summary>
        public void UnloadPreStageGameobjects()
        {
            if (m_currStageContainer.Next != null)
            {
                m_currStageContainer = m_currStageContainer.Next;
                m_currStageContainer.Previous.Value.DestoryAll();
                m_stageGameObjectsContainerList.Remove(m_currStageContainer.Previous);
            }
            //m_currStageContainer.DestoryAll();
            //m_currStageContainer = m_nextStageContainer;
            //m_nextStageContainer = null;
        }

        public string GetLevelName(int level)
        {
            return allLevelName[level];
        }
    }

}