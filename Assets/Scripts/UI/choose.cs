using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class choose : MonoBehaviour
{

    public bool flag;
    public Toggle[] mtoggle;
    myImage mimage;
    myimage2 mimage1;
    myimage2 mimage2;
    
    // Start is called before the first frame update
    void Start()
    {
        mimage = GameObject.Find("level_1").GetComponent<myImage>();
        mimage1 = GameObject.Find("level_2").GetComponent<myimage2>();
        mimage2 = GameObject.Find("level_3").GetComponent<myimage2>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick1(bool isOn)
    {
        if (isOn)
        {
            mimage.Show();
            mimage1.Hide();
            mimage2.Hide();


        }
    }
    public void OnClick2(bool isOn)
    {
        if (isOn)
        {
            mimage.Hide();
            mimage2.Hide();
            mimage1.Show();

        }
    }
    public void OnClick3(bool isOn)
    {
        if (isOn)
        {
            mimage.Hide();
            mimage1.Hide();
            mimage2.Show();

        }
    }
}
