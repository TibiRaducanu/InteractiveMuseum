using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserControlledKnight : KnightController
{
    public Joystick joystick;
    readonly float MOVEMENT_THRESHOLD = 0.2f;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        
        HandleJoystick();
    }

    // Handle rotation and walking animation
    void HandleJoystick()
    {
        Vector2 dir = joystick.Direction;
        bool movingX = Math.Abs(dir.x) >= MOVEMENT_THRESHOLD,
            movingY = Math.Abs(dir.y) >= MOVEMENT_THRESHOLD;

        if (movingX || movingY)
        {
            // Rotate the knight according to the joystick angle
            float angle = (float) Math.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

            Rotate(angle);
            WalkForward();
        }
        else
        {
            // The joystick is not moving, back to idle animation
            _anim.SetBool(WalkBoolName, false); 
        }
    }
}
