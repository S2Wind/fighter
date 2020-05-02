using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfigs
{
    static float health;
    static float speed;
    static float maxHealth;

    public static float Health { get => health; set => health = value; }
    public static float Speed { get => speed; set => speed = value; }
    public static float MaxHealth { get => maxHealth; set => maxHealth = value; }
}
