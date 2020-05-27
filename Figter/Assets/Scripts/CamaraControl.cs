using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraControl : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float speed;

    Vector3 pos;
    
    void Awake()
    {
        transform.position = new Vector3(player.position.x,player.position.y,-10f);
    }

    // Update is called once per frame
    void Update()
    {
        pos = new Vector3(player.position.x, player.position.y, -10f) - transform.position;
        if(Mathf.Abs(pos.x) > 0.1f)
            transform.position += pos * Time.deltaTime*speed;
    }
}
