using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    [SerializeField]  private GameObject camera1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player"){
            camera1.gameObject.SetActive(true);
            Debug.Log("entered");
        }
        
    }

    void OnCollisionExit2D(Collision2D other)
    {

    }
}
