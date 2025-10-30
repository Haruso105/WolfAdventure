using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthBarText;
    Damageable playerDamageable;


    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); //player検索

        if(player == null)
        {
            Debug.Log("No player found in scene. Make sure it has tag 'Player'.");
        }

        playerDamageable = player.GetComponent<Damageable>();   //Damageableコンポネントを取得
    }

     // Start is called before the first frame update   
    void Start()
    {
        healthSlider.value = CalculateSliderPercentage(playerDamageable.Health, playerDamageable.MaxHealth);   //パーセントを計算する関数を呼び出す
        healthBarText.text = "HP " + playerDamageable.Health + " / " + playerDamageable.MaxHealth; //healthBarTextのテキストを変更
    }

    private void OnEnable() //OnEnable関数は、オブジェクトが有効(アクティブ)になったときに呼び出される
    {
        playerDamageable.healthChanged.AddListener(OnPlayerHealthChanged);
    }

    private void OnDisable()
    {
        playerDamageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);   //パーセントを計算する関数を呼び出す
        healthBarText.text = "HP " + newHealth + " / " + maxHealth; //healthBarTextのテキストを変更
    }
}
