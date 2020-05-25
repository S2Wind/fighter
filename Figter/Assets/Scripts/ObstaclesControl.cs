using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Timeline;

public class ObstaclesControl : MonoBehaviour
{
    [SerializeField] PlayerControl player;

    [SerializeField] PlayerHealth playerhealth;

    [SerializeField] float obstaclesDamage;


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            if (PlayerControl.Attitudes != 32)
            {
                if (playerhealth.Touchable)
                {
                    playerhealth.DamagePlayer(obstaclesDamage);
                    Rigidbody2D rb = player.gameObject.GetComponent<Rigidbody2D>();
                    SpriteRenderer spr = player.gameObject.GetComponent<SpriteRenderer>();
                    playerhealth.Touchable = false;
                    rb.velocity = new Vector2(-2f * ((spr.flipX == false) ? 1f : -1f), 5f * ((rb.velocity.y >= 0) ? 1 : -1));

                    //IEnumbertor // Thieu animtion
                    StartCoroutine(PlayerEffects.StunPlayer(player,2f));
                }
            }

        }
    }
}
