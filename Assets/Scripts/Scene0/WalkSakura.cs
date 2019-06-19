using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Ecin
{
    // Flower effect on player movement touches(except tricks, can also serves as a kind of trick hint)
    public class WalkSakura : MonoBehaviour
    {
        private Animator anim;
        private int clicked;
        private SpriteRenderer flowerRenderer;
        private Vector3 screenPos;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            flowerRenderer = GetComponent<SpriteRenderer>();

            // Make sure the effect is not rendered at start(just as designed), and the anim is not played
            flowerRenderer.enabled = false;

            // "Clicked" parameter is used to defer anim state "Flower" from "Idle", while "Idle" is the default state
            clicked = Animator.StringToHash("Clicked");
            anim.SetBool(clicked, false);
            anim.speed = 0;
        }

        // Update is called once per frame
        void Update()
        {
            // Steady version
            if (Input.GetMouseButtonDown(0))
            {
                // Transfer anim state from "Idle" to "Flower", animation is played in "Flower" state
                anim.SetBool(clicked, true);

                screenPos = Input.mousePosition;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

                // Detect if clicked on background(movement) or tricks(interact setting)
                Collider2D[] col = Physics2D.OverlapPointAll(worldPos);
                if (col.Length > 0)
                {
                    Debug.Log("Collider clicked");
                    flowerRenderer.enabled = false;
                }
                else
                {
                    Debug.Log("Background clicked");
                    Vector3 actualPos = new Vector3(worldPos.x, worldPos.y, this.transform.position.z);
                    this.transform.position = actualPos;
                    flowerRenderer.enabled = true;
                    anim.speed = 2.2f;
                }


            }

            // Animation end detect - new(test) -> stable version
            AnimatorStateInfo animatorInfo;
            animatorInfo = anim.GetCurrentAnimatorStateInfo(0);

            if ((animatorInfo.normalizedTime > 1.0f) && (animatorInfo.IsName("Sakura")))
            {
                Debug.Log("Flower animation end detected");
                anim.SetBool(clicked, false);
                flowerRenderer.enabled = false;
            }

            // Animation end time detect - old version
            //if (flowerRenderer.sprite.name == "1_2")
            //{
            //    Debug.Log("Reach anim end");
            //    anim.SetBool(clicked, false);
            //    flowerRenderer.enabled = false;
            //}
        }
    }
}
