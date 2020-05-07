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
    SpriteRenderer sprite;


    Vector3 pos;
    Vector3 climbPos;
    float xVal;
    float xDir;
    float yVal;
    float timer;


    int attitudes;

    int idle = 1;
    int run = 2;
    int jump = 4;
    int slide = 8;
    int climbMode = 16;
    int dead = 32;

    int climbType;

    public int Attitudes { get => attitudes; set => attitudes = value; }

    void Start()
    {
        Attitudes = 1;
        anmt = GetComponentInParent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attitudes != dead)
        {
            //if (attitudes != climbMode)
            //{
                Movement();
                //Test
                IsGround();
                isClimbing();
            //}
            //else
            //{
            //    Climbing();
            //}
        }

    }

    private void Movement()
    {
        xDir = Input.GetAxis("Horizontal");
        //xTemp for dir 
        //xVal for value


        xVal = xDir;
        //Jumping and Sliding

        JumpAndSlide();


        pos = new Vector3(xVal * Time.deltaTime * speed,yVal, 0f);

        anmt.SetFloat("x", xVal);

        //Flip Sprite
        FlipX();

        transform.position += pos;
    }

    //2 type 
    // climb Mode , can climb in actual mean
    // climb by hold , just slow down the fall
    private void Climbing()
    {
        //xVal = Input.GetAxis("Horizontal");
        //yVal = Input.GetAxis("Vertical");
        //xDir = (sprite.flipX == false) ? 1 : -1 ;
        //pos = new Vector3(xVal , yVal, 0f) * Time.deltaTime * speed;
        //if (climbType == 1)
        //{
        //rb.gravityScale = 0;
        //transform.position += pos;
        //    attitudes = idle;
        //}
        //else
        //{
        anmt.SetFloat("x", 1f);
        anmt.SetTrigger("isClimb");


        //Climbing Ability here

        if (rb.velocity.y < 0)
            rb.gravityScale = 0.2f;
        if (IsGround())
        {
            rb.gravityScale = 1f;
            anmt.SetFloat("x", 0f);
            attitudes = idle;
        }
        //}
        //suy xet lại thì thay đổi graviti của player là mượt nhất .chưa tính toán được .    
        //    }
    }

    private void JumpAndSlide()
    {
        if (Input.GetKeyDown(KeyCode.W) && IsGround() )
        {
            // s = (v1^2) - 2*g
            // 4 ô   
            rb.velocity = new Vector2(0, 9f);
            anmt.SetFloat("y", 10f);
            Attitudes = jump;
        }
        else if (Input.GetKeyUp(KeyCode.W) && !IsGround())
        {
            if (rb.velocity.y > 0)
                rb.velocity = Vector2.zero;
            attitudes = 1;
        }
        else if (Input.GetKeyDown(KeyCode.S) && (attitudes & slide) == 0)
        {
            anmt.SetFloat("y", -10f);
            if(!Mathf.Approximately(xDir,0f))
                rb.velocity = new Vector2(3f * Mathf.Sign(xDir), -2f);
            timer = 0;
            Attitudes = slide;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            anmt.SetFloat("y", 0f);
            attitudes = idle;
        }
        else if (Input.GetKey(KeyCode.S) && Mathf.Abs(rb.velocity.x) < 1f)
        {
            timer += Time.deltaTime;
            xVal = xDir - (0.6f * xDir) * Mathf.Clamp(timer, 0, 2f) / 2;
        }
    }

    private void FlipX()
    {
        if (xDir < 0)
        {
            sprite.flipX = true;
        }
        else if (xDir > 0)
        {
            sprite.flipX = false;
        }
    }



    bool IsGround()
    {
        float height = 0.01f;
        RaycastHit2D cast2d = Physics2D.BoxCast(box.bounds.center + Mathf.Sign(xDir)*new Vector3(-0.1f,0f,0f)
                            , box.bounds.size - new Vector3(0.2f,-height,0f), 0f, Vector2.down,height,layerMask);
        Color rayColor;
        if (cast2d.collider != null)
        {
            if (rb.velocity.y < 0 && anmt.GetFloat("y")>=0)
            {
                anmt.SetFloat("y", 0f);
                anmt.SetTrigger("doneClimb");
            }
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(box.bounds.center + new Vector3(box.bounds.extents.x,0f,0f),Vector2.down * (box.bounds.extents.y + height),rayColor);
        Debug.DrawRay(box.bounds.center - new Vector3(box.bounds.extents.x, 0f, 0f), Vector2.down * (box.bounds.extents.y + height), rayColor);
        Debug.DrawRay(box.bounds.center - new Vector3(box.bounds.extents.x, box.bounds.extents.y+height, 0f), Vector2.right * (box.bounds.extents.x*2), rayColor);
        return cast2d.collider != null;
    }

    bool isClimbing()
    {
        float length = box.bounds.extents.x+0.3f;
        int dir;
        if (sprite.flipX == true)
            dir = -1;
        else
            dir = 1;
        RaycastHit2D cast2D = Physics2D.Raycast(box.bounds.center, Vector2.right * Mathf.Sign(dir),length,layerMask);
        Color rayColor;
        if (cast2D.collider != null)
        {
            attitudes = climbMode;
            if (cast2D.collider.tag == "Climbable")
                climbType = 1;
            else
                climbType = 2;
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(box.bounds.center, Vector2.right * Mathf.Sign(dir) * length, rayColor);
        if (!IsGround() && cast2D.collider != null)
            return true;
        else
            return false;
    }
}
  