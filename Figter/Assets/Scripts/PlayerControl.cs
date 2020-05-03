using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] LayerMask layerMask;

    Animator anmt;
    Rigidbody2D rb;
    BoxCollider2D box;

    Vector3 pos;
    float xVal;
    float timer;

    bool isJumping;
    bool isSliding;

    void Start()
    {
        isJumping = false;
        isSliding = false;
        anmt = GetComponentInParent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        //Test
        IsGround();
        //isClimbing();
    }

    private void Movement()
    {
        xVal = Input.GetAxis("Horizontal");

        pos = new Vector3(xVal, 0f);



        //Jumping and Sliding

        JumpAndSlide();

        //Climb


        anmt.SetFloat("x", xVal);

        //Flip Sprite
        FlipX();

        transform.position += pos * Time.deltaTime * speed;
    }

    private void JumpAndSlide()
    {
        if (Input.GetKeyDown(KeyCode.W) && IsGround())
        {
            isJumping = true;
            rb.velocity = new Vector2(0, 9f);
            anmt.SetFloat("y", 10f);
            Debug.Log(1);
        }
        else if (Input.GetKeyUp(KeyCode.W) && !IsGround())
        {
            if (rb.velocity.y > 0)
                rb.velocity = Vector2.zero;
            Debug.Log(2);
        }
        else if (Input.GetKeyDown(KeyCode.S) && !isSliding)
        {
            isSliding = true;
            anmt.SetFloat("y", -10f);
            rb.velocity = new Vector2(3f * Mathf.Sign(xVal), -2f);
            timer = 0;
            Debug.Log(3);
        }
        else if (Input.GetKeyUp(KeyCode.S) && isSliding)
        {
            anmt.SetFloat("y", 0f);
            isSliding = false;
            Debug.Log(4);
        }
        else if (Input.GetKey(KeyCode.S) && Mathf.Abs(rb.velocity.x) < 1f)
        {
            timer += Time.deltaTime;
            pos.x = xVal - (0.6f * xVal) * Mathf.Clamp(timer, 0, 2f) / 2;
        }
    }

    private void FlipX()
    {
        if (xVal < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (xVal > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }



    bool IsGround()
    {
        float height = 0.01f;
        RaycastHit2D cast2d = Physics2D.BoxCast(box.bounds.center + Mathf.Sign(xVal)*new Vector3(-0.1f,0f,0f)
                            , box.bounds.size - new Vector3(0.2f,-height,0f), 0f, Vector2.down,height,layerMask);
        Color rayColor;
        if (cast2d.collider != null)
            rayColor = Color.green;
        else
            rayColor = Color.red;
        Debug.DrawRay(box.bounds.center + new Vector3(box.bounds.extents.x,0f,0f),Vector2.down * (box.bounds.extents.y + height),rayColor);
        Debug.DrawRay(box.bounds.center - new Vector3(box.bounds.extents.x, 0f, 0f), Vector2.down * (box.bounds.extents.y + height), rayColor);
        Debug.DrawRay(box.bounds.center - new Vector3(box.bounds.extents.x, box.bounds.extents.y+height, 0f), Vector2.right * (box.bounds.extents.x*2), rayColor);
        return cast2d.collider != null;
    }

    bool isClimbing()
    {
        float length = box.bounds.extents.x+0.1f;
        RaycastHit2D cast2D = Physics2D.Raycast(box.bounds.center, Vector2.right * Mathf.Sign(xVal),length,layerMask);
        Color rayColor;
        if (cast2D.collider != null)
            rayColor = Color.green;
        else
            rayColor = Color.red;
        Debug.DrawRay(box.bounds.center, Vector2.right * Mathf.Sign(xVal) * length, rayColor);
        return cast2D.collider != null;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Platform" && anmt.GetFloat("y") > 0 )
            anmt.SetFloat("y", 0f);
    }
}
  