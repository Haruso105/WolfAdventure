using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fwower : MonoBehaviour
{
    public bool isRight = true;
    [SerializeField] float revTime = 4f; //反転までの時間
    float passedTime = 0f;
    [SerializeField] float xSpeed = 1f;
    [SerializeField] float ySpeed = 0f;
    [SerializeField] float stopRate = 0.05f;
    [SerializeField] float floatTime = 2f;
    float floatPassedTime = 0f;

    Rigidbody2D rb;
    Animator animator;
    Damageable damageable;
    EnemyStates enemyStates;
    GameObject player;
    SpriteRenderer sprite;

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        enemyStates = GetComponent<EnemyStates>();
        player = GameObject.FindGameObjectWithTag("Player");
        sprite = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyStates.isLoaded) passedTime += Time.deltaTime;

        if(passedTime >= revTime)
        {
            passedTime = 0;
            isRight = !isRight;
        }

        floatPassedTime += Time.deltaTime;
        if(floatPassedTime > floatTime)
        {
            floatPassedTime = 0;
        }

    }

    private void FixedUpdate()
    {
        if(!damageable.LockVelocity && enemyStates.isLoaded && CanMove)
        {
            if(isRight){
                if(passedTime < 2){
                    rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, xSpeed, stopRate), Mathf.Lerp(rb.velocity.y, ySpeed, stopRate));
                } else if(passedTime > revTime - 2) {
                    rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, stopRate), Mathf.Lerp(rb.velocity.y, 0, stopRate));
                } else {
                    rb.velocity = new Vector2(xSpeed, ySpeed);
                }
            } else {
                if(passedTime < 2){
                    rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, -xSpeed, stopRate), Mathf.Lerp(rb.velocity.y, -ySpeed, stopRate));
                } else if(passedTime > revTime - 2) {
                    rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, stopRate), Mathf.Lerp(rb.velocity.y, 0, stopRate));
                } else {
                    rb.velocity = new Vector2(-xSpeed, -ySpeed);
                }
            }

            //ふわふわ浮かせるやつ
            if(floatPassedTime > floatTime /2 ){
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Lerp(rb.velocity.y, 0.5f, stopRate));
            } else {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Lerp(rb.velocity.y, -0.5f, stopRate));
            }

            //常にプレイヤーの方を向く
            if(player.transform.position.x > transform.position.x && !sprite.flipX){
                FlipDirection();
            } else if(player.transform.position.x < transform.position.x && sprite.flipX) FlipDirection();
        }
        else rb.velocity = new Vector2(0, 0);
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, knockback.y + rb.velocity.y);
    }

    private void FlipDirection()
    {
        //gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
        sprite.flipX = !sprite.flipX;
    }
}
