using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glowworm : MonoBehaviour, QuestBehavior
{
    private ParticleSystem m_particleSystem;
    private ParticleSystem.ShapeModule m_shapeModule;
    private Vector3 m_oriPos;
    private Vector3 m_newScale;
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
        // -- PC stable --          -- Android untested yet --                                       
        if(Input.GetMouseButton(0) /*Input.touchCount > 0*/)
        {
            if (m_firstTouch)
            {
                m_shapeModule.shapeType = ParticleSystemShapeType.SingleSidedEdge;
                this.transform.localScale = m_newScale;
                m_particleSystem.Simulate(2f, false, false, true);
                m_particleSystem.Play();
                m_firstTouch = false;
            }
            // -- PC stable --          -- Android untested yet --          
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition /*Input.GetTouch(0).position*/);
            this.transform.position = new Vector3(touchPos.x + m_offset.x, touchPos.y + m_offset.y, this.transform.position.z);
        }
        else
        {

            Vector2 currPos = this.transform.position;


            if (Vector2.Distance(currPos, m_lanternL.bounds.center) < m_lanternL.bounds.extents.x)
            {
                m_licht.LichtLeft();
            }
            if (Vector2.Distance(currPos, m_lanternR.bounds.center) < m_lanternR.bounds.extents.x)
            {
                m_licht.LichtRight();
            }
            if (Vector2.Distance(currPos, m_lanternM.bounds.center) < m_lanternM.bounds.extents.x)
            {
                m_licht.LichtMid();
            }

            if (m_licht.Revert())
            {
                Revert();
            }
            if(m_licht.Reset())
            {
                GameObject glowworm = GameObject.Find("Glowworm_1");
                glowworm.GetComponent<Glowworm>().Revert();
                glowworm = GameObject.Find("Glowworm_2");
                glowworm.GetComponent<Glowworm>().Revert();
                glowworm = GameObject.Find("Glowworm_3");
                glowworm.GetComponent<Glowworm>().Revert();
            }
            
            this.enabled = false;
        }

    }

    public void OnUpdate()
    {
        if (this.transform.position != m_oriPos) // Not reverted or reset -- reach a possible target
            m_lockMovement = true;
        else
            m_lockMovement = false;

        // -- PC stable --          -- Android untested yet --          
        if (!m_lockMovement /*&& Input.GetTouch(0).phase == TouchPhase.Began*/)
        {
            this.enabled = true;
            // -- PC stable --          -- Android untested yet --          
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition /*Input.GetTouch(0).position*/);
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
