using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    // Sakura effect on player movement touches(except tricks, can also serves as a kind of trick hint)
    public class WalkSakura : MonoBehaviour
    {
        private Animator anim;
        private int clicked;
        private SpriteRenderer SakuraRenderer;
        private Vector3 screenPos;
        private Camera m_camera;
        private Vector3 m_originScale;

        // Only set once when awaken
        void Awake()
        {
            anim = GetComponent<Animator>();
            SakuraRenderer = GetComponent<SpriteRenderer>();

            // "Clicked" parameter is used to defer anim state "Sakura" from "Idle", while "Idle" is the default state
            clicked = Animator.StringToHash("Clicked");
            anim.SetBool(clicked, false);
            anim.speed = 0;
            m_camera = Camera.main;
            m_originScale = transform.localScale;
        }

        // Update is called once per frame
        void Update()
        {
            // Animation end detect - new(test) -> stable version
            AnimatorStateInfo animatorInfo;
            animatorInfo = anim.GetCurrentAnimatorStateInfo(0);

            if ((animatorInfo.normalizedTime > 1.0f) && (animatorInfo.IsName("Sakura")))
            {
                //Debug.Log("Sakura animation end detected");
                anim.SetBool(clicked, false);
                SakuraRenderer.enabled = false;
                this.enabled = false;
            }
        }

        public void SetSakura(Vector3 sakuraPos)
        {
            this.enabled = true;
            this.transform.position = sakuraPos;
            float currCamSize = m_camera.orthographicSize;
            this.transform.localScale = new Vector3(m_originScale.x / 5.4f * currCamSize, m_originScale.y / 5.4f * currCamSize, m_originScale.z);
            anim.SetBool(clicked, true);
            SakuraRenderer.enabled = true;
            anim.speed = 0.7f;

            anim.Play("Sakura", 0, 0f);
            anim.Update(0);
        }
    }
}
