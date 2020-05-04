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
    float xDir;
    float yVal;
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
        isClimbing();
    }

    private void Movement()
    {
        xDir = Input.GetAxis("Horizontal");
        //xTemp for dir 
        //xVal for value


        //Jumping and Sliding

        JumpAndSlide();

        //Climb

        if(isClimbing())
        {
            xVal = 0;
            //Climbing Ability here
            rb.gravityScale = 0.2f;
            //suy xet lại thì thay đổi graviti của player là mượt nhất .chưa tính toán được .
        }
        else
        {
            rb.gravityScale = 1f;
            xVal = xDir;
            yVal = 0f;
        }

        pos = new Vector3(transform.position.x + xVal * Time.deltaTime * speed, transform.position.y + yVal ,0f);

        anmt.SetFloat("x", xVal);

        //Flip Sprite
        FlipX();

        transform.position = pos ;
    }

    private void JumpAndSlide()
    {
        if (Input.GetKeyDown(KeyCode.W) && IsGround())
        {
            isJumping = true;
            // s = (v1^2) - 2*g
            // 4 ô 
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
            rb.velocity = new Vector2(3f * Mathf.Sign(xDir), -2f);
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
            pos.x = xDir - (0.6f * xDir) * Mathf.Clamp(timer, 0, 2f) / 2;
        }
    }

    private void FlipX()
    {
        if (xDir < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (xDir > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }



    bool IsGround()
    {
        float height = 0.01f;
        RaycastHit2D cast2d = Physics2D.BoxCast(box.bounds.center + Mathf.Sign(xDir)*new Vector3(-0.1f,0f,0f)
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
        float length = box.bounds.extents.x;
        RaycastHit2D cast2D = Physics2D.Raycast(box.bounds.center, Vector2.right * Mathf.Sign(xDir),length,layerMask);
        Color rayColor;
        if (cast2D.collider != null)
            rayColor = Color.green;
        else
            rayColor = Color.red;
        Debug.DrawRay(box.bounds.center, Vector2.right * Mathf.Sign(xDir) * length, rayColor);
        return cast2D.collider != null;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Platform" && anmt.GetFloat("y") > 0 )
            anmt.SetFloat("y", 0f);
    }
}
  