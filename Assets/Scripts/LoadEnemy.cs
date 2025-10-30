using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyStates es = collision.GetComponent<EnemyStates>();

        if(es != null)
        {
            es.isLoaded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyStates es = collision.GetComponent<EnemyStates>();

        if(es != null)
        {
            es.isLoaded = false;
        }
    }

}
