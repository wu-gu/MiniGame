using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCard : MonoBehaviour, QuestBehavior
{
    public enum AtZone
    {
        NONE, ZONE1, ZONE2, ZONE3, ZONE4,
    }

    private AtZone currentZone;
    private Collider2D[] zones;
    private Vector3 offSet;
    private bool moveToCenter;
    private Vector3 boxCenter;
    private Vector3 originalPos;

    // Start is called before the first frame update
    void Start()
    {
        QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
        currentZone = AtZone.NONE;
        zones = new Collider2D[4];
        zones[0] = GameObject.Find("CardZone_1").GetComponent<Collider2D>();
        zones[1] = GameObject.Find("CardZone_2").GetComponent<Collider2D>();
        zones[2] = GameObject.Find("CardZone_3").GetComponent<Collider2D>();
        zones[3] = GameObject.Find("CardZone_4").GetComponent<Collider2D>();
        offSet = Vector3.zero;
        moveToCenter = true;
        originalPos = this.transform.position;
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {       
        if(Input.GetMouseButton(0))
            this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offSet;
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if(zones[i].bounds.Contains(this.transform.position))
                {
                    this.transform.position = zones[i].bounds.center;
                    currentZone = AtZone.NONE + i + 1;
                    break;
                }
                if (i == 3)
                    currentZone = AtZone.NONE;
            }
            if (currentZone == AtZone.NONE)
                this.transform.position = originalPos;
            this.enabled = false;
        }
    }

    public AtZone GetCurrentZone()
    {
        return currentZone;
    }

    public void OnUpdate()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offSet = mousePos - this.transform.position;
        this.enabled = true;
    }
}
