using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{

    [SerializeField] SpriteRenderer sprite;
    [SerializeField] BoxCollider2D box;

    // Start is called before the first frame update

    public void SetBoxCollider()
    {
        char[] exept = { '_', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        if (sprite.sprite.name.Trim(exept) == "Slide")
        {
            box.offset = BoxCollider2DControl.playerBoxSlideOffset;
            box.size = BoxCollider2DControl.playerBoxSlideSize;
        }
        else
        {
            box.offset = BoxCollider2DControl.playerBoxIdleOffset;
            box.size = BoxCollider2DControl.playerBoxIdleSize;
        }
    }

}
