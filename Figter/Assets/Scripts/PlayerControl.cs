using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float speed;

    [SerializeField] LayerMask groundMask;

    [SerializeField] LayerMask climbMask;

    [SerializeField] Vector2 slideVecType1;

    [SerializeField] Vector2 slideVecType2;

    [SerializeField] Vector2 jumpVecType1;

    [SerializeField] Vector2 climbVecBack = new Vector2(3f,4f);

    [SerializeField] Vector2 climbVecForward = new Vector2(0,-2f);

    [SerializeField] Vector2 climbVecUp = new Vector2(0,2f);

    [SerializeField] Vector2 climbVecDown = new Vector2(3f,-4f);

    [SerializeField] Transform climbObj;

    Animator anmt;
    Rigidbody2D rb;
    BoxCollider2D box;
    SpriteRenderer sprite;


    Vector3 pos;
    Vector3 climbPos;
    Vector3 tempPos;
    float xVal;
    float xDir;
    float yVal;
    float timer;
    float extentHeight =0.1f;



    int attitudes;

    int idle = 1;
    int run = 2;
    int jump = 4;
    int slide = 8;
    int climb = 16;
    int dead = 32;

    /*Run type 1 : Walk
     * Type 2 : Run 
     */
    int runType;

    /*Slide Type 1 : Slide on Ground
     * Type 2 : Slide down in the air
     */
    int slideType;
    float slideXVal;

    /*Climb type 1  : Climb for actual
     * type 2 : just hold and slide down
      */
    int climbType;

    /* Jump type 1 : jump
     * Type 2 : fall 
     */
    int jumpType;

    public int Attitudes { get => attitudes; set => attitudes = value; }

    void Start()
    {
        Attitudes = 1;
        anmt = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        if (sprite.flipX == false) xDir = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (attitudes != dead)
        {
            xVal = Input.GetAxis("Horizontal");
            yVal = Input.GetAxis("Vertical");

            if(xDir * xVal < 0)
                FlipX();

            anmt.SetFloat("x", xVal);
           
                


            CheckAttitudes();

            ActiveAttitudes();



            ////if (attitudes != climbMode)
            ////{
            //    Movement();
            //    //Test
            //    IsGround();
            //    isClimbing();
            ////}
            ////else
            ////{
            ////    Climbing();
            ////}
        }
    }

    private void ActiveAttitudes()
    {
        if (attitudes == idle)
        {
            anmt.SetFloat("y", 0f);
            //anmt.SetTrigger("isIdle");
        }
        else if (attitudes == run)
        {
            //anmt.SetFloat("y", 0f);
            //anmt.SetTrigger("isRun");
            anmt.SetFloat("y", 0f);
            MoveFunction();
        }
        else if (attitudes == slide)
        {
            anmt.SetFloat("y", yVal);
            //anmt.SetFloat("y", -10f);
            SlideFunction();
        }
        else if (attitudes == climb)
        {

            //anmt.SetTrigger("isClimb");
            ClimbFunction();
        }
        else if (attitudes == jump)
        {
            anmt.SetFloat("y", rb.velocity.y);
            //anmt.SetFloat("y", 10f);
            JumpFunction();
        }

        if (attitudes != climb && rb.gravityScale !=1f)
            rb.gravityScale = 1f;
    }

    private void CheckAttitudes()
    {
        if (IsGround())
        {
            if (xVal != 0 && Mathf.Approximately(yVal, 0))
            {
                attitudes = run;
            }
            else if (yVal > 0.1f)
            {
                attitudes = jump;
                jumpType = 1;
            }
            else if (yVal < -0.1f)
            {
                attitudes = slide;
                slideType = 1;
            }
            else
            {
                attitudes = idle;
            }
        }
        else
        {
            if (yVal < -0.1f)
            {
                attitudes = slide;
                slideType = 2;
            }
            else if (isClimbing())
            {
                attitudes = climb;
                //Type get in IsClimbing
            }
            else
            {
                attitudes = jump;
                jumpType = 2;
            }
        }
        //Debug.Log(rb.velocity);
    }

    //private void JumpAndSlide()
    //{
    //    if (Input.GetKeyDown(KeyCode.W) && IsGround())
    //    {
    //        // s = (v1^2) - 2*g
    //        // 4 ô   
    //        rb.velocity = new Vector2(0, 9f);
    //        anmt.SetFloat("y", 10f);
    //        Attitudes = jump;
    //    }
    //    else if (Input.GetKeyUp(KeyCode.W) && !IsGround())
    //    {
    //        if (rb.velocity.y > 0)
    //            rb.velocity = Vector2.zero;
    //        attitudes = 1;
    //    }
    //    else if (Input.GetKeyDown(KeyCode.S) && (attitudes & slide) == 0)
    //    {
    //        anmt.SetFloat("y", -10f);
    //        if (!Mathf.Approximately(xDir, 0f))
    //            rb.velocity = new Vector2(3f * Mathf.Sign(xDir), -2f);
    //        timer = 0;
    //        Attitudes = slide;
    //    }
    //    else if (Input.GetKeyUp(KeyCode.S))
    //    {
    //        anmt.SetFloat("y", 0f);
    //        attitudes = idle;
    //    }
    //    else if (Input.GetKey(KeyCode.S) && Mathf.Abs(rb.velocity.x) < 1f)
    //    {
    //        timer += Time.deltaTime;
    //        xVal = xDir - (0.6f * xDir) * Mathf.Clamp(timer, 0, 2f) / 2;
    //    }
    //}


    void SlideFunction()
    {
        //Debug.Log("slide");

        if(Mathf.Abs(rb.velocity.x) <0.1f && !IsGround())
        {
            timer = 0;
            if (slideType == 1)
            {
                rb.velocity += new Vector2(slideVecType1.x*((xVal==0f)?0.01f:xDir),slideVecType1.y);
                //Add animation here
            }
            else
            {
                rb.velocity += new Vector2(slideVecType2.x * ((xVal == 0f) ? 0.01f : xDir), slideVecType2.y);
            }
        }
        else
        {
            Debug.Log(timer);
            timer += Time.deltaTime;
            xVal = xVal * (1f - 0.8f * Mathf.Clamp(timer / 2, 0.01f, 1f));
        }
        MoveFunction();
    }

    void JumpFunction()
    {
        //Debug.Log("jump");
        if (rb.velocity.y < 0.1f )
        {
           if(jumpType == 1)
            {
                rb.velocity = jumpVecType1;
                jumpType = 2;
            }
           else
            {
                rb.velocity = new Vector2(0f,rb.velocity.y);
            }
        }
        else
        {
            if(yVal <0.1f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y/2f);
                anmt.SetFloat("y",10f);
            }
        }
        

        MoveFunction();
    }

    private void ClimbFunction()
    {
        //Debug.Log("Climb");

        if(climbType == 1)
        {
            if (yVal > 0 && xVal == 0)
                rb.velocity = climbVecUp;
            else if (yVal < 0 && xVal == 0)
                rb.velocity = climbVecDown;
            else if (xVal > 0)
                rb.velocity = climbVecForward;
            else if (xVal < 0)
                rb.velocity = climbVecBack;

        }
        else
        {
            if (xDir * xVal >= 0)
            {
                rb.velocity = climbVecDown;
            }
            //if(!IsGround())
            //    rb.gravityScale = 0.2f;
            //MoveFunction();
        }

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
        //anmt.SetFloat("x", 1f);
        //anmt.SetTrigger("isClimb");


        ////Climbing Ability here

        //if (rb.velocity.y < 0)
        //    rb.gravityScale = 0.2f;
        //if (IsGround())
        //{
        //    rb.gravityScale = 1f;
        //    anmt.SetFloat("x", 0f);
        //    attitudes = idle;
        //}
        //}
        //suy xet lại thì thay đổi graviti của player là mượt nhất .chưa tính toán được .    
        //    }
    }

    private void MoveFunction()
    {
        //Debug.Log("Move");

        //More State : 3 State 
           //Walk : 0.3 speed
           //Slightly Run : 0.7Speed 
           //Max Run : max speed 

        pos = new Vector3(xVal, 0f);

        transform.position += pos*Time.deltaTime*speed;

        //xDir = Input.GetAxis("Horizontal");
        ////xTemp for dir 
        ////xVal for value
        ////Jumping and Sliding
        //JumpAndSlide();
        //pos = new Vector3(xVal * Time.deltaTime * speed,yVal, 0f);
        //anmt.SetFloat("x", xVal);
        ////Flip Sprite
        //FlipX();
        //transform.position += pos;
    }


    private void FlipX()
    {
        sprite.flipX = !sprite.flipX;
        xDir = -xDir;
    }
    bool IsGround()
    {
        float height = 0.01f;
        RaycastHit2D cast2d = Physics2D.BoxCast(box.bounds.center + Mathf.Sign(xDir)*new Vector3(-0.1f,0f,0f)
                            , box.bounds.size - new Vector3(0.2f,-height,0f), 0f, Vector2.down,height,groundMask);
        Color rayColor;
        if (cast2d.collider != null)
        {
            if (rb.velocity.y < 0 && anmt.GetFloat("y")>=0)
            {
                anmt.SetFloat("y", 0f);
            }
            rayColor = Color.green;
            if (!anmt.GetBool("Grounded"))
                anmt.SetBool("Grounded", true);
        }
        else
        {
            if (anmt.GetBool("Grounded"))
                anmt.SetBool("Grounded", false);
            rayColor = Color.red;
        }
        Debug.DrawRay(box.bounds.center + new Vector3(box.bounds.extents.x,0f,0f),Vector2.down * (box.bounds.extents.y + height),rayColor);
        Debug.DrawRay(box.bounds.center - new Vector3(box.bounds.extents.x, 0f, 0f), Vector2.down * (box.bounds.extents.y + height), rayColor);
        Debug.DrawRay(box.bounds.center - new Vector3(box.bounds.extents.x, box.bounds.extents.y+height, 0f), Vector2.right * (box.bounds.extents.x*2), rayColor);
        return cast2d.collider != null;
    }

    bool isClimbing()
    {
        float length = box.bounds.extents.x +extentHeight ;
        int dir;
        if (sprite.flipX == true)
            dir = -1;
        else
            dir = 1;
        RaycastHit2D cast2D = Physics2D.Raycast(box.bounds.center, Vector2.right * Mathf.Sign(dir),length,climbMask);
        Color rayColor;
        if (cast2D.collider != null)
        {
            attitudes = climb;
            if (cast2D.collider.tag == "Climbable")
            {
                extentHeight = 0.3f;
                climbType = 1;
            }
            else
            {
                extentHeight = 0.1f;
                climbType = 2;
            }
            if (!anmt.GetBool("Climbed"))
                anmt.SetBool("Climbed", true);

            transform.SetParent(cast2D.collider.transform);
            rayColor = Color.green;
        }
        else
        {
            if (anmt.GetBool("Climbed"))
                anmt.SetBool("Climbed", false);
            transform.SetParent(null);
            rayColor = Color.red;
        }
        Debug.DrawRay(box.bounds.center, Vector2.right * Mathf.Sign(dir) * length, rayColor);
        if (!IsGround() && cast2D.collider != null)
            return true;
        else
            return false;
    }
}
  