using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayFloor : MonoBehaviour
{
    GameObject playerObj;
    [SerializeField] GameObject colliderObj;
    float beforePos = 0;
    float nowPos = 0;
    float playerSp = 0;


    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        beforePos = playerObj.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        nowPos = playerObj.transform.position.y;
        playerSp = nowPos - beforePos;


        if(playerObj.transform.position.y-1.1 >= transform.position.y)
        {
            if(playerSp <= 0){
                colliderObj.SetActive(true);
                
            }
        }
        else
        {
            colliderObj.SetActive(false);
        }
        beforePos = nowPos;
    }
}
