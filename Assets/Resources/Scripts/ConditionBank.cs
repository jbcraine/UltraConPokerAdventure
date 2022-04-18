using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ConditionBank : MonoBehaviour
{
    public static ConditionBank Bank;
    private void Awake() {
        Bank = this;
    }
    public bool conditionA = true;

    public bool ConditionA
    {
        get { return conditionA; }
        set { conditionA = value; }   
    }
    public bool conditionB = false;

    public bool ConditionB
    {
        get { return conditionB; }
        set { conditionB = value ;}
    }

    public bool conditionC = true;

    public bool ConditionC
    {
        get { return conditionC; }
        set { conditionC = value; }
    }
}
