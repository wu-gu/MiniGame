using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public class Glowworm : MonoBehaviour, QuestBehavior
    {
        private ParticleSystem m_particleSystem;
        private ParticleSystem.ShapeModule m_shapeModule;
        private Vector3 m_oriPos;
        private Vector3 m_newScale;
        private Vector3 m_dragScale;
        private Vector3 m_oriScale;
        private Vector2 m_offset;
        private bool m_firstTouch;
        private Licht m_licht;
        private Renderer m_lanternL;
        private Renderer m_lanternM;
        private Renderer m_lanternR;
        private bool m_lockMovement;

        // Start is called before the first frame update
        void Start()
        {
            QuestController.Instance.RegisterQuest(gameObject.ToString(), this);
            m_particleSystem = GetComponent<ParticleSystem>();
            m_shapeModule = m_particleSystem.shape;
            m_oriPos = this.transform.position;
            m_oriScale = this.transform.localScale;
            m_dragScale = new Vector3(3.11f, 3.34f, 1.272228f);
            m_newScale = new Vector3(1.145005f, 1.272228f, 1.272228f);
            m_firstTouch = false;
            m_licht = GameObject.Find("Lanterns").GetComponent<Licht>();
            m_lanternL = GameObject.Find("Lantern_light1").GetComponent<Renderer>();
            m_lanternM = GameObject.Find("Lantern_light2").GetComponent<Renderer>();
            m_lanternR = GameObject.Find("Lantern_light3").GetComponent<Renderer>();
            m_lockMovement = false;
            this.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            // -- PC stable --          -- Android stable --                                       
            if (/*Input.GetMouseButton(0)*/Input.touchCount > 0)
            {
                if (m_firstTouch)
                {
                    m_shapeModule.shapeType = ParticleSystemShapeType.SingleSidedEdge;
                    this.transform.localScale = m_dragScale;
                    m_particleSystem.Simulate(2f, false, false, true);
                    m_particleSystem.Play();
                    m_firstTouch = false;
                }
                // -- PC stable --          -- Android stable --          
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(/*Input.mousePosition*/Input.GetTouch(0).position);
                this.transform.position = new Vector3(touchPos.x + m_offset.x, touchPos.y + m_offset.y, this.transform.position.z);
            }
            else
            {

                Vector2 currPos = this.transform.position;


                if (m_licht.AcceptLicht())
                {
                    if (m_lanternL.bounds.Contains(currPos))
                    {
                        transform.position = m_lanternL.transform.position;
                        m_licht.LichtLeft();
                    }
                    if (m_lanternR.bounds.Contains(currPos))
                    {
                        transform.position = m_lanternR.transform.position;
                        m_licht.LichtRight();
                    }
                    if (m_lanternM.bounds.Contains(currPos))
                    {
                        transform.position = m_lanternM.transform.position;
                        m_licht.LichtMid();
                    }
                }

                if (m_licht.Revert())
                {
                    Revert();
                }
                else if (m_licht.Reset())
                {
                    GameObject glowworm = GameObject.Find("Glowworm_1");
                    glowworm.GetComponent<Glowworm>().Revert();
                    glowworm = GameObject.Find("Glowworm_2");
                    glowworm.GetComponent<Glowworm>().Revert();
                    glowworm = GameObject.Find("Glowworm_3");
                    glowworm.GetComponent<Glowworm>().Revert();
                }
                else if (!m_lanternL.bounds.Contains(currPos) && !m_lanternM.bounds.Contains(currPos) && !m_lanternR.bounds.Contains(currPos))
                    Revert();
                else
                    this.transform.localScale = new Vector3(0f, 0f, 0f);

                this.enabled = false;
            }

        }

        public void OnUpdate()
        {
            if (this.transform.position != m_oriPos) // Not reverted or reset -- reach a possible target
                m_lockMovement = true;
            else
                m_lockMovement = false;

            // -- PC stable --          -- Android stable --          
            if (!m_lockMovement && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                this.enabled = true;
                // -- PC stable --          -- Android stable --          
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(/*Input.mousePosition*/ Input.GetTouch(0).position);
                m_offset = new Vector2(transform.position.x, transform.position.y) - touchPos;
                m_firstTouch = true;
            }
        }

        public void Revert()
        {
            m_shapeModule.shapeType = ParticleSystemShapeType.Hemisphere;
            this.transform.localScale = m_oriScale;
            this.transform.position = m_oriPos;
        }

        public void UnRegisterQuest()
        {
            QuestController.Instance.UnRegisterQuest(gameObject.ToString());
        }
    }
}
