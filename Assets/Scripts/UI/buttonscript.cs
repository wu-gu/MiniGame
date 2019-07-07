using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MiniGame;

public class Buttonscript : MonoBehaviour
{
    // Start is called before the first frame update
    mycanvas com;
    mycanvas1 com1;
    mycanvas2 com2;
    mycanvas2 com3;
    WalkSakura sakura;
    Vector3 point;
    public ParticleSystem par;

    [Tooltip("按钮点击音效")]
    public AudioClip buttonEffectClip;

    public Slider ambientMusicSlider;
    public Slider effectMusicSlider;


    void Start()
    {
        com = GameObject.Find("init").GetComponent<mycanvas>();
        com1 = GameObject.Find("start").GetComponent<mycanvas1>();
        com2 = GameObject.Find("set").GetComponent<mycanvas2>();
        com3 = GameObject.Find("End").GetComponent<mycanvas2>();
        sakura = GameObject.Find("OnClickSakura").GetComponent<WalkSakura>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            point = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 4f);//获得鼠标点击点
            point = Camera.main.ScreenToWorldPoint(point);//从屏幕空间转换到世界空间
            sakura.SetSakura(point);
        }
    }

    public void OnClickStar()
    {
        AudioController.Instance.PushClip(buttonEffectClip);
        com.Hide();
        com1.Show();
        par.gameObject.SetActive(false);
    }

    public void OnClickReturn()
    {
        AudioController.Instance.PushClip(buttonEffectClip);
        Debug.Log("return");
        com1.Hide();
        com.Show();
        par.gameObject.SetActive(true);
    }

    public void OnClickSet()
    {
        AudioController.Instance.PushClip(buttonEffectClip);
        ambientMusicSlider.value = AudioController.Instance.ambientVolume;
        effectMusicSlider.value = AudioController.Instance.musicVolume;

        com2.Show();
        com.Hide();
        par.gameObject.SetActive(false);

    }

    public void OnClickReturn1()
    {
        AudioController.Instance.PushClip(buttonEffectClip);
        com1.Hide();
        com.Show();
        com2.Hide();
        par.gameObject.SetActive(true);
    }

    public void OnClickEND()
    {
        AudioController.Instance.PushClip(buttonEffectClip);
        com3.Show();
        com.Hide1();
    }

    public void OnClickENDSure()
    {
        AudioController.Instance.PushClip(buttonEffectClip);
        com3.Hide();
        com.Show1();
    }

    public void OnClickENDCancel()
    {
        AudioController.Instance.PushClip(buttonEffectClip);
        com3.Hide();
        com.Show1();
    }

    //public void OnClickgameSet()
    //{

    //    com4.Show();
    //}
    //public void OnClickgameSetclose()
    //{

    //    com4.Hide();
    //}
    //public void OnClickmainScene()
    //{

    //    com4.Hide();
    //}

    public void OnClickLevel_1()
    {
        AudioController.Instance.PushClip(buttonEffectClip);
        GameController.Instance.TransitionToNewLevel(0);
        //        string targetLevelName = GameController.Instance.GetLevelName(0);
        //TransitionPoint transitionPoint = GameObject.Find("TransitionStart").GetComponent<TransitionPoint>();
        //transitionPoint.newSceneName = targetLevelName;
        //transitionPoint.transitionType = TransitionPoint.TransitionType.DifferentZone;
        //transitionPoint.Transition();

    }
    public void OnClickLevel_2()
    {

        print("2222222222!");
    }
    public void OnClickLevel_3()
    {

        print("333333333!");
    }



}
