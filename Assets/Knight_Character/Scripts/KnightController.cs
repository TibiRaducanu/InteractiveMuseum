using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(EnemyChecker))]
public class KnightController : MonoBehaviour
{
    public float maxHealth = 100f;
    public float speed = 80f;
    public float damagePts = 35f;
    
    public float attackTime = 1f;

    private float _lastAttackTime;

    private float _currHealth;

    protected bool _isAttacking = false;
    protected bool _isColliding;
    
    protected Animator _anim;
    protected EnemyChecker enemyChecker;

    protected bool isDead = false;
    
    // Constants
    protected const string AttackAnim = "Attack";
    protected const string AttackTrigName = "isAttacking";
    protected const string WalkBoolName = "isWalking";

    public Image healthBar;

    public Canvas healthBarCanvas;
    
    // Start is called before the first frame update
    protected void Start()
    {
        _isColliding = false; 
        _currHealth = maxHealth;
        
        _anim = GetComponent<Animator>();
        enemyChecker = GetComponent<EnemyChecker>();

        healthBarCanvas = GetComponentInChildren<Canvas>();

        _lastAttackTime = Time.time;
    }

    // Update is called once per frame
    protected void Update()
    {
        CheckAttackAnim();
        
        // Rotate the health bar such that it always points to the camera
        healthBarCanvas.transform.LookAt(-Camera.main.transform.forward);
    }

    // Update the length of the health bar
    private void UpdateHealthBar()
    {
        healthBar.fillAmount = _currHealth / maxHealth;
    }

    // The character walks forward if it isn't currently attacking or colliding with another knight
    protected void WalkForward()
    {
        if (_isAttacking || _isColliding)
        {
            return;
        }
        
        transform.position = transform.position + transform.forward * (speed * Time.deltaTime);
        
        // Play the walking animation
        _anim.SetBool(WalkBoolName, true);
    }

    // If an attack began, check continuously for the animation end 
    protected void CheckAttackAnim()
    {
        // While attacking the character should stop moving forward
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName(AttackAnim))
        {
            _isAttacking = true;
        }
        else
        {
            _isAttacking = false;
        }
    }
    
    // Rotate the actor along it's local y axis
    protected void Rotate(float angle)
    {
        var currRotation = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(currRotation.x, angle, currRotation.z);

        // If we no longer have the enemy in sight we consider we are not colliding with it
        if (!enemyChecker.CheckEnemyInSight())
        {
            _isColliding = false;
        }
    }

    // Attack the enemy 
    // -> play the attacking anim
    // -> if the enemy is in range damage him
    public void Attack()
    {
        // Don't attack the enemy if already dead
        if (isDead)
        {
            return;
        }

        if (Time.time - _lastAttackTime < attackTime)
        {
            return;
        }

        _lastAttackTime = Time.time;

        _anim.SetTrigger(AttackTrigName);

        if (enemyChecker.CheckEnemyInSight())
        {
            // the enemy got killed, remove it
            if (enemyChecker.enemy.Damage(damagePts))
            {
                enemyChecker.enemy.isDead = true;
                enemyChecker.RemoveEnemy();
            }
        }
    }

    // Get damaged by an enemy and return if getting killed
    public bool Damage(float damageTaken)
    {
        this._currHealth -= damageTaken;
        UpdateHealthBar();

        if (this._currHealth <= 0)
        {
            Hide();
            return true;
        }
        
        return false;
    }

    private void Hide()
    {
        foreach (var render in GetComponentsInChildren<Renderer>())
        {
            render.enabled = false;
        }
        
        // also hide the health bar
        healthBarCanvas.enabled = false;
    }

    // Check onTrack to see if not dead, otherwise hide
    public void OnImageTracked()
    {
        if (isDead)
        {
            Hide();
        }
    }
    
    private void OnCollisionEnter()
    {
        _isColliding = true;
    }
    
    private void OnCollisionExit()
    {
        _isColliding = false;
    }
}
