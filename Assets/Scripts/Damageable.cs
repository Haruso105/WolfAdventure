using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    //Eventを追加、実行することで他のゲームオブジェクトのスクリプト(コンポネント)を操作/実行することができる。UnityEvent<int, Vector2>で
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent<int, int> healthChanged;
    Animator animator;

    [SerializeField]
    private int _maxHealth = 100;

    public int MaxHealth
    {
        get{ return _maxHealth; }
        set
            {
                _maxHealth = value;
            }
    }

    [SerializeField]
    private int _health = 100;

    public int Health
    { 
        get { return _health; }
        set
        {
            _health = value;
            healthChanged?.Invoke(_health, MaxHealth);  //?.Invokeでnullではない時のみ実行される
            //体力が0以下になったらキャラは死亡
            if(_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;

    public bool IsAlive
    {
        get { return _isAlive; }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive set" + value);
        }
    }

    public bool LockVelocity
    {
        get { return animator.GetBool(AnimationStrings.lockVelocity); }

        set { animator.SetBool(AnimationStrings.lockVelocity, value); }
    }

    [SerializeField]
    private bool isInvincible = false;
    private float timeSinceHit = 0;
    public float invinsibilityTime = 0.25f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        if(isInvincible)
        {
            if(timeSinceHit > invinsibilityTime)
            {
                //無敵解除
                isInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }

        //Hit(10);
    }

    public bool Hit(int damage, Vector2 knockback) //ダメージを受けたら
    {
        if(IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;

            //Invokeは設定した時間に関数を呼び出す。event名.?Invokeと？を入れることで、Nullじゃないかを確認することができる。
            damageableHit?.Invoke(damage, knockback);    

            //ダメージ表示
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            return true;
        }

        return false;
    }

    public bool Heal(int healthRestore)
    {
        if(IsAlive && (Health < MaxHealth))
        {
            int maxHeal = Mathf.Max((int)MaxHealth - (int)Health, 0); //MaxHealは最大体力値と現在の体力の差
            int actualHeal = Mathf.Min(maxHeal, healthRestore);    //回復できる量と、回復アイテムの値、少ない方を選ぶ
            Health += actualHeal;
            //回復量を表示するイベントを呼び出す
            CharacterEvents.characterHealed(gameObject, actualHeal);

            return true;
        }

        return false;
    }
}
