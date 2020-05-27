using System;
using System.Collections;
using System.Collections.Generic;
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
    static float xDir;
    float yVal;
    float timer;
    float extentHeight =0.1f;


    //lenghtOf

    static bool isGround;
    static int groundedType;
    // type 1 : RayCast for cross ground not null
    //type 2  : RayCast for cross ground null

    static int attitudes;

    static int idle = 1;
    static int run = 2;
    static int jump = 4;
    static int slide = 8;
    static int climb = 16;
    static int dead = 32;

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

    public static int Attitudes { get => attitudes; set => attitudes = value; }
    public static int Idle { get => idle; set => idle = value; }
    public static int Run { get => run; set => run = value; }
    public static int Jump { get => jump; set => jump = value; }
    public static int Slide { get => slide; set => slide = value; }
    public static int Climb { get => climb; set => climb = value; }
    public static int Dead { get => dead; set => dead = value; }
    public static float XDir { get => xDir; set => xDir = value; }
    public static bool IsGround1 { get => isGround; set => isGround = value; }

    void Start()
    {
        Attitudes = 1;
        anmt = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        if (sprite.flipX == false) XDir = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (attitudes != Dead)
        {
            xVal = Input.GetAxis("Horizontal");
            yVal = Input.GetAxis("Vertical");

            if(XDir * xVal < 0)
                FlipX();

            anmt.SetFloat("x", xVal);
           
                


            CheckAttitudes();

            ActiveAttitudes();

        }
    }

    private void ActiveAttitudes()
    {
        if (attitudes == Idle)
        {
            anmt.SetFloat("y", 0f);
            //anmt.SetTrigger("isIdle");
        }
        else if (attitudes == Run)
        {
            //anmt.SetFloat("y", 0f);
            //anmt.SetTrigger("isRun");
            anmt.SetFloat("y", 0f);
            MoveFunction();
        }
        else if (attitudes == Slide)
        {
            //Debug.Log("Slide");
            anmt.SetFloat("y", yVal);
            //anmt.SetFloat("y", -10f);
            SlideFunction();
        }
        else if (attitudes == Climb)
        {

            //anmt.SetTrigger("isClimb");
            ClimbFunction();
        }
        else if (attitudes == Jump)
        {
            anmt.SetFloat("y", rb.velocity.y);
            //anmt.SetFloat("y", 10f);
            JumpFunction();
        }

        if (attitudes != Climb && rb.gravityScale !=1f)
            rb.gravityScale = 1f;
    }

    private void CheckAttitudes()
    {
        if (IsGround() != 0)
        {
            if (xVal != 0 && Mathf.Approximately(yVal, 0))
            {
                attitudes = Run;
            }
            else if (yVal > 0.1f)
            {
                attitudes = Jump;
                jumpType = 1;
            }
            else if (yVal < -0.1f)
            {
                attitudes = Slide;
                slideType = 1;
            }
            else
            {
                attitudes = Idle;
            }
        }
        else
        {
            if (yVal < -0.1f)
            {
                attitudes = Slide;
                slideType = 2;
            }
            else if (isClimbing())
            {
                attitudes = Climb;
                //Type get in IsClimbing
            }
            else
            {
                attitudes = Jump;
                jumpType = 2;
            }
        }
        //Debug.Log(rb.velocity);
    }

    void SlideFunction()
    {
        //Debug.Log("slide");

        if(Mathf.Abs(rb.velocity.x) <0.1f && IsGround() == 1 )
        {
            timer = 0;
            if (slideType == 1)
            {
                rb.velocity += new Vector2(slideVecType1.x*((xVal==0f)?0.01f:XDir),slideVecType1.y);
                //Add animation here
            }
            else
            {
                rb.velocity += new Vector2(slideVecType2.x * ((xVal == 0f) ? 0.01f : XDir), slideVecType2.y);
            }
        }
        else
        {
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
            if (rb.gravityScale == 1)
            {
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0f;
            }
            if (yVal > 0 && xVal == 0)
                rb.velocity = climbVecUp;
            else if (yVal < 0 && xVal == 0)
                rb.velocity = climbVecDown;
            else if (xVal * XDir < 0)
                rb.velocity = climbVecBack;

        }
        else
        {
            if (XDir * xVal >= 0)
            {
                rb.velocity = climbVecDown;
            }
        }

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

    }


    private void FlipX()
    {
        sprite.flipX = !sprite.flipX;
        XDir = -XDir;
    }
    public int IsGround()
    {
        float lenghtOfGroundRay = 0.1f;
        RaycastHit2D cast2d = Physics2D.BoxCast(box.bounds.center + Mathf.Sign(XDir)*new Vector3(-0.1f,0f,0f)
                            , box.bounds.size - new Vector3(0.2f,-lenghtOfGroundRay,0f), 0f, Vector2.down,lenghtOfGroundRay,groundMask);
        RaycastHit2D rayCast2D = Physics2D.Raycast(box.bounds.center, Vector2.down, box.bounds.extents.y + box.bounds.extents.x + lenghtOfGroundRay,groundMask);

        Color rayColor;
        Color raycastColor;
        if (cast2d.collider != null || rayCast2D.collider != null)
        {
            if (rayCast2D.collider != null) raycastColor = Color.green;
            else raycastColor = Color.red;
            if (rb.velocity.y < 0 && anmt.GetFloat("y")>=0)
            {
                anmt.SetFloat("y", 0f);
            }
            rayColor = Color.green;
            if (!anmt.GetBool("Grounded"))
                anmt.SetBool("Grounded", true);
            IsGround1 = true;
        }
        else
        {
            if (anmt.GetBool("Grounded"))
                anmt.SetBool("Grounded", false);
            raycastColor = Color.red;
            rayColor = Color.red;
            IsGround1 = false;
        }
        Debug.DrawRay(box.bounds.center + new Vector3(box.bounds.extents.x,0f,0f),Vector2.down * (box.bounds.extents.y + lenghtOfGroundRay),rayColor);
        Debug.DrawRay(box.bounds.center - new Vector3(box.bounds.extents.x, 0f, 0f), Vector2.down * (box.bounds.extents.y + lenghtOfGroundRay), rayColor);
        Debug.DrawRay(box.bounds.center - new Vector3(box.bounds.extents.x, box.bounds.extents.y+lenghtOfGroundRay, 0f), Vector2.right * (box.bounds.extents.x*2), rayColor);
        Debug.DrawRay(box.bounds.center, Vector2.down * (box.bounds.extents.y + box.bounds.extents.x + lenghtOfGroundRay), raycastColor);
        if(cast2d.collider != null) return 1;
        if (rayCast2D.collider != null) return 2;
        return 0;
    }

    public bool isClimbing()
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
            attitudes = Climb;
            if (cast2D.collider.tag == "Climbable")
            {
                extentHeight = 1f;
                climbType = 1;
            }
            else
            {
                extentHeight = 0.1f;
                climbType = 2;
            }
            if (!anmt.GetBool("Climbed"))
                anmt.SetBool("Climbed", true);

            //transform.SetParent(cast2D.collider.transform);
            rayColor = Color.green;
        }
        else
        {
            extentHeight = 0.1f;
            if (anmt.GetBool("Climbed"))
                anmt.SetBool("Climbed", false);
            //transform.SetParent(null);
            rayColor = Color.red;
            if (rb.gravityScale == 0)
                rb.gravityScale = 1;
        }
        Debug.DrawRay(box.bounds.center, Vector2.right * Mathf.Sign(dir) * length, rayColor);
        if (IsGround() == 0 && cast2D.collider != null)
            return true;
        else
            return false;
    }
}
  