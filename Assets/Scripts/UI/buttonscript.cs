using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class buttonscript : MonoBehaviour
{
    // Start is called before the first frame update
    mycanvas com;
    mycanvas1 com1;
    mycanvas2 com2;
    mycanvas2 com3;
    public ParticleSystem par;

    void Start()
    {
        com = GameObject.Find("init").GetComponent<mycanvas>();
        com1 = GameObject.Find("start").GetComponent<mycanvas1>();
        com2 = GameObject.Find("set").GetComponent<mycanvas2>();
        com3 = GameObject.Find("endshow").GetComponent<mycanvas2>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickStar()
    {
        com.Hide();
        com1.Show();
        com2.Hide();
        par.gameObject.SetActive(false);
    }

    public void OnClickReturn()
    {
        com1.Hide();
        com.Show();
        com2.Hide();
        par.gameObject.SetActive(true);
    }

    public void OnClickSet()
    {
        com1.Hide();
        com2.Show();
        com.Hide();
        par.gameObject.SetActive(false);
    }

    public void OnClickReturn1()
    {
        com1.Hide();
        com.Show();
        com2.Hide();
        par.gameObject.SetActive(true);
    }

    public void OnClickEND()
    {
        com3.Show();
        com1.Hide();
        com2.Hide();
    }

    public void OnClickENDSure()
    {
        com3.Hide();
    }

    public void OnClickENDCancel()
    {
        com3.Hide();
    }

    public void OnClickLevel_1()
    {
        print("wdnmd!");

        
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
