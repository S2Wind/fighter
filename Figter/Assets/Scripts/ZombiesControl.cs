using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiesControl : MonoBehaviour
{

    [Header("Link")] 

    [SerializeField] BoxCollider2D box;

    [SerializeField] LayerMask layerMask;

    [SerializeField] Transform player;

    [SerializeField] Transform body;

    [SerializeField] UIManager uiManager;

    [SerializeField] PlayerHealth playerHealth;

    [Header("Configs")]

    [SerializeField] float speed;

    [SerializeField] float distanceForChase;

    [SerializeField] float distanceForAttack;

    [SerializeField] float attackAproachSpeed;

    [SerializeField] float dameToPlayer;


    [Header("Aria")]

    [SerializeField] float angle;

    [SerializeField] float distance;

    [SerializeField] Vector2 dir;

    [SerializeField] Vector2 size;

    [Header("Attact Aria")]

    [SerializeField] float attackAngle;

    [SerializeField] Vector2 attackDir;

    [SerializeField] Vector2 attackSize;

    [SerializeField] float attackDis;


    Vector3 vec;
    RaycastHit2D aria;
    Animator anmt;
    float xDir;
    SpriteRenderer zombies;
    PlayerControl playerControl;
    
    /* 1. Idle
     * 2. Chase
     * 3. Attack
     * 4. Dead
     */
    int attitude;
    int idle = 1;
    int chase = 2;
    int attack = 4;
    int dead = 8;

    private void Start()
    {
        anmt = GetComponent<Animator>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        playerControl = playerHealth.gameObject.GetComponent<PlayerControl>();
        zombies = GetComponentInChildren<SpriteRenderer>();
    }


    void Update()
    {
        //Test;
        RayCastAria();

        //Run
        //Van co Direction tron day
        IfElseAI();

        if (xDir < 0f)
            zombies.flipX = true;
        else
            zombies.flipX = false;

    }

    private void IfElseAI()
    {
        if ((attitude & chase) == chase && playerControl.Attitudes !=32)
        {
            vec = (player.position - body.position);
            xDir = Mathf.Sign(vec.x);

            if (vec.magnitude <= distanceForChase)
            {
                if (vec.magnitude <= distanceForAttack)
                {
                    body.position += vec.normalized * speed*attackAproachSpeed * Time.deltaTime;
                    anmt.SetTrigger("isAttack");
                }
                else
                {
                    body.position += vec.normalized * speed * Time.deltaTime;
                    anmt.SetTrigger("isRun");
                }
            }
            else
            {
                attitude = (attitude ^ chase);
            }
        }
        else
            anmt.SetTrigger("isIdle");
    }

    private bool RayCastAria()
    {
        if (xDir == 0) xDir = 1;
        aria = Physics2D.BoxCast(box.bounds.center, size, angle, dir*xDir,distance, layerMask);
        Color rayColor;
        if (aria.collider != null)
        {
            rayColor = Color.green;
            attitude = attitude | chase;
        }
        else
            rayColor = Color.red;
        Debug.DrawRay(box.bounds.center, dir*size*xDir,rayColor);
        Debug.DrawRay(box.bounds.center + (Vector3)dir*size.x*xDir, new Vector3(0f, size.y / 2, 0f),rayColor);
        Debug.DrawRay(box.bounds.center + (Vector3)dir*size.x*xDir, new Vector3(0f, -size.y / 2, 0f),rayColor);
        return aria.collider != null;
    }

    public void DetechPlayer()
    {
        RaycastHit2D cast2D = Physics2D.BoxCast(box.bounds.center, attackSize, attackAngle, attackDir, attackDis, layerMask);
        if (cast2D.collider != null)
            playerHealth.DamagePlayer(dameToPlayer);
    }

}
