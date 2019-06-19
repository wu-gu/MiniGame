using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Ecin
{
    // Sakura effect on player movement touches(except tricks, can also serves as a kind of trick hint)
    public class WalkSakura : MonoBehaviour
    {
        private Animator anim;
        private int clicked;
        private SpriteRenderer SakuraRenderer;
        private Vector3 screenPos;

        // Only set once when awaken
        void Awake()
        {
            anim = GetComponent<Animator>();
            SakuraRenderer = GetComponent<SpriteRenderer>();

            // "Clicked" parameter is used to defer anim state "Sakura" from "Idle", while "Idle" is the default state
            clicked = Animator.StringToHash("Clicked");
            anim.SetBool(clicked, false);
            anim.speed = 0;
        }

        // Update is called once per frame
        void Update()
        {
            // Animation end detect - new(test) -> stable version
            AnimatorStateInfo animatorInfo;
            animatorInfo = anim.GetCurrentAnimatorStateInfo(0);

            if ((animatorInfo.normalizedTime > 1.0f) && (animatorInfo.IsName("Sakura")))
            {
                Debug.Log("Sakura animation end detected");
                anim.SetBool(clicked, false);
                SakuraRenderer.enabled = false;
                this.enabled = false;
            }

            // Animation end time detect - old version
            //if (SakuraRenderer.sprite.name == "1_2")
            //{
            //    Debug.Log("Reach anim end");
            //    anim.SetBool(clicked, false);
            //    SakuraRenderer.enabled = false;
            //}
        }

        public void SetSakura(Vector3 sakuraPos)
        {
            this.enabled = true;
            this.transform.position = sakuraPos;
            anim.SetBool(clicked, true);
            SakuraRenderer.enabled = true;
            anim.speed = 1.0f;

            anim.Play("Sakura", 0, 0f);
            anim.Update(0);

            
        }
    }
}
