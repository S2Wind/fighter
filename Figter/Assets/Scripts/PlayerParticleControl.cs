using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerParticleControl : MonoBehaviour
{

    [SerializeField] ParticlesControl particle;

    [SerializeField] BoxCollider2D bodyBox;

    Vector3 offsetLeft;

    Vector3 offsetRight;

    Vector3 offsetUpRight;

    Vector3 offsetUpLeft;

    void Start()
    {
        bodyBox = GetComponent<BoxCollider2D>();
        offsetLeft = new Vector3(-bodyBox.bounds.extents.x  , -bodyBox.bounds.extents.y , 0f );
        offsetRight = new Vector3(bodyBox.bounds.extents.x, -bodyBox.bounds.extents.y, 0f);
        offsetUpLeft = new Vector3(-bodyBox.bounds.extents.x, bodyBox.bounds.extents.y, 0f);
        offsetUpRight = new Vector3(bodyBox.bounds.extents.x, bodyBox.bounds.extents.y, 0f);
    }   
    void Update()
    {
        if(((PlayerControl.Attitudes & (PlayerControl.Slide | PlayerControl.Run)) != 0) && PlayerControl.IsGround1)
        {
            if(particle.gameObject.active == false)
            {
                particle.gameObject.SetActive(true);
            }
            if (PlayerControl.XDir > 0)
            {
                particle.SetDir("L");
                particle.SetPos(offsetLeft);
            }
            else if (PlayerControl.XDir < 0)
            {
                particle.SetDir("R");
                particle.SetPos(offsetRight);
            }
            
        }
        else if (PlayerControl.Attitudes == PlayerControl.Climb && !PlayerControl.IsGround1)
        {
            particle.SetDir("U");
            if (PlayerControl.XDir > 0)
            {
                particle.SetPos(offsetUpLeft);
            }
            else if (PlayerControl.XDir < 0)
            {
                particle.SetPos(offsetUpRight);
            }
        }
        else
        {
            if (particle.gameObject.active == true)
            {
                particle.gameObject.SetActive(false);
            }
        }

        
    }
}
