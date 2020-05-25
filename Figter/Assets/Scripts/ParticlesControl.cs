using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesControl : MonoBehaviour
{
    [SerializeField] Vector3 dirUp;

    [SerializeField] Vector3 dirDown;

    [SerializeField] Vector3 dirRight;

    [SerializeField] Vector3 dirLeft;

    [SerializeField] GameObject player;

    [SerializeField] ParticleSystem particle;


    public enum Direction {Up , Right , Down , Left};
    public Direction dir;

    public Direction Dir { get => dir; set => dir = value; }

    void Start()
    {
        if (player.GetComponent<SpriteRenderer>().flipX)
            Dir = Direction.Left;
        else
            Dir = Direction.Right;

        particle = GetComponent<ParticleSystem>();
    }

    public void SetDir(string s)
    {
        var par = particle.shape;
        if(s == "R")
        {
            par.rotation  = dirRight;
        }
        else if(s == "L")
        {
            par.rotation = dirLeft;
        }
        else if(s == "D")
        {
            par.rotation = dirDown;
        }
        else if(s == "U")
        {
            par.rotation = dirUp;
        }   
    }

    public void SetPos(Vector3 pos)
    {
        transform.localPosition = pos;
    }
}
