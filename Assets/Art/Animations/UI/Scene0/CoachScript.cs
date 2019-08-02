using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class CoachScript : MonoBehaviour
    {
        private Animator m_CoachAnimator;
        private Animator m_FishAnimator;
        private Animator m_BallAnimator;
        private float lastTime;   //计时器
        private float curTime;
        private CoachWater m_Water;
        private bool One = true;
        private bool Tow = true;
        private bool Three = true;

        // Start is called before the first frame update
        void Start()
        {
            m_CoachAnimator = GetComponent<Animator>();
            m_FishAnimator = GameObject.Find("CoachFish2").GetComponent<Animator>();
            m_BallAnimator = GameObject.Find("Ball").GetComponent<Animator>();
            m_Water = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CoachWater>();
            m_CoachAnimator.enabled = false;
            m_FishAnimator.enabled = false;
            m_BallAnimator.enabled = false;
            lastTime = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            curTime = Time.time;
            Debug.Log(curTime);
            if (curTime-lastTime >= 1)
            {
                m_CoachAnimator.enabled = true;
                //m_timer = 0;
            }
            if (curTime - lastTime >= 3&&One==true)
            {
                m_FishAnimator.enabled = true;
                m_Water.OneWaterWave();
                One = false;
                //m_timer = 0;
            }

            if (curTime - lastTime >= 6)
            {
                m_CoachAnimator.SetBool("TouchBool", true) ;
                m_FishAnimator.SetBool("TouchBool", true);
                //m_timer = 0;
            }

            if (curTime - lastTime >= 8&&Tow==true)
            {
                m_BallAnimator.enabled = true;
                m_Water.TowWaterWave();
                Tow = false;
                //m_timer = 0;
            }

            if (curTime - lastTime >= 10.5)
            {
                m_FishAnimator.SetBool("PositionBool", true);
                m_BallAnimator.SetBool("PositionBool", true);
                //m_timer = 0;
            }

            if (curTime - lastTime >= 12)
            {
                m_CoachAnimator.SetBool("PositionBool", true);
                //m_timer = 0;
            }

            if (curTime - lastTime >= 15)
            {
                m_FishAnimator.SetBool("PtoRBool", true);
                //m_timer = 0;
            }

            if (curTime - lastTime >= 16)
            {
                m_CoachAnimator.SetBool("RotationBool", true);
                //m_timer = 0;
            }

            if (curTime - lastTime >= 17)
            {
                
                m_FishAnimator.SetBool("RotationBool", true);
                m_BallAnimator.SetBool("RotationBool", true);
                //m_timer = 0;
            }

            if (curTime - lastTime >= 21.5&&Three==true)
            {
                m_FishAnimator.SetBool("DisappearBool", true);
                m_BallAnimator.SetBool("DisappearBool", true);
                m_Water.ThreeWaterWave();
                Three = false;
                //m_timer = 0;
            }

            if (curTime - lastTime >= 22.5)
            {
                m_CoachAnimator.SetBool("EndBool", true);
                //m_timer = 0;
            }
        }
    }
}
