using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fareem : MonoBehaviour
{
    [SerializeField] private float attackRate = 5;
    private float attackTimer = 0f;
    [SerializeField] private int attackType = 0;

    int atkTypeRange = 5;    //攻撃手段の種類
    int n = 0;
    int bfAttackType = 0;

    private bool isAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAttack)
        {
            attackTimer += Time.deltaTime;
        }
        
        if(attackTimer >= attackRate)
        {
            
            bfAttackType = attackType;
            while(n == 0)
            {
                attackType = Random.Range(0,atkTypeRange); //(0,5) 0から4まで
                attackTimer = 0;
                if(attackType != bfAttackType)  break;
            }
            
            switch(attackType)
            {
                case 0:
                    Debug.Log("attack 0");
                    isAttack = true;
                    break;
                case 1:
                    Debug.Log("attack 1");
                    isAttack = true;
                    break;
                case 2:
                    Debug.Log("attack 2");
                    isAttack = true;
                    break;
                case 3:
                    Debug.Log("attack 3");
                    isAttack = true;
                    break;
                case 4:
                    Debug.Log("attack 4");
                    break;
                default:
                    Debug.Log("error");
                    break;
            }
        }
    }

    private void whipAttack()   //attack 0  頭部のツルでムチを打つ
    {
        
    }

    private void rootAttack()   //attack 1 根を張り巡らせる
    {

    }

    private void leafCutter()   //attack 2 葉っぱを飛ばす
    {

    }

    private void sharpBall()    //とげとげボールをとばす
    {

    }
    
    private void rocketBranch() //枝の腕を発射する
    {

    }
}
