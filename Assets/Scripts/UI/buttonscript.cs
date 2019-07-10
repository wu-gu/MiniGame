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
    public ParticleSystem leafPar;
    public ParticleSystem flowerPar;

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
        leafPar.gameObject.SetActive(false);
        flowerPar.gameObject.SetActive(false);
    }

    public void OnClickReturn()
    {
        AudioController.Instance.PushClip(buttonEffectClip);
        Debug.Log("return");
        com1.Hide();
        com.Show();
        leafPar.gameObject.SetActive(true);
        flowerPar.gameObject.SetActive(false);
    }

    public void OnClickSet()
    {
        AudioController.Instance.PushClip(buttonEffectClip);
        ambientMusicSlider.value = AudioController.Instance.backgroundVolume;
        effectMusicSlider.value = AudioController.Instance.soundEffectVolume;

        com2.Show();
        com.Hide();
        leafPar.gameObject.SetActive(false);
        flowerPar.gameObject.SetActive(true);
    }

    public void OnClickReturn1()
    {
        AudioController.Instance.PushClip(buttonEffectClip);
        com1.Hide();
        com.Show();
        com2.Hide();
        leafPar.gameObject.SetActive(true);
        flowerPar.gameObject.SetActive(false);
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
        GameController.Instance.EndGame();
        //com3.Hide();
        //com.Show1();
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
        GameController.Instance.TransitionToNewLevel("Level0");
        //        string targetLevelName = GameController.Instance.GetLevelName(0);
        //TransitionPoint transitionPoint = GameObject.Find("TransitionStart").GetComponent<TransitionPoint>();
        //transitionPoint.newSceneName = targetLevelName;
        //transitionPoint.transitionType = TransitionPoint.TransitionType.DifferentZone;
        //transitionPoint.Transition();

    }


    public void OnClickLevel_2()
    {
        if (GameController.Instance.highestProgress > 2)
        {
            AudioController.Instance.PushClip(buttonEffectClip);
            GameController.Instance.TransitionToNewLevel("Level2");
        }
    }


    public void OnClickLevel_3()
    {
        if (GameController.Instance.highestProgress > 3)
        {
            AudioController.Instance.PushClip(buttonEffectClip);
            GameController.Instance.TransitionToNewLevel("Level3");
        }
    }

    /// <summary>
    /// 写在这里是因为如果直接在Slider的OnValueChanged中拉入GameObjectController时，当再回到该界面时，Slider的OnValueChanged
    /// 已经时Missing了，所以OnValueChanged拉入的物体最好不是动态的，而是跟随Slider一起出现，一起消失
    /// </summary>
    /// <param name="slider"></param>
    public void AmbientMusicVolumeChanged(Slider slider)
    {
        AudioController.Instance.BackgroundMusicVolumeChanged(slider);
    }

    public void EffectMusicVolumeChanged(Slider slider)
    {
        AudioController.Instance.SoundEffectVolumeChanged(slider);
    }

}
