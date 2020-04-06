using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControlledKnight : KnightController
{
    // How fast should we rotate towards the player
    public float rotationSpeed = 2f;

    private bool _isFollowingPlayer = false;

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        CheckPlayerProximity();
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (!_isFollowingPlayer)
        {
            return;
        }
        
        var player = enemyChecker.enemy;
        if (player == null)
        {
            return;
        }

        Vector3 playerPos = player.transform.position,
            thisPos = transform.position,
            dirToPlayer = playerPos - thisPos;
    
        var rotationToPlayer = Quaternion.LookRotation(dirToPlayer, transform.up);
        transform.rotation = Quaternion.Lerp(rotationToPlayer, transform.rotation, rotationSpeed * Time.deltaTime);

        WalkForward();
    }

    private void CheckPlayerProximity()
    {
        // If we no longer have the enemy in sight we consider we are not colliding with it
        if (!enemyChecker.CheckEnemyInSight())
        {
            _isColliding = false;
        }
        else
        {
            _anim.SetBool(WalkBoolName, false);
            _isColliding = true;
            
            Attack();
        }
    }
    
    // Called on image tracked
    public void StartFolowingPlayer()
    {
        _isFollowingPlayer = true;
    }
    
    // Called on image untracked
    public void StopFolowingPlayer()
    {
        _isFollowingPlayer = false;
    }
}
