using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCards : MonoBehaviour
{
    private MoveCard[] cardScripts;

    // Start is called before the first frame update
    void Start()
    {
        cardScripts = new MoveCard[4];
        cardScripts[0] = GameObject.Find("Card_1").GetComponent<MoveCard>();
        cardScripts[1] = GameObject.Find("Card_2").GetComponent<MoveCard>();
        cardScripts[2] = GameObject.Find("Card_3").GetComponent<MoveCard>();
        cardScripts[3] = GameObject.Find("Card_4").GetComponent<MoveCard>();
    }

    // Update is called once per frame
    void Update()
    {
        int correctCounter = 0;
        for (int i = 0; i < 4; i++)
        {
            if (cardScripts[i].GetCurrentZone() == MoveCard.AtZone.NONE + i + 1)
                correctCounter++;
        }
        if (correctCounter == 4)
        {
            Debug.Log("3-1 Success");
            for (int i = 0; i < 4; i++)
                QuestController.Instance.UnRegisterQuest(cardScripts[i].gameObject.ToString());
            Destroy(this);
        }
    }
}
