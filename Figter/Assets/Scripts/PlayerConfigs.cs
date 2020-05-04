using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerConfigs
{
    static float health;

    static float maxHealth;

    static bool doubleJump;

    static Vector2 jumpVector = new Vector2(0f, 5f);
    static Vector2 jumpAfterJumpVector = new Vector2(0, 1f);

    static Vector2 slideVector = new Vector2(4f, 0);
    static Vector2 SlideAfterSlideVector = new Vector2(1f, 0);

    public static float Health { get => health; set => health = value; }
    public static float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static Vector2 JumpVector { get => jumpVector; set => jumpVector = value; }
    public static Vector2 SlideVector { get => slideVector; set => slideVector = value; }
    public static Vector2 JumpAfterJumpVector { get => jumpAfterJumpVector; set => jumpAfterJumpVector = value; }
    public static Vector2 SlideAfterSlideVector1 { get => SlideAfterSlideVector; set => SlideAfterSlideVector = value; }
    public static bool DoubleJump { get => doubleJump; set => doubleJump = value; }
}
