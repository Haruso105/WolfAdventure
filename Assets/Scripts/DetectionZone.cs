using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{
    public UnityEvent noCollidersRemain;

    //Listは配列と近いイメージ、配列とは違い、要素数を気にすることなく要素の追加、削除ができる。    UnityのスクリプトにList一覧を作成できる。
    public List<Collider2D> detectedColliders = new List<Collider2D>(); //要素数は定義する必要がない
    //List<型名> 変数名 = new List<型名> で宣言

    Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }
   
    //当たりに入ったとき
    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedColliders.Add(collision);   
    }

    //当たりから出たとき
    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedColliders.Remove(collision);

        if(detectedColliders.Count <= 0)
        {
            noCollidersRemain.Invoke();
        }
    }
}
