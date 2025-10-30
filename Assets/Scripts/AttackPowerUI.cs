using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;
    public GameObject powerUpTextPrefab;
    public Canvas gameCanvas;

    private void Awake()
    {
        gameCanvas = FindObjectOfType<Canvas>();
    }

    private void OnEnable() //OnEnableはこのスクリプトが付いたObjectが有効になったときに呼び出される。(Awake > OnEnable > Start)の順
    {
        CharacterEvents.characterDamaged += CharacterTookDamage; //Actionに関数を追加
        CharacterEvents.characterHealed += CharacterHealed;
        CharacterEvents.characterPowerUp += CharacterPowerUp;
    }

    private void OnDisable()
    {
        CharacterEvents.characterDamaged -= CharacterTookDamage; //Actionから関数を除外
        CharacterEvents.characterHealed -= CharacterHealed;
        CharacterEvents.characterPowerUp -= CharacterPowerUp;
    }

    public void CharacterTookDamage(GameObject character, int damageReceived)
    {
        //Create text at character hit
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();

        tmpText.text = damageReceived.ToString();
    }

    public void CharacterHealed(GameObject character, int healthRestored)
    {
        //Create text at character hit
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();

        tmpText.text = healthRestored.ToString();

    }

        public void CharacterPowerUp(GameObject character, int reinforceAttack)
    {
        //Create text at character hit
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        TMP_Text tmpText = Instantiate(powerUpTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();

        tmpText.text = reinforceAttack.ToString();

    }
}
