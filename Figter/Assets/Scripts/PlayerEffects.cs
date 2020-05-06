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
    static int controlable = 64;

    public static IEnumerator StunPlayer(PlayerControl player,float time)
    {
        curAtt = player.Attitudes;
        player.Attitudes = dead;
        yield return new WaitForSeconds(time);
        player.Attitudes = curAtt;
    }    
}
