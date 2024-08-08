using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class RandomCondition : Conditional
{
    public int count = 4;
    public SharedInt randomNum;
    public override void OnStart()
    {
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        randomNum.Value = Random.Range(1, count+1);
        // Debug.Log("randomNum is : " + randomNum);
        return TaskStatus.Success;
    }
}
