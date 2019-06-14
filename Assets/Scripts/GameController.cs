using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public InputController inputController { get; private set; }
    public RoleController roleController { get; private set; }

    protected static GameController instance;
    protected Scene m_CurrentZoneScene;

    public static GameController Instance
    {
        get
        {
            if (instance != null)
                return instance;

            instance = FindObjectOfType<GameController>();

            if (instance != null)
                return instance;

            Create();

            return instance;
        }
    }

    public static GameController Create()
    {
        GameObject gameControllerGameObject = new GameObject("GameController");
        instance = gameControllerGameObject.AddComponent<GameController>();

        return instance;
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
        //可以在此初始化输入控制等其他控制器

    }

    void Start()
    {

    }


    void Update()
    {
        //update时序控制
        inputController.onUpdate();
    }
}
