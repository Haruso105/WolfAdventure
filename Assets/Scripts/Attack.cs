using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{ 

    public int attackDamage = 10;

    public Vector2 knockback = Vector2.zero;

    private PlayerStates playerStates;

    void Start()
    {
        playerStates = transform.parent.gameObject.GetComponent<PlayerStates>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //攻撃を当てることができるか確認する, ぶつかった相手の情報をdamageableに入れる。
        Damageable damageable = collision.GetComponent<Damageable>();

        if(damageable != null)
        {
            Vector2 deliveredKnockback;
            //ノックバックの方向を反転する。
            deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

            if(playerStates != null)
            {
                //攻撃を当てる
                bool gotHit = damageable.Hit(attackDamage + playerStates.ReinforcedDamage, deliveredKnockback);
            }
            else
            {
                bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);
            }
        }
    }
}
