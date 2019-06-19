using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class myscroll : MonoBehaviour,IBeginDragHandler,IEndDragHandler
{
    private ScrollRect mscroll;
    private float[] page = new float[] { 0, 0.5f, 1 };
    private float slow = 0;
    public bool flag;
    public Toggle[] mtoggle;
    myImage mimage;
    myimage2 mimage1;
    myimage2 mimage2;


    // Start is called before the first frame update
    void Start()
    {
        mscroll = GetComponent<ScrollRect>();
        mimage = GameObject.Find("level_1").GetComponent<myImage>();
        mimage1 = GameObject.Find("level_2").GetComponent<myimage2>();
        mimage2 = GameObject.Find("level_3").GetComponent<myimage2>();

    }

    // Update is called once per frame
    void Update()
    {
        if(flag==false)
        {
            mscroll.horizontalNormalizedPosition = Mathf.Lerp(mscroll.horizontalNormalizedPosition, slow, Time.deltaTime * 4);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        flag = true;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        flag = false;
        float pos = mscroll.horizontalNormalizedPosition;
        int index = 0;
        float offset = Mathf.Abs(page[index] - pos);
        for (int i = 1; i < page.Length; i++)
        {
            float offsetpos = Mathf.Abs(page[i] - pos);
            if (offsetpos < offset)
            {
                index = i;
                offset = offsetpos;
            }
        }
        slow = page[index];
        mtoggle[index].isOn = true;
    }

  
    public void OnClick2(bool isOn)
    {
        if (isOn)
        {
            slow = page[1];
            mimage.Hide();
            mimage2.Hide();
            mimage1.Show();

        }
    }
    public void OnClick3(bool isOn)
    {
        if (isOn)
        {
            slow = page[2];
            mimage.Hide();
            mimage1.Hide();
            mimage2.Show();
        }
    }
    public void OnClick1(bool isOn)
    {
        if (isOn)
        {
            slow = page[0];
            mimage.Show();
            mimage1.Hide();
            mimage2.Hide();

        }
    }
}
