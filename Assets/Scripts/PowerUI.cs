using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Power : MonoBehaviour
{
    public TMP_Text PowerText;
    PlayerStates playerStates;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); //player検索

        if(player == null)
        {
            Debug.Log("No player found in scene. Make sure it has tag 'Player'.");
        }

        playerStates = player.GetComponent<PlayerStates>(); 
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
