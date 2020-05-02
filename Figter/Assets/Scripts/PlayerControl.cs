using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float speed;

    Animator anmt;

    Vector3 pos;
    float xVal, yVal;


    void Start()
    {
        anmt = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        xVal = Input.GetAxis("Horizontal");
        yVal = Input.GetAxis("Vertical") * 3f;
        pos = new Vector3(xVal,Mathf.Clamp(yVal,0f,1f)) * speed * Time.deltaTime;

        anmt.SetFloat("x", xVal);
        anmt.SetFloat("y", yVal);

        if (xVal < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (xVal > 0) 
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false; 
        }

        
        transform.position += pos;
    }  
}
