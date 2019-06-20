using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniGame
{

    public class GameController : MonoSingleton<GameController>
    {

        public InputController inputController { get; private set; }
        public CharacterController chatacterController { get; private set; }

        public Camera mainCamera;

        protected Scene m_CurrentZoneScene;
        protected string m_CurrentZoneSceneName;
        //private List<AnimationClip> animationClips;
        public AnimationClip[] scenesAnimationClips = new AnimationClip[5];
        public RuntimeAnimatorController[] scenesAnimatorControllers = new RuntimeAnimatorController[5];
        private SelfMonoBehaviour[] selfMonoBehaviours;
        

        void Awake()
        {
            base.Awake();
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            m_CurrentZoneScene = SceneManager.GetActiveScene();

            switch (m_CurrentZoneScene.name)
            {
                case "Scene0":
                {
                    AnimationController.Instance.AddAnimation(scenesAnimationClips[0]);
                    AnimationController.Instance.PlayAnimation(scenesAnimationClips[0].name);
                        //AnimatorController.Instance.AddAnimator(scenesAnimatorControllers[0]);
                        //AnimatorController.Instance.PlayAnimator()
                    Debug.Log(scenesAnimationClips[0].name);
                    break;
                }
                case "Scene1":
                {
                    AnimationController.Instance.AddAnimation(scenesAnimationClips[1]);
                    AnimationController.Instance.PlayAnimation(scenesAnimationClips[1].name);
                    Debug.Log(scenesAnimationClips[1].name);
                    break;
                }
                default:
                    break;
            }
            //可以在此初始化输入控制等其他控制器

            mainCamera = Camera.main;

        }

        void Start()
        {
            selfMonoBehaviours = gameObject.GetComponents<SelfMonoBehaviour>();
        }


        void Update()
        {
            //update时序控制
            inputController.onUpdate();

            var count = selfMonoBehaviours.Length;
		    for (var i = 0; i < count; i++)
		    {
			    selfMonoBehaviours[i].DoUpdate();
		    }
            Debug.Log("拥有组件" + selfMonoBehaviours[0].name);
        }

        /// <summary>
        /// 设置场景为活动场景
        /// 必须要保证：目标场景被加载后，才可以正确设置活动状态
        /// </summary>
        IEnumerator SetActiveSceneEnumerator(int index)
        {
            yield return SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive); //等待场景加载完毕后，再向下执行;异步加载，方式：附加
            SceneManager.SetActiveScene(SceneManager.GetSceneAt(index));             //设置场景为活动场景
            m_CurrentZoneSceneName = m_CurrentZoneScene.name;
        }
    }

}