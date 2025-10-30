using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))] //Rigidbody2Dがあるか最初に確認する

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5.0f;
    public float runSpeed = 8.0f;
    public float jumpImpulse = 10.0f;
    public float airWalkSpeed = 0.1f;
    float airMaxVelocity = 0;
    bool onJumping;
    bool onAttacking;
    public float attackImpulse = 5.0f;
    public float attackFriction = 0.5f;
    public bool gameEnd = false;

    Vector2 moveInput;
    TouchingDirections touchingDirections;
    Damageable damageable;
    //DiamondFlower gooalObject;

    public float CurrentMoveSpeed   //プロパティを呼び出したとき、移動速度を呼び出す。
    {
        get
        {
            if(CanMove && !gameEnd)
            {
                if(IsMoving && ((!touchingDirections.IsOnWall) || (touchingDirections.IsOnWall && touchingDirections.IsGrounded)))
                {
                    if(touchingDirections.IsGrounded)
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
                    else
                    {
                        //空中の移動速度
                        return airWalkSpeed;
                    }
                }
                else
                {
                    //止まってるときは速度0
                    return 0;
                }
            }
            else
            {
                //CanMoveがfalseの時、動けなくする。
                return 0;
            }
        }
    }

    [SerializeField]
    private bool _isMoving = false;
    //プロパティを用いることで、privateの変数の値を外部から参照できるようになる。

    public bool IsMoving    //プロパティを宣言。プロパティそのものは値を保持できないため別に変数を用意する。今回は、_isMovingに代入している。
    {   //値を代入、参照するとき、何らかのコードを実行するのがプロパティ。
        get
        {   //getはプロパティから取り出す値。//IsMovingを呼び出したら、_isMovingの値を返す。
            return _isMoving;  
        }
        private set
        {   //IsMovingの値を_isMovingにset(代入)する。
            _isMoving = value;   //設定する値はvalueで参照できる。
            animator.SetBool(AnimationStrings.isMoving, value);    //_isMovingに値を代入すると共に、このメソッドを実行することができる。
        }
    }

    [SerializeField]
    private bool _isRunning = false;
    
    public bool IsRunning
    {   
        get
        {   
            return _isRunning;  
        }
        private set
        {   
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value); 
        }
    }

    public bool _isFacingRight = true;

    public bool IsFacingRight 
    {
        get
        {
            return _isFacingRight;
        }
        private set
        {
            if(_isFacingRight != value) //_isFacingRightの値がIsFacingRightの値と違っていたら
            {
                //画像を反転
                transform.localScale *= new Vector2(-1, 1);
            }   
            _isFacingRight = value; 
        }
        
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool IsAlive 
    {
        get { return animator.GetBool(AnimationStrings.isAlive); }

    }

    Rigidbody2D rb;
    Animator animator;

    private void Start()    //Awakeはこのスクリプトが読み込まれた直後に実行、Startより早い
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
        //gooalObject = GameObject.FindWithTag("Goal").GetComponent<DiamondFlower>();
    }

    private void FixedUpdate()
    {
        if(CanMove && !gameEnd) SetFacingDirection(moveInput);

        if(!damageable.LockVelocity)
        {
            //プレイヤーの地上移動。
            if(touchingDirections.IsGrounded)
            {
                if(onAttacking)
                {
                    rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, attackFriction) ,rb.velocity.y);
                    if(CanMove && !gameEnd) onAttacking = false;
                }
                else
                {
                    rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
                }

                //ジャンプ後の慣性判定
                if(onJumping)
                {
                    airMaxVelocity = rb.velocity.x / 1.2f;
                    //ジャンプして空中にでたら、慣性がそこそこ乗る
                }
                else
                {
                    airMaxVelocity = rb.velocity.x/3; 
                    //ジャンプせずに空中にでたら、移動速度の半分の慣性が乗る
                }
                onJumping = false;  //地に足付いたらジャンプ判定オフ
            }
            else    //プレイヤーの空中移動
            {
                if(airMaxVelocity > 0)
                {
                    airMaxVelocity -= Time.deltaTime/2;   //空中にいるとき、少しずつ慣性がなくなる
                    if(airMaxVelocity < 0 || !_isFacingRight || touchingDirections.IsOnWall)    //慣性が0以下、左を向く、壁にぶつかったら慣性を0にリセット。
                    {
                        airMaxVelocity = 0;
                    }
                }
                else
                {
                    airMaxVelocity += Time.deltaTime * 2;
                    if(airMaxVelocity > 0 || _isFacingRight || touchingDirections.IsOnWall)
                    {
                        airMaxVelocity = 0;
                    }
                }

                rb.velocity = new Vector2(airMaxVelocity + moveInput.x * CurrentMoveSpeed, rb.velocity.y);
            }
        }
        else if(!IsAlive || gameEnd)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            
        }


        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y); //yの速度をanimationに反映
    }

    public void OnMove(InputAction.CallbackContext context) //OnMoveメソッドを呼び出す。Callbackに代入できるようにpublicにする。
    {
        moveInput = context.ReadValue<Vector2>();

        if(IsAlive)
        {
            //moveInputが0でない(動いている)場合、true, 0(動いてない)の場合false
            IsMoving = moveInput != Vector2.zero;
        }
        else
        {
            IsMoving = false;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if(!IsFacingRight && moveInput.x > 0)
        {
            //Face the right
            IsFacingRight = true;
        }
        else if(IsFacingRight && moveInput.x < 0)
        {
            //Face the left
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if(context.started)//startedで押され始める
        {
            IsRunning = true;
        }
        else if(context.canceled)   //canceledで離す。
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //To check if alive as well
        if(context.started && touchingDirections.IsGrounded && CanMove && !gameEnd)    //スペースキーが押された＆地面に触れていたら && 動ける状態だったら
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);  //AddForceは力を加える、velocityは加速度をそのまま変更する(重力を無視して加える)

            onJumping = true;   //ジャンプ状態にする
        }

        if (rb.velocity.y > 0f && context.canceled)
        {
            //ジャンプ中にスペースキーを離したら
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    } 

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started && CanMove && !gameEnd)  //touchingDirections.IsGrounded && 
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);

            /*if(IsFacingRight) rb.velocity = new Vector2(rb.velocity.x * attackImpulse, rb.velocity.y);
            else rb.velocity = new Vector2(rb.velocity.x * attackImpulse, rb.velocity.y);*/
            
            onAttacking = true;
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        
    }

}
