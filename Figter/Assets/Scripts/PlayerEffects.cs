using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerEffects
{
    static int  curAtt;

    static int  idle = 1;
    static int  run = 2;
    static int  jump = 4;
    static int  slide = 8;
    static int  climb = 16;
    static int  dead = 32;

    public static IEnumerator StunPlayer(PlayerControl player,float time)
    {
        curAtt = PlayerControl.Attitudes;
        player.gameObject.GetComponentInParent<Animator>().SetBool("beStun", true);
        PlayerControl.Attitudes = dead;
        yield return new WaitForSeconds(time);
        player.gameObject.GetComponentInParent<Animator>().SetBool("beStun", false);
        PlayerControl.Attitudes = curAtt;
    }


}
