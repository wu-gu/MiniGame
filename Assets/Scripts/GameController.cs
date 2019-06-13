using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController gameController { get; private set; }

    public InputController inputController { get; private set; }
    public RoleController roleController { get; private set; }


    void Start()
    {
        gameController = this;
        inputController = new InputController();
        roleController = new RoleController();
    }


    void Update()
    {
        //update时序控制
        inputController.onUpdate();
    }
}
