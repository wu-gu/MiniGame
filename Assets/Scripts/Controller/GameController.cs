using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml;
using System.IO;
using UnityEngine.EventSystems;

namespace MiniGame
{

    public class GameController : MonoSingleton<GameController>
    {
        public Camera mainCamera;

        protected Scene m_CurrentZoneScene;
        //用于选择关卡时
        public List<string> allLevelName = new List<string>();//一关对应一个名字
        public int m_currLevelIndex;
        public int highestProgress;//玩家的最大进度，为allLevelName中的索引，即第几关

        //private List<AnimationClip> animationClips;
        //public AnimationClip[] scenesAnimationClips = new AnimationClip[5];
        //public RuntimeAnimatorController[] scenesAnimatorControllers = new RuntimeAnimatorController[5];
        //private SelfMonoBehaviour[] selfMonoBehaviours;

        //public TransitionDestination initialSceneTransitionDestination;
        //protected TransitionDestination.DestinationTag m_ZoneRestartDestinationTag;
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

        public GamingUI gamingUI;


        void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            //所有关卡名字(包括开始关卡)
            allLevelName.Add("Start");//0
            allLevelName.Add("Level0");//1
            allLevelName.Add("Level1");//2
            allLevelName.Add("Level2-1");//3
            allLevelName.Add("Level3");//4
            //highestProgress = 1;


            //从文件中读取游戏进度、配置，没有则默认进度配置
            LoadLevelProgressFromFile();

            m_CurrentZoneScene = SceneManager.GetActiveScene();

            if (!m_CurrentZoneScene.name.Equals("Start"))
            {
                //获取UI脚本
                gamingUI = GameObject.Find("GameUI").GetComponent<GamingUI>();
                //加载非开始关卡的开始画面
                LoadFirstStageGameObjects();
            }



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

        void Update()
        {
            //update时序控制
            if (!m_CurrentZoneScene.name.Equals("Start"))
            {
                {
                    InputController.Instance.OnUpdate();
                }
                ////不在开始界面时按返回键弹出返回按钮，所以此处注释掉
                //if (Application.platform == RuntimePlatform.Android && (Input.GetKeyDown(KeyCode.Escape)))
                //{
                //    GameObject.Find("GameUI").GetComponent<GamingSetButton>().ShowExitWindow();
                //}
            }
        }


        IEnumerator Transition(string newSceneName)
        {
            Debug.Log(newSceneName + "正在加载");
            m_Transitioning = true;
            QuestController.Instance.UnRegisterAllQuest();//注销所有机关
            yield return SceneManager.LoadSceneAsync(newSceneName); //异步加载新场景
            m_CurrentZoneScene = SceneManager.GetActiveScene();
            if (!m_CurrentZoneScene.name.Equals("Start"))
            {
                //获取UI脚本
                gamingUI = GameObject.Find("GameUI").GetComponent<GamingUI>();
                //加载非开始关卡的开始画面
                LoadFirstStageGameObjects();
            }

            //加载该关卡对应的背景音乐
            AudioController.Instance.MuteJustAmbient();
            AudioController.Instance.ChangeAmbient(GameObject.Find("InitSetting").GetComponent<InitSetting>().backgroundClip);
            AudioController.Instance.PlayJustAmbient();
            AudioController.Instance.UnmuteJustAmbient(1.0f);

            Debug.Log(newSceneName + "已经加载完成");
            m_Transitioning = false; 
        }


        /// <summary>
        /// 加载一个新场景(场景名)
        /// </summary>
        /// <param name="newSceneName"></param>
        public void TransitionToNewLevel(string newSceneName)
        {
            int index = allLevelName.IndexOf(newSceneName);
            if (index >= 0)
            {
                UnLoadAllStageGameObjects();
                m_currLevelIndex = index;
                Instance.StartCoroutine(Instance.Transition(newSceneName));
            }
        }

        /// <summary>
        /// 加载一个新场景(根据索引)
        /// </summary>
        /// <param name="newSceneName"></param>
        public void TransitionToNewLevel(int index)
        {
            if (index >= 0)
            {
                UnLoadAllStageGameObjects();
                m_currLevelIndex = index;
                Instance.StartCoroutine((Instance.Transition(allLevelName[index])));
            }
        }

        /// <summary>
        /// 自动加载下一个关卡
        /// </summary>
        public void TransitionToNextLevel()
        {
            m_currLevelIndex++;
            Debug.Log("加载下一个关卡" + allLevelName[m_currLevelIndex]);
            TransitionToNewLevel(m_currLevelIndex);
        }

        /// <summary>
        /// 设置场景为活动场景
        /// 必须要保证：目标场景被加载后，才可以正确设置活动状态
        /// </summary>
        //IEnumerator Transition(string newSceneName, bool resetInputValues, TransitionDestination.DestinationTag destinationTag, TransitionPoint.TransitionType transitionType = TransitionPoint.TransitionType.DifferentZone)
        //{
        //    m_Transitioning = true;

        //    yield return SceneManager.LoadSceneAsync(newSceneName); //异步加载新场景
        //    TransitionDestination entrance = GetDestination(destinationTag);//获取新场景中人物传送目的地
        //    SetEnteringGameObjectLocation(entrance);//设置新场景人物初始位置
        //    SetupNewScene(transitionType, entrance); //设置场景为活动场景
        //    if (entrance != null)
        //        entrance.onReachDestination.Invoke();

        //    //加载该关卡的开始画面
        //    LoadFirstStageGameObjects();
        //    Debug.Log(newSceneName + "已经加载完成");

        //    m_Transitioning = false;           
        //}

        //重新加载当前场景
        //public static void RestartZone()
        //{
        //    Instance.StartCoroutine(Instance.Transition(Instance.m_CurrentZoneScene.name, false, Instance.m_ZoneRestartDestinationTag, TransitionPoint.TransitionType.DifferentZone));
        //}

        //根据传送点信息传送到下个场景中
        //public static void TransitionToScene(TransitionPoint transitionPoint)
        //{
        //    Instance.StartCoroutine(Instance.Transition(transitionPoint.newSceneName, transitionPoint.resetInputValuesOnTransition, transitionPoint.transitionDestinationTag, transitionPoint.transitionType));
        //}

        //获取场景中的传送目的地
        //protected TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
        //{
        //    TransitionDestination[] entrances = FindObjectsOfType<TransitionDestination>();
        //    for (int i = 0; i < entrances.Length; i++)
        //    {
        //        if (entrances[i].destinationTag == destinationTag)
        //            return entrances[i];
        //    }
        //    Debug.LogWarning("No entrance was found with the " + destinationTag + ".tag. ");
        //    return null;
        //}

        //设置人物的初始化位置
        //protected void SetEnteringGameObjectLocation(TransitionDestination entrance)
        //{
        //    if (entrance == null)
        //    {
        //        Debug.LogWarning("Entering Transform's location has not been set yet. ");
        //        return;
        //    }
        //    Transform entranceLocation = entrance.transform;
        //    Transform enteringTransform = entrance.transitioningGameObject.transform;
        //    enteringTransform.position = entranceLocation.position;
        //    enteringTransform.rotation = entranceLocation.rotation;
        //}

        //protected void SetupNewScene(TransitionPoint.TransitionType transitionType, TransitionDestination entrance)
        //{
        //    if (entrance == null)
        //    {
        //        Debug.LogWarning("Restart information has not been set. ");
        //        return;
        //    }

        //    if (transitionType == TransitionPoint.TransitionType.DifferentZone)
        //        SetZoneStart(entrance);
        //}

        //protected void SetZoneStart(TransitionDestination entrance)
        //{
        //    m_CurrentZoneScene = entrance.gameObject.scene;
        //    m_ZoneRestartDestinationTag = entrance.destinationTag;
        //}

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
                        m_currStageContainer = m_stageGameObjectsContainerList.AddFirst(LoadManager.Instance.LoadStageGameObject(allLevelName[m_currLevelIndex] + "/" + initStageName));
                    }
                    else
                    {
                        Debug.Log("不存在初始画面的checkPoint或者stageName不正确");
                    }
                }
                else
                {
                    Debug.Log("该关卡不需要动态加载初始界面");
                }
            }
        }

        /// <summary>
        /// 加载场景中下一个stage的物体
        /// </summary>
        /// <param name="stageName"></param>
        public void LoadNextStageGameObjects(string stageName)
        {
            GameObjectStageContainer newStageContainer = LoadManager.Instance.LoadStageGameObject(allLevelName[m_currLevelIndex]+"/"+stageName);
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

        /// <summary>
        /// 清空当前关卡的所有stageContainer
        /// </summary>
        public void UnLoadAllStageGameObjects()
        {
            m_stageGameObjectsContainerList.Clear();
        }

        public string GetLevelName(int level)
        {
            return allLevelName[level];
        }

        public void setCurrLevelIndex(int index)
        {
            m_currLevelIndex = index;
        }

        /// <summary>
        /// 从配置文件加载游戏关卡进度，即最高到第几关了
        /// </summary>
        public void LoadLevelProgressFromFile()
        {
            Debug.Log("读取游戏最高进度");
            string filePath = Application.persistentDataPath + "/config.xml";
            if (File.Exists(filePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("config").ChildNodes;
                foreach (XmlElement xmlEle in xmlNodeList)
                {
                    if (xmlEle.Name.Equals("progress"))
                    {
                        highestProgress = int.Parse(xmlEle.InnerText);
                        Debug.Log("从配置文件读取，最高关卡为" + highestProgress);
                    }
                    if (xmlEle.Name.Equals("progress"))
                    {
                        highestProgress = int.Parse(xmlEle.InnerText);
                        Debug.Log("从配置文件读取，最高关卡为" + highestProgress);
                    }
                }
            }
            else
            {
                highestProgress = 1;
            }
        }



        /// <summary>
        /// 写入配置文件
        /// </summary>
        public void WriteLevelProgressToFile()
        {
            string filePath = Application.persistentDataPath + "/config.xml";
            //修改
            if (File.Exists(filePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                XmlNodeList xmlNodeList = xmlDoc.SelectSingleNode("config").ChildNodes;
                foreach (XmlElement xmlEle in xmlNodeList)
                {
                    if (xmlEle.Name.Equals("progress"))
                    {
                        xmlEle.InnerText = highestProgress.ToString();
                    }
                }
                xmlDoc.Save(filePath);
            }
            //新增
            else
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlElement root = xmlDoc.CreateElement("config");
                XmlElement element = xmlDoc.CreateElement("progress");
                element.InnerText = 1.ToString();
                root.AppendChild(element);
                xmlDoc.AppendChild(root);
                xmlDoc.Save(filePath);
            }
            Debug.Log("写入配置文件，最高关卡为" + highestProgress);
        }

        /// <summary>
        /// 公共结束游戏接口，退出前存储游戏关卡最高进度
        /// </summary>
        public void EndGame()
        {
            WriteLevelProgressToFile();
            Application.Quit();
        }

        /// <summary>
        /// 游戏中非UI区域点击关闭正在显示UI
        /// </summary>
        public void HideGamingUI()
        {
            if (gamingUI != null)
                gamingUI.HideShowingWindow();
        }
    }

}