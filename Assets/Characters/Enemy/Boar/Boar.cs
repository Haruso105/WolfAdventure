using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class Boar : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 3f;
    public float runStopRate = 0.2f;
    public float runningTime = 3f;
    float passedTime = 0f; //走り始めてからの経過時間

    public DetectionZone attackZone;    //敵を見つける範囲
    public DetectionZone cliffDetectionZone;

    [SerializeField]
    private bool rightDirection;

    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;
    Damageable damageable;
    EnemyStates enemyStates;

    private Vector2 walkDirectionVector = Vector2.right;

    //複数の定数(書き換える予定のない変数)を一つにまとめて宣言できる。boolに似た感じで、Right or Falseみたいに設定できる(と思う。)
    public enum WalkableDirection { Right, Left }

    private WalkableDirection _walkDirection;

    public WalkableDirection WalkDirection
    {
        get{ return _walkDirection; }
        set
        { 
            if(_walkDirection != value)    //歩いている方角が逆だったら
            {
                //x軸を反転
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if(value == WalkableDirection.Right)    //もし右向きであれば、
                {
                    walkDirectionVector = Vector2.right;    //ベクトルを右
                } 
                else if(value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value; 
        }
    }

    public bool _hasTarget = false;

    public bool HasTarget
    {
        get{ return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
            if(_hasTarget) 
            { 
                IsRunning = true; 
                passedTime = runningTime;
            }
        }
    }

    public bool _isRunning = false;
    public bool IsRunning
    {
        get{ return _isRunning; }
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        
            if(!_isRunning) passedTime = 0;
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public float CurrentSpeed
    {
        get
        {
            if(IsRunning)
            {
                return runSpeed;
            }
            else
            {
                return walkSpeed;
            }
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        enemyStates = GetComponent<EnemyStates>();
    }

    private void Start()
    {
        if(rightDirection){ WalkDirection = WalkableDirection.Right; }
        else{ WalkDirection = WalkableDirection.Left; }
    }

    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;

        if(IsRunning)
        {
            passedTime -= Time.deltaTime;
            if(passedTime < 0)
            {
                IsRunning = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if(touchingDirections.IsGrounded && touchingDirections.IsOnWall) 
        {
            FlipDirection();
        }

        if(!damageable.LockVelocity && enemyStates.isLoaded)
        {
            if(CanMove) {rb.velocity = new Vector2(CurrentSpeed * walkDirectionVector.x, rb.velocity.y); }
            else
            {
            if(HasTarget)   //見つけた時にその場にとどまる
                rb.velocity = new Vector2(0, rb.velocity.y);
            else    //見失って立ち止まるとき、滑りながら止まる
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, runStopRate), rb.velocity.y);
            }
        }
    }

    private void FlipDirection()
    {
        if(WalkDirection == WalkableDirection.Right)    //右に進んでいるとき壁にぶつかったら
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if(WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("Walkable Direction に正しい値が代入されていないよ～><");
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, knockback.y + rb.velocity.y);
    }

    public void OnCliffDetected()
    {
        if(touchingDirections.IsGrounded && !IsRunning)
            FlipDirection();
    }

}
