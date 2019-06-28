using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatAppearAnimationTrigger : MonoBehaviour
{
    GameObject m_boat;
    void BoatAppear()
    {
        m_boat = GameObject.Find("Boat");
        m_boat.GetComponent<Animator>().enabled = true;
        Debug.Log("wk");
    }
}
