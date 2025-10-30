using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public string sceneName; //読み込むシーン名

    private PlayerStates beforePS;

    private int num;

    void Update()
    {
        if (Input.GetKey (KeyCode.LeftControl) && Input.GetKey (KeyCode.P)) 
        {
            Load();
        }
    }

    public void Load()
    {

        // イベントに登録
        if(beforePS != null)
        {
            num = beforePS.ReinforcedDamage;
        }
        
        SceneManager.sceneLoaded += GameSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {  
        if(collision.tag == "Player")
        {
            beforePS = GameObject.FindWithTag("Player").GetComponent<PlayerStates>();
            Load();
        }
    }

   private void GameSceneLoaded(Scene next, LoadSceneMode mode)
    {
        // シーン切り替え後のスクリプトを取得
        var playerStates = GameObject.FindWithTag("Player").GetComponent<PlayerStates>();
        
        // データを渡す処理
        playerStates.ReinforcedDamage = num;

        // イベントから削除
        SceneManager.sceneLoaded -= GameSceneLoaded;
    }
}
