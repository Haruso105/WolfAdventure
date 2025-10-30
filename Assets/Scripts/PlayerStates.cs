using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStates : MonoBehaviour
{
    public UnityEvent<int> atkChanged;
    //public UnityEvent<int> damageChanged;
    
    [SerializeField]
    private int _reinforcedDamage = 0;

    public int ReinforcedDamage
    { 
        get { return _reinforcedDamage; }
        set
        {
            _reinforcedDamage = value;
            atkChanged?.Invoke(_reinforcedDamage);  //?.Invokeでnullではない時のみ実行される
        }
    }
}
