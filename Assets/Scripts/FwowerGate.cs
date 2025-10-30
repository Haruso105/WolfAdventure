using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FwowerGate : MonoBehaviour
{
    [SerializeField] GameObject bulletObj;
    [SerializeField] float fireTime = 3f;
    [SerializeField] float fireSpeed = 1f;
    [SerializeField] float rangeLength = 7f;
    GameObject player;
    float bulletPassedTime = 0f;
    [SerializeField] Damageable parentFwower;
    
    
    //距離を確認
    bool CheckLength(Vector2 targetPos)
    {
        bool ret = false;
        //Distance(このオブジェクト、ターゲットオブジェクト)で距離がわかる。
        float d = Vector2.Distance(transform.position, targetPos);
        if(rangeLength >= d)
        {
            ret = true;
        }
        return ret;
    }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        bulletPassedTime += Time.deltaTime;
        if(CheckLength(player.transform.position))
        {
            if(bulletPassedTime > fireTime)
            {
                bulletPassedTime = 0;
                Vector3 direction = (player.transform.position - this.transform.position);
                this.transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);
                GameObject bullet = Instantiate(bulletObj, transform.position, Quaternion.identity);
                //砲身が向いている方向に発射する
                Rigidbody2D rbody = bullet.GetComponent<Rigidbody2D>();
                float angleZ = this.transform.localEulerAngles.z;
                float x = Mathf.Cos(angleZ * Mathf.Deg2Rad);
                float y = Mathf.Sin(angleZ * Mathf.Deg2Rad);
                Vector2 v = new Vector2(x, y) * fireSpeed;
                rbody.AddForce(v, ForceMode2D.Impulse);
                Destroy(bullet, 3.0f);
            }
        }

        if(!parentFwower.IsAlive) Destroy(gameObject);
    }
    // 範囲表示
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, rangeLength);
    }
}
