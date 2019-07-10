using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Choose : MonoBehaviour
{
    
    public bool flag;
    public Toggle[] mtoggle;
    private Image chooselevel1, chooselevel2, chooselevel3;
    myImage mimage;
    myimage2 mimage1;
    myimage2 mimage2;
    
    // Start is called before the first frame update
    void Start()
    {
        mimage = GameObject.Find("level_1").GetComponent<myImage>();
        mimage1 = GameObject.Find("level_2").GetComponent<myimage2>();
        mimage2 = GameObject.Find("level_3").GetComponent<myimage2>();
        chooselevel1 = GameObject.Find("chooselevel1").GetComponent<Image>();
        chooselevel2 = GameObject.Find("chooselevel2").GetComponent<Image>();
        chooselevel3 = GameObject.Find("chooselevel3").GetComponent<Image>();



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick1(bool isOn)
    {
        if (isOn)
        {
            chooselevel1.enabled = false;
            chooselevel2.enabled = true;
            chooselevel3.enabled = true;
            mimage.Show();
            mimage1.Hide();
            mimage2.Hide();


        }
    }
    public void OnClick2(bool isOn)
    {
        if (isOn)
        {
            chooselevel1.enabled = true;
            chooselevel2.enabled = false;
            chooselevel3.enabled = true;
            mimage.Hide();
            mimage2.Hide();
            mimage1.Show();

        }
    }
    public void OnClick3(bool isOn)
    {
        if (isOn)
        {
            chooselevel1.enabled = true;
            chooselevel2.enabled = true;
            chooselevel3.enabled = false;
            mimage.Hide();
            mimage1.Hide();
            mimage2.Show();

        }
    }
}
