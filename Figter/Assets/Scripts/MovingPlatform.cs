using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform beforeParent;

    [SerializeField] List<Transform> pos;

    [SerializeField] float stopTime;

    [SerializeField] bool isMoving;

    [SerializeField] float speed;

    [SerializeField] bool[] kindOfMove;

    bool stop;

    int i, j;

    private void Start()
    {
        i = 0;
        stop = true;
    }

    private void Update()
    {
        if (isMoving && !stop)
        {
            transform.position += (pos[j].position - transform.position).normalized * speed * Time.deltaTime;
        }
        else
        {
            if(stop && !isMoving)
                StartCoroutine(LineMove(stopTime));
        }
    }

    IEnumerator LineMove(float stopTime)
    {
        isMoving = true;
        stop = false;
        j = (i + 1 < pos.Count) ? i + 1 : 0;
        yield return new WaitUntil(() => Mathf.Abs((transform.position - pos[j].position).magnitude) <= 0.1f);
        stop = true;
        yield return new WaitForSeconds(stopTime);
        i = j;
        isMoving = false;
        stop = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.collider.transform.SetParent(beforeParent);
        }
    }
}
