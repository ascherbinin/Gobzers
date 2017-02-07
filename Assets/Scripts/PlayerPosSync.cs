using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerPosSync : NetworkBehaviour {
    [SyncVar(hook = "FacingCallback")]
    public bool netFacingRight = true;

    private float factor;
    // Use this for initialization
    void Awake()
    {
        factor = transform.localScale.x;
    }

    [Command]
    public void CmdFlipSprite(bool facing)
    {
        Debug.Log("FACING: " + facing);
        netFacingRight = facing;
        if (netFacingRight)
        {
            Vector3 SpriteScale = transform.localScale;
            SpriteScale.x = factor;
            transform.localScale = SpriteScale;
        }
        else
        {
            Vector3 SpriteScale = transform.localScale;
            SpriteScale.x = -factor;
            transform.localScale = SpriteScale;
        }
        
               
        Debug.Log("SCALE: " + transform.localScale);
    }

    void FacingCallback(bool facing)
    {
        netFacingRight = facing;
        if (netFacingRight)
        {
            Vector3 SpriteScale = transform.localScale;
            SpriteScale.x = factor;
            transform.localScale = SpriteScale;
        }
        else
        {
            Vector3 SpriteScale = transform.localScale;
            SpriteScale.x = -factor;
            transform.localScale = SpriteScale;
        }
    }
}
