using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChecker : MonoBehaviour
{
    [SerializeField] public KnightController enemy;
    
    public float viewAngle = 45f;
    public float viewDistance = 5f;

    // Enemy got killed
    public void RemoveEnemy()
    {
        enemy = null;
    }
    
    public bool CheckEnemyInSight()
    {
        if (enemy == null)
        {
            return false;
        }

        // Get the vector from us to the enemy
        Vector3 enemyPos = enemy.transform.position,
            thisPos = transform.position,
            enemyDir = enemyPos - thisPos;
        
        // Check the angle between us and the enemy
        float angle = Mathf.Acos(Vector3.Dot(transform.forward, enemyDir.normalized)) * Mathf.Rad2Deg;

        // The enemy is out of view range 
        if (angle > viewAngle || enemyDir.magnitude > viewDistance)
        {
            return false;
        }
        
        return true;
    }
}
