using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDamageBall : MonoBehaviour
{
    [SerializeField] private GameObject damageBall;
    [SerializeField] float transformX = 3;
    float time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >= 3)   
        {
            GenerateBall();
            time = 0;
        }
    }

    void GenerateBall()
    {
        float randomNum = Random.Range(0, transformX);
        int RL = Random.Range(0,2);
        if(RL == 0) Instantiate(damageBall, new Vector3(this.transform.position.x + randomNum, this.transform.position.y, 0), Quaternion.identity);
        else Instantiate(damageBall, new Vector3(this.transform.position.x - randomNum, this.transform.position.y, 0), Quaternion.identity);

    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector3(this.transform.position.x + transformX, this.transform.position.y, 0), new Vector3(this.transform.position.x - transformX, this.transform.position.y, 0));
    }
}
