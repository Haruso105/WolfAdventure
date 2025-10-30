using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//体のどこを触っているかを感知する
public class TouchingDirections : MonoBehaviour
{
    
    public ContactFilter2D castFilter;  //特定のオブジェクトを見分けるフィルターをpublicで指定できるようにする。
    //例えば敵のcastFilterにGroundを設定して、use LayerMaskにチェックを入れると、プレイヤーぶつかってもIsOnwallがtrueにならなくなる(Groundだけが判定になる)
    public float groundDistance = 0.05f;    //光線を発射する最大距離
    public float wallDistance = 0.2f;
    public float ceilingDistance = 0.05f;

    CapsuleCollider2D touchingCol;
    Animator animator;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    //RaycastHit2Dは検知されたオブジェクトの情報を返す。
    //型名[] で配列型の変数。今回はRaycastHit2Dの情報を5個まで収容できるようにする。
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    [SerializeField]
    private bool _isGrounded = true;

    public bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }
        set
        {
            _isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    [SerializeField]
    private bool _isOnWall;

    public bool IsOnWall
    {
        get
        {
            return _isOnWall;
        }
        set
        {
            _isOnWall = value;
            animator.SetBool(AnimationStrings.isOnWall, value);
        }
    }

    [SerializeField]
    private bool _isOnCeiling;

    //=>矢印はラムダ式関数(簡単な関数)のことを指す。  wallCheckDirectionのlocalScale.xが0以上なら右向きベクトル、0未満なら左向きベクトルを返す。
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    public bool IsOnCeiling
    {
        get
        {
            return _isOnCeiling;
        }
        set
        {
            _isOnCeiling = value;
            animator.SetBool(AnimationStrings.isOnCeiling, value);
        }
    }

    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //Castは光線を発射してぶつかったオブジェクトの情報を取得。ぶつかった当たりの個数を検知することもできる。
        //地上判定,Cast(発射方向、検知するオブジェクト、ぶつかった物の情報の取得、発射距離)
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0; 
        //下方向に0個以上地面が見つかればTrue

        IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;

    }
}
