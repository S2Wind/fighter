using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainControl : MonoBehaviour
{
    PolygonCollider2D pcollider;
    private void Start()
    {
        pcollider = GetComponent<PolygonCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                pcollider.isTrigger = false;
                
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                pcollider.isTrigger = true;
            }
        }
    }
}
