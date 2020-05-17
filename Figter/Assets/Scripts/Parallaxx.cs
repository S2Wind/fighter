using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxx : MonoBehaviour
{

    [SerializeField] Transform cam;

    [SerializeField] float xParallax;

    [SerializeField] float yParallax;

    Vector3 previousCam;

    float extent;

    float length;

    void Start()
    {
        previousCam = cam.position;
        extent = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        transform.position += new Vector3 ((cam.position.x - previousCam.x)*xParallax,(previousCam.y - cam.position.y)*yParallax,0f) ;
        previousCam = cam.position;

        length = cam.position.x - transform.position.x;
        if (length >= extent)
            transform.position += new Vector3(extent, 0f, 0f);
        else if(length <= -extent)
            transform.position += new Vector3(-extent, 0f, 0f);
    }

}
