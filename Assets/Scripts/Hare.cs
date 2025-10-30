using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hare : MonoBehaviour
{

    public float xJumpImpulse;
    public float yJumpImpulse;
    [SerializeField]
    private float jumpTime = 3f;
    private float passedTime = 0f;
    private bool onJumping = false;

    [SerializeField]
    private bool isFacingRight = true;

    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;
    Damageable damageable;
    EnemyStates enemyStates;

    public DetectionZone attackZone;    //プレイヤー検知

    Vector2 walkDirectionVector = Vector2.right;    //右か左か
    public enum WalkableDirection { Right, Left }   //enumは複数の定数(書き換える予定のない変数)を一つにまとめて宣言できる。
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
                passedTime = 0;
            }
        }
    }

    public bool CanMove 
    { 
        get { return animator.GetBool(AnimationStrings.canMove); } 
    }


    public float AttackCooldown
    {
        get
        {
            return animator.GetFloat(AnimationStrings.attackCooldown);
        }
        private set
        {
            animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));    //Mathf.Maxは()内の最大値を返す。この場合、valueが0以下になったら0が最大なので表示される
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

    void Start()
    {
        if(isFacingRight) WalkDirection = WalkableDirection.Right;
        else WalkDirection = WalkableDirection.Left;
    }

    void Update()
    {

        if(touchingDirections.IsGrounded && CanMove)
        {
            passedTime += Time.deltaTime;
        }
        HasTarget = attackZone.detectedColliders.Count > 0;


        if(AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }


    }

    private void FixedUpdate()
    {
        if(touchingDirections.IsOnWall && CanMove) 
        { 
            FlipDirection();
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
        }

        if(!damageable.LockVelocity && CanMove && enemyStates.isLoaded)
        {
            if(touchingDirections.IsGrounded && passedTime > jumpTime) 
            {
                Jump();
            }

            if(touchingDirections.IsGrounded) 
            {
                if(onJumping) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
                else rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
                onJumping = false;
            }
        }
    }

    private void FlipDirection()
    {
        if(WalkDirection == WalkableDirection.Right)    WalkDirection = WalkableDirection.Left;
        else if(WalkDirection == WalkableDirection.Left)    WalkDirection = WalkableDirection.Right;
    }

    private void FrictionStop()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private void Jump()
    {

        rb.velocity = new Vector2(xJumpImpulse * walkDirectionVector.x, yJumpImpulse);
        onJumping = true;
        animator.SetTrigger(AnimationStrings.jumpTrigger);
        passedTime = 0;
        
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, knockback.y + rb.velocity.y);
        Invoke("FrictionStop",0.3f);
        passedTime = 0;
    }
}
