using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    public int testScore = 0;

    void Awake()
    {
        CheckInstance();
    }


    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.I))
        {
            testScore += 1;
            Debug.Log("1追加したよん");
        }
    }

    void CheckInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
