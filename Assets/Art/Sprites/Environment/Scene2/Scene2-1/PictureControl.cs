using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class PictureControl : MonoBehaviour
    {
        private GameObject m_girl;
        private CatControl m_cat;
        private Animator m_cataimator;

        // Start is called before the first frame update
        void Start()
        {
            m_girl = GameObject.Find("Girl");
            m_cat = GameObject.Find("Cat 1").GetComponent<CatControl>();
            m_cataimator = GameObject.Find("Cat 1").GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.Equals(m_girl))
            {
                Debug.Log("成功了");
                InputController.Instance.SetPlayerCanMove(false);
                m_cat.enabled = true;
                m_cataimator.enabled = true;

            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.Equals(m_girl))
            {

            }
        }
    }
 
}