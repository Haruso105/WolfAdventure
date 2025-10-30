using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DiamondFlower : MonoBehaviour
{
    PlayerController pc;
    [SerializeField]GameObject panel;
    [SerializeField]GameObject text1;
    [SerializeField]GameObject text2;
    [SerializeField]GameObject text3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.CompareTag("Player"))
        {
            pc = collision.GetComponent<PlayerController>();
            pc.gameEnd = true;
            Time.timeScale = 0;
            panel.SetActive(true);
            StartCoroutine("ClearCoroutine");
        } 

    }

    IEnumerator ClearCoroutine()  
    {
        yield return new WaitForSecondsRealtime(1);
        text1.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        text2.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        text3.SetActive(true);
    }
}
