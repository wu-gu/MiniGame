using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision"+collision.gameObject.ToString());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger" + collision.gameObject.ToString());
    }
}
