using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AttackStatesUI : MonoBehaviour
{
    public TMP_Text AttackStatesText;
    PlayerStates playerStates;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); //player検索

        if(player == null)
        {
            Debug.Log("No player found in scene. Make sure it has tag 'Player'.");
        }

        playerStates = player.GetComponent<PlayerStates>();   //Damageableコンポネントを取得
    }

    // Start is called before the first frame update
    void Start()
    {
        AttackStatesText.text = "ATK " + (playerStates.ReinforcedDamage + 5);
    }

    private void OnEnable() //OnEnable関数は、オブジェクトが有効(アクティブ)になったときに呼び出される
    {
        playerStates.atkChanged.AddListener(OnPlayerAttackChanged);
    }

    private void OnDisable()
    {
        playerStates.atkChanged.RemoveListener(OnPlayerAttackChanged);
    }

    private void OnPlayerAttackChanged(int newAttack)
    {
        AttackStatesText.text = "ATK " + (playerStates.ReinforcedDamage + 5);
    }
 
}
